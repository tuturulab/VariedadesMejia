using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para ProveedorWindow.xaml
    /// </summary>
    public partial class ProveedorWindow : Window
    {
        //public event Action<Proveedor> ActualizarProveedor;
        //public Proveedor proveedor;

        //Llamando al evento de la clase padre que instancio esta ventana
        private void LLamarEventoProveedor()
        {
            //ActualizarProveedor?.Invoke(proveedor);
        }

        public PageViewModel ViewModel;

        public ProveedorWindow(PageViewModel viewModel)
        {
            //Defining the viewmodel 
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            //proveedor = new Proveedor();
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Acción del boton insertar
        private void BtnInsertarProveedor(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NombreTextBox.Text) == true)
            {
                MessageBoxResult result = MessageBox.Show("Por favor Ingrese un Nombre",
                                              "Confirmation",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Exclamation);
            }

            else if (String.IsNullOrEmpty(PrecioCostoTextBox.Text))
            {
                MessageBoxResult result = MessageBox.Show("Por favor ingrese el precio del costo original",
                                              "Confirmation",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Exclamation);
            }


            //Si tanto el precio costo, y el nombre que son parametros requeridos no estan vacios, devolvemos el proveedor a la ventana padre
            else
            {
                /*proveedor.Empresa = NombreTextBox.Text;
                proveedor.Lugar_Importacion = LugarImportacionTextBox.Text;
                proveedor.Precio_Costo = double.Parse(PrecioCostoTextBox.Text);
                proveedor.Fecha_Llegada = Fecha_LLegada_Datepick.DisplayDate;

                LLamarEventoProveedor();

                this.Close();
                */
            }

        }


    }
}