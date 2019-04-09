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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public PageViewModel pageViewModel;
        private Producto _Producto;
        private Proveedor _Proveedor;

        //Evento actualizar paginacion 
        public EventHandler UpdatePagination;
        ObservableCollection<Especificacion_producto> EspecificacionList;

        public EditProductWindow(PageViewModel _model, Producto producto)
        {
            InitializeComponent();
            pageViewModel = _model;

            if(producto != null)
            {
                _Producto = producto;
                EspecificacionList = new ObservableCollection<Especificacion_producto>();

                _Producto.Especificaciones_producto.ToList().ForEach(item =>
                {
                    EspecificacionList.Add(item);
                });

                //Emei check
                if(_Producto.Imei_Disponible != 1)
                {
                    ImeiColumn.Visibility = Visibility.Hidden;
                }

                ProductosDatagrid.ItemsSource = EspecificacionList;
            }
        }

        private void SetDataToWindow()
        {
            MarcaTextBox.Text = _Producto.Marca;
            ModeloTextBox.Text = _Producto.Modelo;
            PrecioTextBox.Text = _Producto.Precio_Venta.ToString();
            ComboBoxCredito.SelectedIndex = _Producto.Credito_Disponible == 1 ? 0 : 1;
            ComboBoxImei.SelectedIndex = _Producto.Imei_Disponible == 1 ? 1 : 0;
            ComboBoxGarantia.SelectedIndex = _Producto.Garantia_Disponible == 1 ? 0 : 1;
            CategoriaComboBox.SelectedIndex = GetIndexCategory(_Producto.Tipo_Producto);

            if (ComboBoxImei.Text == "Si")
            {
                ImeiColumn.Visibility = Visibility.Visible;
            }

            if (ComboBoxGarantia.Text == "Si")
            {
                Garantia_Column.Visibility = Visibility.Visible;
            }

            if (ComboBoxGarantia.SelectedIndex > -1 && ComboBoxImei.SelectedIndex > -1)
            {
                //EspecificacionList.Clear();
                ChangeBetweenImei();

                /*
                TextBoxProveedor.Text = _SelectedProduct.Especificaciones_producto.FirstOrDefault()
                    .Proveedor_Producto.DetalleProveedor.Proveedor
                    .Empresa;

                */

                TextBoxCantidad.Text = _Producto.NumeroDeEspecificacionesDisponibles.ToString();
                TextBoxCantidad.IsEnabled = false;

                //AgregarATablaBtn.IsEnabled = false;
                //AgregarATablaBtn.Visibility = Visibility.Hidden;

                //Grid productos - Cambiar los bindings
                //Este codigo no funciona Maykol revisar aca ya que cambiamos la propiedad que se va a mostrar en el codigo xaml con el fin de que solo se puedan meter numeros
                //ImeiColumn.Binding = new Binding("IMEI");
                //Precio_Costo_Column.Binding = new Binding("Proveedor_Producto.DetalleProveedor.Precio_Costo");
                //ProveedorColumn.Binding = new Binding("Proveedor_Producto.DetalleProveedor.Proveedor.Empresa");

            }
        }

        public void BtnActualizarProducto(object sender, RoutedEventArgs e)
        {

        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SelectProveedorClick(object sender, RoutedEventArgs e)
        {
            var selectProveedorWindow = new SelectProveedorWindow(pageViewModel);

            selectProveedorWindow.EventSelectedProveedor += EventoSetProveedor;

            selectProveedorWindow.Show();
        }

        //Seteamos el proveedor escogido
        public void EventoSetProveedor(object sender, EventArgs e)
        {
            _Proveedor = pageViewModel.SelectedProveedorWindow;
            TextBoxProveedor.Text = _Proveedor.Empresa;
        }

        //Si el usuario crea un Proveedor, abrimos la ventana y obtenemos el dato
        private void CreateProveedorClick(object sender, RoutedEventArgs e)
        {
            var addProveedorWindow = new AddProveedorWindow(pageViewModel);

            addProveedorWindow.ActualizarProveedor += EventoSetProveedor;

            addProveedorWindow.Show();
        }

        public void OnComboBoxImeiSelect(object sender, EventArgs e)
        {
            if (ComboBoxImei.Text == "Si")
            {
                ImeiColumn.Visibility = Visibility.Visible;
            }

            else
            {
                ImeiColumn.Visibility = Visibility.Hidden;
            }

            if (ComboBoxGarantia.Text == "Si")
            {
                Garantia_Column.Visibility = Visibility.Visible;
                PanelGarantia.Visibility = Visibility.Visible;
            }
            else
            {
                Garantia_Column.Visibility = Visibility.Hidden;
                PanelGarantia.Visibility = Visibility.Hidden;
            }

            //Si se seleccionaron las dos opciones
            if (ComboBoxGarantia.SelectedIndex > -1 && ComboBoxImei.SelectedIndex > -1)
            {
                EspecificacionList.Clear();
                //EspecificacionesToEditProductoList.Clear();
                ChangeBetweenImei();
            }

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

        public void ChangeBetweenImei()
        {
            PanelImei.Visibility = Visibility.Visible;
            ProductosDatagrid.Visibility = Visibility.Visible;
            InsertarButton.Visibility = Visibility.Visible;
        }

        public int GetIndexCategory(string Category)
        {
            //Iterar sobre el contenido las propiedades del combobox categoria, para obtener una list<string> de las categorias
            List<string> Lista = CategoriaComboBox.Items.Cast<ComboBoxItem>()
                .Select(item => item.Content.ToString()).ToList();

            return Lista.IndexOf(Category);
        }

        public void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }
    }
}
