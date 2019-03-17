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
    /// Lógica de interacción para AgregarClienteWindow.xaml
    /// </summary>
    public partial class AgregarClienteWindow : Window
    {
        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        ObservableCollection<Telefonos> TelefonosList = new ObservableCollection<Telefonos>();

        public AgregarClienteWindow()
        {
            InitializeComponent();
        }

        //Validación
        private void EventoPaginacion()
        {
            if (UpdatePagination != null)
            {
                UpdatePagination(this, EventArgs.Empty);
            }
        }

        public PageViewModel ViewModel;

        public void BtnInsertarCliente(object sender, RoutedEventArgs e)
        {
            try
            {

                if (String.IsNullOrEmpty(NombreTextBox.Text) == false)
                {
                    //Ingresando el Cliente
                    var client = new Cliente()
                    {
                        Nombre = NombreTextBox.Text,
                        Email = EmailTextBox.Text,
                        Domicilio = DomicilioTextBox.Text,
                        Tipo_Pago = "Cordobas",
                    };

                    var Telefonos = new List<Telefono>();
                    //Si el producto tiene Imeis se agregan, de lo contrario no
                    var Cantidad = int.Parse(CantidadTextBox.Text);
                    foreach (var item in TelefonosList)
                    {
                        Telefonos.Add(new Telefono() { Cliente = client, Numero = item.Numero, Empresa = item.Empresa, Tipo_Numero = item.Tipo_Numero });
                    }


                    //Agregamos a la base de datos y actualizamos la paginación
                    ViewModel.AddClient(client, Telefonos);

                    EventoPaginacion();

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
            //Limpiamos la anterior lista
            TelefonosList.Clear();

            //Generamos la cantidad de registros a agregar
            var cantidad = int.Parse(CantidadTextBox.Text);

            for (int i = 0; i < cantidad; i++)
            {
                TelefonosList.Add(new Telefonos() { IdNumero = (i + 1), Numero = " ", Tipo_Numero = " ", Empresa = " " });
            }

            //Los seteamos en el datagris
            TelefonoDatagrid.ItemsSource = TelefonosList;
        }

    }

    //Clase para generar la lista de Telefonos
    public class Telefonos
    {
        public int IdNumero { get; set; }
        public string Numero { get; set; }
        public string Tipo_Numero { get; set; }
        public string Empresa { get; set; }

    }
}
