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
    /// Lógica de interacción para AddToExistentProduct.xaml
    /// </summary>
    public partial class AddToExistentProductWindow : Window
    {
        PageViewModel ViewModel;

        private Proveedor _Proveedor;

        AddProveedorWindow addProveedorWindow;
        SelectProveedorWindow selectProveedorWindow;

        //Evento de Actualizar Paginacion y el numero de elementos disponibles
        public event EventHandler UpdatePagination;

        private Producto _Producto;

        ObservableCollection<EspecificacionClass> EspecificacionList;

        //Numero de registros
        public int n = 0;

        public AddToExistentProductWindow(PageViewModel viewModel, Producto _Product)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            _Producto = _Product;

            EspecificacionList = new ObservableCollection<EspecificacionClass>();

            ProductosDatagrid.ItemsSource = EspecificacionList;

            NombreTextBox.Text = _Product.Marca + " " + _Product.Modelo;


            //Seteamos los campos editables en la tabla
            if (_Product.Imei_Disponible == 1)
            {
                ImeiColumn.Visibility = Visibility.Visible;
            }

            else
            {
                ImeiColumn.Visibility = Visibility.Hidden;
            }

            if (_Product.Garantia_Disponible == 1)
            {
                GarantiaColumn.Visibility = Visibility.Visible;
            }

            else
            {
                GarantiaColumn.Visibility = Visibility.Hidden;
            }

            ProductosDatagrid.Visibility = Visibility.Visible;

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


        //Si el usuario crea un Proveedor, abrimos la ventana y obtenemos el dato
        private void CreateProveedorClick(object sender, RoutedEventArgs e)
        {
            addProveedorWindow = new AddProveedorWindow(ViewModel);

            addProveedorWindow.ActualizarProveedor += EventoSetProveedor;

            addProveedorWindow.Show();
        }

        //Si el usuario decide seleccionar un proveedor existente, también abrimos la ventana y obtenemos el dato
        private void SelectProveedorClick(object sender, RoutedEventArgs e)
        {
            selectProveedorWindow = new SelectProveedorWindow(ViewModel);

            selectProveedorWindow.EventSelectedProveedor += EventoSetProveedor;

            selectProveedorWindow.Show();
        }

        //Seteamos el proveedor escogido
        public void EventoSetProveedor(object sender, EventArgs e)
        {
            _Proveedor = ViewModel.SelectedProveedorWindow;
            TextBoxProveedor.Text = _Proveedor.Empresa;

        }

        //Ingresar Existencias
        public void BtnInsertarProducto(object sender, RoutedEventArgs e)
        {
            if (EspecificacionList.Count < 1)
            {
                MessageBoxResult result = MessageBox.Show("Por favor Especifique almenos 1 en los campo asociados a este producto",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

            else
            {
                List<Especificacion_producto> ListaEspecificaciones = new List<Especificacion_producto>();

                foreach (var i in EspecificacionList)
                {
                    var ElementoProducto = new Especificacion_producto();

                    ElementoProducto.Producto = _Producto;
                    ElementoProducto.Descripcion = i.Descripcion;

                    var ProveedorAsignado = new DetalleProveedor()
                    {
                        Garantia_Original = i.Garantia,
                        Precio_Costo = i.Precio_Costo,
                        Proveedor = ViewModel.GetProveedor(i.ProveedorId),
                    };

                    var ListProductos = new List<Especificacion_producto>();
                    ListProductos.Add(ElementoProducto);

                    var TablaSeguimiento = new Proveedor_producto()
                    {
                        Especificacion_Productos = ListProductos,
                        DetalleProveedor = ProveedorAsignado,
                    };

                    //ProveedorAsignado.Proveedor_producto = TablaSeguimiento;

                    ElementoProducto.Proveedor_Producto = TablaSeguimiento;
                    ElementoProducto.Proveedor_Producto.DetalleProveedor = ProveedorAsignado;

                    //Si la columnas estan visibles, agregar el dato insertado a la relacion
                    if (GarantiaColumn.Visibility == Visibility.Visible)
                    {
                        ElementoProducto.Garantia = i.Garantia;
                    }

                    if (ImeiColumn.Visibility == Visibility.Visible)
                    {
                        ElementoProducto.IMEI = i.Imei;
                    }

                    ListaEspecificaciones.Add(ElementoProducto);
                    
                }
                //Agregamos existencias al producto
                ViewModel.AddEspecificacionProducto(ListaEspecificaciones);

                //Actualizamos pero validando si hay una busqueda y no afectarla
                if (ViewModel.SearchProductList == null)
                {
                    ViewModel.UpdateProducts(3);
                }

                else
                {
                    ViewModel.UpdateProducts(3, ViewModel.SearchProductList);
                }

            }
            this.Close();
        }

        //Agregar Elementos a la tabla
        public void AgregarATablaClick(object sender, RoutedEventArgs e)
        {
            if (_Proveedor != null)
            {
                if (TextBoxCantidad.Text != String.Empty)
                {
                    for (int i = 0; i < int.Parse(TextBoxCantidad.Text); i++)
                    {
                        n++;
                        EspecificacionList.Add(new EspecificacionClass() { Numero = (n), Descripcion = " ", Imei = " ", Proveedor = _Proveedor.Empresa, ProveedorId = _Proveedor.IdProveedor });
                    }


                }

                else
                {
                    MessageBoxResult result = MessageBox.Show("Por favor ingrese La cantidad de Datos a ingresar que sean de ese mismo proveedor",
                                                  "Confirmation",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Exclamation);
                }
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Por favor seleccione un proveedor para agregar a la tabla",
                                                  "Confirmation",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Exclamation);
            }
        }
    }

    
}
