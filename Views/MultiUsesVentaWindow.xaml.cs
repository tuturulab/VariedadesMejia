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
    /// Lógica de interacción para MultiUsesVentaWindow.xaml
    /// </summary>
    public partial class MultiUsesVentaWindow : Window
    {
        PageViewModel ViewModel;
        SelectClientWindow window;
        MultiUsesClienteWindow window2;
        SelectProductWindow ProductWindow;

        public Cliente cliente;
        public Venta venta;
        public Especificacion_producto _producto;

        private double TotalPago = 0;

        public List<Especificacion_producto> Especificacion_Productos;

        ObservableCollection<Especificacion_producto> ProductosList = new ObservableCollection<Especificacion_producto>();

        public MultiUsesVentaWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            //Los seteamos en el datagris
            ProductosDatagrid.ItemsSource = ProductosList;
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

      
        private void BtnInsertarVenta(object sender, RoutedEventArgs e)
        {
            
            

        }
        
        //Boton de agregar productos a la tabla de venta
        private void BtnAddProduct (object sender, RoutedEventArgs e)
        {
            if (_producto != null)
            {
                //Agregamos a la lista
                ProductosList.Add(_producto);

                //Actualizamos el total a pagar
                foreach (var i in ProductosList)
                {
                    TotalPago = TotalPago + i.Precio;
                }

                PrecioFinalTextBox.Text = TotalPago.ToString();
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Por favor Seleccione el producto que desea ingresar a la venta" + "",
                                             "Confirmation",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation);
            }
           

        }

        public void OnComboBoxTipoPago(object sender, EventArgs e)
        {
            if (TipoPagoComboBox.Text == "Crédito")
            {
                PagosPanel.Visibility = Visibility.Visible;
                PagosDataGridPanel.Visibility = Visibility.Visible;
                PlazosPanel.Visibility = Visibility.Visible;
            }

            else
            {
                

                PagosPanel.Visibility = Visibility.Hidden;
                PagosDataGridPanel.Visibility = Visibility.Hidden;
                PlazosPanel.Visibility = Visibility.Hidden;
            }
        }


        private void BtnInsertarPagos(object sender, RoutedEventArgs e)
        {
            // do something
        }

        //Recibiendo el id creado
        public void EventoActualizarCliente(object sender, EventArgs e)
        {
            //Obtenemos el cliente seleccionado de la ventana SelectClient
            cliente = ViewModel.SelectedClientWindow;
            ClienteTextBox.Text = cliente.Nombre;
        }

        //Pasar cliente
        public void EventoPasarProducto (object sender, EventArgs e)
        {
            _producto = ViewModel.SelectedProductWindow;
            ProductoTextBox.Text = _producto.Nombre;
        }

        public void EventoInsertarCliente (object sender, EventArgs e)
        {
            cliente = window2.cliente;
            ClienteTextBox.Text = cliente.Nombre;
        }

        private void BtnSelectProduct(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            ProductWindow = new SelectProductWindow(ViewModel);

            ProductWindow.UpdateProduct += new EventHandler(EventoPasarProducto);
            ProductWindow.Show();
        }


        private void BtnSelectClient(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new SelectClientWindow(ViewModel);

            //Subscribimos al evento
            window.EventSelectedClient += new EventHandler(EventoActualizarCliente);
            window.Show();
        }

        private void BtnCreateClient (object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window2 = new MultiUsesClienteWindow(ViewModel);

            //Subscribimos al evento
            window2.PassClient += new EventHandler(EventoInsertarCliente);
           
            window2.Show();
        }
    }
}
