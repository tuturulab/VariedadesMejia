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
    /// Lógica de interacción para MultiUsesPedidoWindow.xaml
    /// </summary>
    public partial class MultiUsesPedidoWindow : Window
    {
        PageViewModel ViewModel;
        ObservableCollection<Especificacion_pedido> EspecificacionList;

        SelectClientWindow window;
        MultiUsesClienteWindow window2;
        private Cliente cliente;

        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        
        public MultiUsesPedidoWindow(PageViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            EspecificacionList = new ObservableCollection<Especificacion_pedido>();
            ProductosDatagrid.ItemsSource = EspecificacionList;
        }

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        //Recibiendo el id creado
        public void EventoActualizarCliente(object sender, EventArgs e)
        {
            //Obtenemos el cliente seleccionado de la ventana SelectClient
            cliente = ViewModel.SelectedClientWindow;
            ClienteTextBox.Text = cliente.Nombre;
        }

        public void EventoInsertarCliente(object sender, EventArgs e)
        {
            cliente = window2.cliente;
            ClienteTextBox.Text = cliente.Nombre;
        }

        private void BtnSelectClient(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un cliente
            window = new SelectClientWindow(ViewModel);

            //Subscribimos al evento
            window.EventSelectedClient += new EventHandler(EventoActualizarCliente);
            window.Show();
        }



        private void BtnInsertarPedido (object sender, RoutedEventArgs e)
        {
            if (cliente == null)
            {
                MessageBoxResult result = MessageBox.Show("Por favor Seleccione el cliente quien realizó el pedido",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

            else
            {
                if (EspecificacionList.Count < 1)
                {
                    MessageBoxResult result = MessageBox.Show("Por favor Especifique Almenos 1 producto encargado",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
                }
                else
                {
                    //Finalmente Agregamos
                    Pedido pedido = new Pedido()
                    {
                        cliente = cliente,
                        Fecha_Pedido = DateTime.Now,
                        Estado_Pedido = "En trámite"
                    };

                    List<Especificacion_pedido> ListaProductos = new List<Especificacion_pedido>();

                    //Agregamos y asignamos los productos a este pedido
                    foreach (var i in EspecificacionList)
                    {
                        var ProductoPedido = new Especificacion_pedido()
                        {     
                            Cantidad = i.Cantidad,
                            Descripcion = i.Descripcion,
                            Marca = i.Marca,
                            Modelo = i.Modelo,
                            Tipo_Producto = i.Tipo_Producto,
                            Pedido = pedido,
                        };
                        ListaProductos.Add(ProductoPedido);
                    }

                       
                    ViewModel.AddPedido(pedido);
                    ViewModel.AddEspecificacionPedido(ListaProductos);
                    EventoPaginacion();

                    if (MessageBox.Show("Se ha ingresado correctamente el pedido, ¿desea seguir ingresando pedidos?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                    else
                    {
                        ClienteTextBox.Text = String.Empty;
                        cliente = null;
                        CategoriaComboBox.Text = String.Empty;


                        EspecificacionList.Clear();
                    }

                }
            }
        }

        //Funcion que permite el click rapido a una tabla
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

        private void BtnBorrarClick (object sender, RoutedEventArgs e)
        {
            EspecificacionList.Remove(ViewModel.SelectedEspecificacionPedido);
        }

        private void BtnCreateClient(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window2 = new MultiUsesClienteWindow(ViewModel);

            //Subscribimos al evento
            window2.PassClient += new EventHandler(EventoInsertarCliente);

            window2.Show();
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Agregar pedidos a la lista
        private void BtnAddProduct(object sender, RoutedEventArgs e)
        {
            if (CategoriaComboBox.Text == String.Empty)
            {
                MessageBoxResult result = MessageBox.Show("Por favor Especifique el tipo de producto",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }
            else
            {
                EspecificacionList.Add(new Especificacion_pedido() { Tipo_Producto = CategoriaComboBox.Text });
            }
        }

    }
}
