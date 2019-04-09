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
    /// Lógica de interacción para MultiUsesImportacionWindow.xaml
    /// </summary>
    public partial class MultiUsesImportacionWindow : Window
    {
        PageViewModel ViewModel;

        ObservableCollection<Producto_importado> ProductosList;
        ObservableCollection<Especificacion_pedido> PedidosList;
        private SelectPedidoWindow PedidoWindow;


        private Pedido _pedido;
        
        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        public MultiUsesImportacionWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            ProductosList = new ObservableCollection<Producto_importado>();
            PedidosList = new ObservableCollection<Especificacion_pedido>();

            //Los seteamos en el datagris
            ProductosDatagrid.ItemsSource = ProductosList;
            PedidosDatagrid.ItemsSource = PedidosList;
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }
        

        private void BtnSelectPedido(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            PedidoWindow = new SelectPedidoWindow(ViewModel);

            PedidoWindow.EventSelectedPedido += new EventHandler(EventoPasarPedido);
            PedidoWindow.Show();
        }

        //Obtener el pedido seleccionado
        public void EventoPasarPedido(object sender, EventArgs e)
        {
            _pedido = ViewModel.SelectedPedidoWindow;
            PedidoTextBox.Text = "Pedido de " + _pedido.NombreCliente;

            PedidosList.Clear();
            
            PedidosList = new ObservableCollection<Especificacion_pedido>(_pedido.Especificaciones_pedido);
            PedidosDatagrid.ItemsSource = PedidosList;
        }


        private void BtnBorrarClick (object sender, RoutedEventArgs e)
        {
            var seleccionado = ViewModel.SelectedProductImportado;

            ProductosList.Remove(seleccionado);
        }

        private void BtnInsertarImportacion (object sender, RoutedEventArgs e)
        {
            if (ProductosList.Count == 0)
            {
                MessageBoxResult result = MessageBox.Show("Por favor Ingrese el numero de productos que encargará",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Question);
            }
            else
            {
                if (_pedido == null)
                {
                    MessageBoxResult result = MessageBox.Show("Por favor Seleccione el pedido del cliente que le encargó este producto",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Question);
                }

                else
                {
                    //Finalmente agregamos
                    DetalleProveedor detalleProveedor = new DetalleProveedor();
                    detalleProveedor.Pedido = _pedido;

                    
                    detalleProveedor.Numero_Seguimiento = SeguimientoTextBox.Text;


                    //Calculamos el precio total
                    double precio = 0;

                    foreach (var i in ProductosList.ToList() )
                    {
                        precio = precio +  (i.Precio * i.Cantidad) ;
                    }

                    detalleProveedor.Precio_Costo = precio;

                    ViewModel.AddImportacion(detalleProveedor, ProductosList.ToList() );
                    

                    //Cambiamos el estado del pedido
                    ViewModel.ChangeEstatusPedido(_pedido);

                    MessageBoxResult result = MessageBox.Show("Se ha ingresado correctamente",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);


                    EventoPaginacion();

                    this.Close();
                }
            }
        }

        private void BtnInsertarProducto (object sender, RoutedEventArgs e)
        {
            ProductosList.Add(new Producto_importado());
        }

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
