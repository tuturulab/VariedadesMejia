using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Variedades.Models;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para MultiUsesClienteWindow.xaml
    /// </summary>
    public partial class MultiUsesClienteWindow : Window
    {
        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        //Evento de Pasar cliente
        public event EventHandler PassClient;


        ObservableCollection<TelefonosAddList> TelefonosList = new ObservableCollection<TelefonosAddList>();

        public PageViewModel ViewModel;
        public Cliente cliente;

        private List<Telefono> TelefonoMainList;
        
        public MultiUsesClienteWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            //Los seteamos en el datagris
            TelefonoDatagrid.ItemsSource = TelefonosList;
        }

        //Si la ventana de agregar Cliente es llamada desde ventas o pedido
        private void EventoPasarCliente()
        {
            PassClient?.Invoke(this, EventArgs.Empty);
        }
        
        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        public void BtnInsertarCliente(object sender, RoutedEventArgs e)
        {
            try
            {

                if (String.IsNullOrEmpty(NombreTextBox.Text) == false)
                {
                    if (String.IsNullOrEmpty(DiaPago1TextBox.Text) == true || int.Parse(DiaPago1TextBox.Text) >31 || int.Parse(DiaPago1TextBox.Text) < 1)
                    {
                        MessageBoxResult result = MessageBox.Show("Por Favor Ingrese almenos un dia de pago, y asegurese de que sea entre 1 y 30 dias", "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
                    }
                    
                    else
                    {
                        //Ingresando el Cliente
                        cliente = new Cliente()
                        {
                            Nombre = NombreTextBox.Text,
                            Email = EmailTextBox.Text,
                            Domicilio = DomicilioTextBox.Text,
                            Tipo_Pago = TipoPagoComboBox.Text,
                            Cedula = CedulaTextBox.Text,
                            Compania = CompañiaTextBox.Text,
                            Fecha_Pago_1 = int.Parse(DiaPago1TextBox.Text),
                        };


                        //Parametro opcional
                        if (String.IsNullOrEmpty (DiaPago2TextBox.Text) == false )
                        {
                            cliente.Fecha_Pago_2 = int.Parse(DiaPago2TextBox.Text);
                        }


                        TelefonoMainList = new List<Telefono>();

                        foreach (var i in TelefonosList)
                        {
                            TelefonoMainList.Add(new Telefono()
                            {
                                Cliente = cliente,
                                Empresa = i.Empresa ,
                                Tipo_Numero = i.Tipo_Numero,
                                Numero = i.Numero
                            });
                        }

                        ViewModel.AddClient(cliente, TelefonoMainList);

                        EventoPaginacion();
                       

                        //Si no se le subscribio un evento por tanto fue llamado desde la pagina cliente
                        if (PassClient == null)
                        {
                            if (MessageBox.Show("Se ha ingresado correctamente el cliente, ¿desea seguir ingresando clientes?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            {
                                this.Close();
                            }
                            else
                            {
                                //Limpiamos los campos para seguir insertando
                                NombreTextBox.Text = String.Empty;
                                EmailTextBox.Text = String.Empty;
                                DomicilioTextBox.Text = String.Empty;
                                TipoPagoComboBox.Text = String.Empty;
                                DiaPago1TextBox.Text = String.Empty;
                                DiaPago2TextBox.Text = String.Empty;
                                CompañiaTextBox.Text = String.Empty;
                                CedulaTextBox.Text = String.Empty;
                                TelefonosList.Clear();
                            }
                        }

                        //Si fue llamado desde una subventana
                        else
                        {
                            EventoPasarCliente();
                            this.Close();
                        }
                        
                    }
                    
                }

                else
                {
                    MessageBoxResult result = MessageBox.Show("Ingrese el nombre del cliente por favor",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
                }

            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Error al ingresar en la base de datos",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Agregar telefonos
        private void BtnInsertarTelefono(object sender, RoutedEventArgs e)
        {
            var Tipo_Numero = TipoTelefonoComboBox.Text;

            var Empresa = TipoCompañiaComboBox.Text;

            if (Tipo_Numero != String.Empty && Empresa != String.Empty)
            {
                TelefonosList.Add(new TelefonosAddList() { IdNumero = (TelefonosList.Count() + 1), Numero = " ", Tipo_Numero = Tipo_Numero, Empresa = Empresa });
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Por favor ingrese los campos requeridos para añadir un telefono",
                                             "Confirmation",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation);
            }
            
            
        }

        private void BtnBorrarClick (object sender, RoutedEventArgs e)
        {
            var Numero = ViewModel.SelectedTelefonoAdd;

            TelefonosList.Remove(Numero);

        }

        //Metodo para que el datagrid sea simple clickeable
        private void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

                Control control = GetFirstChildByType<Control>(e.OriginalSource as DataGridCell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }

        private T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild((prop), i) as DependencyObject;
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    return castedProp;

                castedProp = GetFirstChildByType<T>(child);

                if (castedProp != null)
                    return castedProp;
            }
            return null;
        }

        private void DiaPago2TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DiaPago1TextBox.Text != String.Empty && DiaPago2TextBox.Text != String.Empty)
                PagoCheck.Visibility = Visibility.Hidden;
            else
                PagoCheck.Visibility = Visibility.Visible;
        }

        private void DiaPago1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DiaPago1TextBox.Text != String.Empty && DiaPago2TextBox.Text != String.Empty)
                PagoCheck.Visibility = Visibility.Hidden;
            else
                PagoCheck.Visibility = Visibility.Visible;
        }

        private void NombreTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NombreTextBox.Text != String.Empty)
                NombreCheck.Visibility = Visibility.Hidden;
            else
                NombreCheck.Visibility = Visibility.Visible;
        }
    }

    //Clase para generar la lista de Telefonos
    public class TelefonosAddList
    {
        public int IdNumero { get; set; }
        public string Numero { get; set; }
        public string Tipo_Numero { get; set; }
        public string Empresa { get; set; }

    }
}
