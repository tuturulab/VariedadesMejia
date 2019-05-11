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

        ObservableCollection<Especificacion_producto> ProductosList;

        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;
        
        public MultiUsesVentaWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            ProductosList = new ObservableCollection<Especificacion_producto>(); 
    
            //Los seteamos en el datagris
            ProductosDatagrid.ItemsSource = ProductosList;

            //Seteamos los productos disponibles
            ViewModel.FillEspecificacionesProducts();
        }

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

      
        private void BtnInsertarVenta(object sender, RoutedEventArgs e)
        {
            if (OrdenPagareTextBox.Text != String.Empty)
            {
                int Cantidad = ViewModel.ExistPagare(OrdenPagareTextBox.Text);

                Console.WriteLine(Cantidad);

                if (Cantidad > 0 )
                {
                    MessageBoxResult result = MessageBox.Show("Esta Orden Pagare ya existe, revise nuevamente" + "",
                                             "Confirmation",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation);
                }

                else
                {
                    try
                    {

                        //Validando campos
                        if (ProductosList.Count < 1)
                        {
                            MessageBoxResult result = MessageBox.Show("Por Favor ingrese almenos un producto a vender antes de agregar la venta" + "",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation);
                        }

                        else
                        {
                            if (TipoPagoComboBox.Text == String.Empty)
                            {
                                MessageBoxResult result = MessageBox.Show("Por Favor escriba el tipo de pago a la venta, si es de contado, o si es de crédito" + "",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation);
                            }

                            else
                            {
                                int contadorProductosGarantia = 0;
                                //Haciendo un recorrido en la lista de productos, si algun producto tiene garantia, es necesario que se le asocie un cliente
                                foreach (var i in ProductosList)
                                {
                                    if (i.Producto.Garantia_Disponible == 1)
                                    {
                                        contadorProductosGarantia++;
                                    }

                                    i.Vendido = "Si";

                                    ViewModel.SetProductosVendidos(i);
                                }

                                if (contadorProductosGarantia > 0 && cliente == null)
                                {
                                    MessageBoxResult result = MessageBox.Show("Por Favor ingrese el cliente que desea comprar los productos, puesto que uno de los productos escogidos tiene Garantía" + "",
                                                    "Confirmation",
                                                    MessageBoxButton.OK,
                                                    MessageBoxImage.Exclamation);
                                }

                                else
                                {
                                    //Finalmente agregamos 
                                    var venta = new Venta()
                                    {
                                        Fecha_Venta = DateTime.Now,
                                        MontoVenta = TotalPago,
                                        Tipo_Venta = TipoPagoComboBox.Text,
                                        Especificaciones_producto = ProductosList,
                                        Orden_Pagare = OrdenPagareTextBox.Text
                                    };

                                    List<Pago> Pagos = new List<Pago>();

                                    //Validamos que datos insertaremos según si es una venta al crédito o venta al contado
                                    if (TipoPagoComboBox.Text == "Crédito")
                                    {
                                        venta.VentaCompletada = "No";
                                    }
                                    //Venta al contado
                                    else
                                    {
                                        venta.VentaCompletada = "Si";
                                        var pago = new Pago
                                        {
                                            Venta = venta,
                                            Monto = TotalPago,
                                            Fecha_Pago = DateTime.Now
                                        };
                                        venta.Pagos.Add(pago);
                                    }

                                    //Si el producto vendido tiene un cliente asociado
                                    if (ClienteTextBox.Text != string.Empty)
                                    {
                                        venta.Cliente = cliente;
                                    }

                                    //Finalmente agregamos la venta y actualizamos la pagina venta
                                    ViewModel.AddVenta(venta);

                                    EventoPaginacion();

                                    MessageBoxResult result = MessageBox.Show("Se ha ingresado correctamente la venta",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Information);

                                    this.Close();

                                    /*
                                    if (MessageBox.Show("Se ha ingresado correctamente la venta, ¿desea seguir ingresando ventas?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                    {
                                        this.Close();
                                    }
                                    else
                                    {
                                        CantidadTextBox.Text = String.Empty;
                                        ProductoTextBox.Text = String.Empty;
                                        PrecioFinalTextBox.Text = String.Empty;
                                        TotalPago = 0;
                                        cliente = null;
                                        ClienteTextBox.Text = String.Empty;
                                        TipoPagoComboBox.Text = String.Empty;

                                        PagosPanel.Visibility = Visibility.Hidden;

                                        ProductosList.Clear();

                                        ViewModel.FillEspecificacionesProducts();
                                    }

                                    */
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBoxResult result = MessageBox.Show("Error al ingresar la venta, revise si todos los datos fueron escritos correctamente" + "",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation);
                    }
                }
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Inserte por favor la orden pagare" + "",
                                             "Confirmation",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation);
            }


            
        }

        //Boton para borrar productos del datagrid
        private void BtnBorrarClick (object sender, RoutedEventArgs e)
        {
            ViewModel.especificacion_Productos.Add(ViewModel.SelectedEspecificacionProducto);
            ProductosList.Remove(ViewModel.SelectedEspecificacionProducto);
            ObtenerTotalPago();
        }

        private void ObtenerTotalPago()
        {
            TotalPago = 0;
            //Actualizamos el total a pagar
            foreach (var i in ProductosList)
            {
                TotalPago = TotalPago + i.Precio;
            }

            PrecioFinalTextBox.Text = TotalPago.ToString();
        }
        
        //Boton de agregar productos a la tabla de venta
        private void BtnAddProduct (object sender, RoutedEventArgs e)
        {
            if (_producto != null)
            {
                //Agregamos a la lista
                ProductosList.Add(_producto);

                TotalPago = 0;

                ObtenerTotalPago();

                PrecioFinalTextBox.Text = TotalPago.ToString();

                //Reseteamos los valores
                _producto = null;
                ProductoTextBox.Text = String.Empty;
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Por favor Seleccione el producto que desea ingresar a la venta" + "",
                                             "Confirmation",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation);
            }
           

        }

        //Cambiar la interfaz segun el tipo de pagos
        public void OnComboBoxTipoPago(object sender, EventArgs e)
        {
            if (TipoPagoComboBox.Text == "Crédito")
            {
                PagosPanel.Visibility = Visibility.Visible;
            }

            else
            {
                CantidadTextBox.Text = String.Empty;

                PagosPanel.Visibility = Visibility.Hidden;
            }
        }

       
        //Recibiendo el id creado
        public void EventoActualizarCliente(object sender, EventArgs e)
        {
            //Obtenemos el cliente seleccionado de la ventana SelectClient
            cliente = ViewModel.SelectedClientWindow;
            ClienteTextBox.Text = cliente.Nombre;
        }

        //Obtener el cliente desde la ventana de productos
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
            //Iniciamos la ventana de crear un cliente
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

        private void OrdenPagareTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OrdenPagareTextBox.Text != String.Empty)
                PagareCheck.Visibility = Visibility.Hidden;
            else
                PagareCheck.Visibility = Visibility.Visible;
        }
    }
}
