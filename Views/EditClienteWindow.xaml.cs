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
    /// Interaction logic for EditClienteWindow.xaml
    /// </summary>
    public partial class EditClienteWindow : Window
    {

        public PageViewModel pageViewModel;
        private Cliente _Cliente;

        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        ObservableCollection<Telefono> _TelefonosList;


        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        public EditClienteWindow(PageViewModel model, Cliente cliente)
        {
            pageViewModel = model;
            DataContext = pageViewModel;
            InitializeComponent();
            
            if(cliente != null)
            {
                _Cliente = cliente;
                _TelefonosList = new ObservableCollection<Telefono>();

                SetDatatoWindow();

                //Add items to observable
                _Cliente.Telefonos.ToList().ForEach(item => _TelefonosList.Add(item));
                TelefonoDatagrid.ItemsSource = _TelefonosList;

            }
        }

        public void SetDatatoWindow()
        {
            NombreTextBox.Text = _Cliente.Nombre;
            EmailTextBox.Text = _Cliente.Email;
            DomicilioTextBox.Text = _Cliente.Domicilio;
            CompaniaTextBox.Text = _Cliente.Compania;
            CedulaTextBox.Text = _Cliente.Cedula;

            if (_Cliente.Fecha_Pago_1 != null)
            {
                DiaPago1TextBox.Text = _Cliente.Fecha_Pago_1.ToString();
            }

            if(_Cliente.Fecha_Pago_2 != null)
            {
                DiaPago2TextBox.Text = _Cliente.Fecha_Pago_2.ToString();
            }

            TipoPagoComboBox.SelectedIndex = _Cliente.Tipo_Pago == "Presencial" ? 0 : 1;

            //TipoTelefonoComboBox.SelectedIndex = GetIndexTipoTelefono(_Cliente.Telefonos.FirstOrDefault().Tipo_Numero);
            //TipoCompaniaComboBox.SelectedIndex = GetIndexTipoTelefonoComp(_Cliente.Telefonos.FirstOrDefault().Empresa);
        }

        public int GetIndexTipoPago(string Category)
        {
            //Iterar sobre el contenido las propiedades del combobox categoria, para obtener una list<string> de las categorias
            List<string> Lista = TipoPagoComboBox.Items.Cast<ComboBoxItem>()
                .Select(item => item.Content.ToString()).ToList();

            return Lista.IndexOf(Category);
        }

        public int GetIndexTipoTelefono(string Category)
        {
            //Iterar sobre el contenido las propiedades del combobox categoria, para obtener una list<string> de las categorias
            List<string> Lista = TipoTelefonoComboBox.Items.Cast<ComboBoxItem>()
                .Select(item => item.Content.ToString()).ToList();

            return Lista.IndexOf(Category);
        }


        public int GetIndexTipoTelefonoComp(string Category)
        {
            //Iterar sobre el contenido las propiedades del combobox categoria, para obtener una list<string> de las categorias
            List<string> Lista = TipoCompaniaComboBox.Items.Cast<ComboBoxItem>()
                .Select(item => item.Content.ToString()).ToList();

            return Lista.IndexOf(Category);
        }

        //Agregar telefonos
        private void BtnInsertarTelefono(object sender, RoutedEventArgs e)
        {
            var Tipo_Numero = TipoTelefonoComboBox.Text;

            var Empresa = TipoCompaniaComboBox.Text;

            if (Tipo_Numero != String.Empty && Empresa != String.Empty)
            {
                _TelefonosList.Add(new Telefono() { Numero = " ", Tipo_Numero = Tipo_Numero, Empresa = Empresa });
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Por favor ingrese los campos requeridos para añadir un telefono",
                                             "Confirmation",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation);
            }
        }

        private void ActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(NombreTextBox.Text) == false)
                {
                    if (String.IsNullOrEmpty(DiaPago1TextBox.Text) == true || int.Parse(DiaPago1TextBox.Text) > 31 || int.Parse(DiaPago1TextBox.Text) < 1)
                    {
                        MessageBoxResult result = MessageBox.Show("Por Favor Ingrese almenos un dia de pago, y asegurese de que sea entre 1 y 30 dias", "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        //Get all the data
                        _Cliente.Nombre = NombreTextBox.Text;
                        _Cliente.Email = EmailTextBox.Text;
                        _Cliente.Domicilio = DomicilioTextBox.Text;
                        _Cliente.Tipo_Pago = TipoPagoComboBox.Text;
                        _Cliente.Cedula = CedulaTextBox.Text;
                        _Cliente.Compania = CompaniaTextBox.Text;
                        _Cliente.Fecha_Pago_1 = int.Parse(DiaPago1TextBox.Text);

                        //Parametro opcional
                        if (String.IsNullOrEmpty(DiaPago2TextBox.Text) == false)
                        {
                            _Cliente.Fecha_Pago_2 = int.Parse(DiaPago2TextBox.Text);
                        }

                        ICollection<Telefono> i_telefonos = _TelefonosList as ICollection<Telefono>;

                        //Iterate over 2 collections

                        _Cliente.Telefonos.Clear();

                        i_telefonos.ToList().ForEach(item =>
                        {
                            _Cliente.Telefonos.Add(item);
                        });

                        //i_telefonos.Zip(_Cliente.Telefonos, (toItem, item) =>
                        //{
                        //    item.Empresa = toItem.Empresa;
                        //    item.Numero = toItem.Numero;
                        //    item.Tipo_Numero = toItem.Tipo_Numero;
                        //    return true;
                        //});

                        //Do update
                        pageViewModel.UpdateCliente(_Cliente);

                        EventoPaginacion();

                        this.Close();
                    }
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

        private void BtnBorrarClick(object sender, RoutedEventArgs e)
        {
            //Get selected number
            var Numero = pageViewModel.SelectedEditTelefono;

            //Delete form observable
            _TelefonosList.Remove(Numero);

            //Delete also from Cliente.Telefonos;
            _Cliente.Telefonos.Remove(Numero);

        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
    }
}
