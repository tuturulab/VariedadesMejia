using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
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
    /// Lógica de interacción para MultiUsesProductoWindow.xaml
    /// </summary
    public partial class MultiUsesProductoWindow : Window
    {
        
        public PageViewModel ViewModel;
        public Proveedor _Proveedor;
        private Producto _Product;
        private Producto _SelectedProduct;

        AddProveedorWindow addProveedorWindow;
        SelectProveedorWindow selectProveedorWindow;

        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        //Cambia la ventana segun si tiene imei o no, 0 No se ha seleccionado, 1 = Si hay Imei , 2 = No hay Imei
        public int ImeiActivate = 0;

        ObservableCollection<EspecificacionClass> EspecificacionList;

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        public MultiUsesProductoWindow(PageViewModel viewModel, Producto producto = null)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            EspecificacionList = new ObservableCollection<EspecificacionClass>();
            ProductosDatagrid.ItemsSource = EspecificacionList;

            if(producto != null)
            {
                _SelectedProduct = producto;
                SetAndChangeWindowAppareance();
            }

        }

        private void SetAndChangeWindowAppareance()
        {
            WindowTitle.Text = "Editar Producto";
            MarcaTextBox.Text = _SelectedProduct.Marca;
            ModeloTextBox.Text = _SelectedProduct.Modelo;
            PrecioTextBox.Text = _SelectedProduct.Precio_Venta.ToString();
            ComboBoxCredito.SelectedIndex = _SelectedProduct.Credito_Disponible == 1 ? 0 : 1;
            ComboBoxImei.SelectedIndex = _SelectedProduct.Imei_Disponible == 1 ? 0 : 1;
            ComboBoxGarantia.SelectedIndex = _SelectedProduct.Garantia_Disponible == 1 ? 0 : 1;
            CategoriaComboBox.SelectedIndex = GetIndexCategory(_SelectedProduct.Tipo_Producto);

            if(ComboBoxImei.Text == "Si")
            {
                ImeiColumn.Visibility = Visibility.Visible;
            }

            if (ComboBoxGarantia.Text == "Si")
            {
                GarantiaColumn.Visibility = Visibility.Visible;
            }

            if (ComboBoxGarantia.SelectedIndex > -1 && ComboBoxImei.SelectedIndex > -1)
            {
                //EspecificacionList.Clear();
                ChangeBetweenImei();
                
            }
        }

        public int GetIndexCategory(string Category)
        {
            //Iterar sobre el contenido las propiedades del combobox categoria, para obtener una list<string> de las categorias
            List<string> Lista = CategoriaComboBox.Items.Cast<ComboBoxItem>()
                .Select(item => item.Content.ToString()).ToList();

            return Lista.IndexOf(Category);
        }

        public void OnComboBoxImeiSelect (object sender, EventArgs e)
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
                GarantiaColumn.Visibility = Visibility.Visible;
            }

            else
            {
                GarantiaColumn.Visibility = Visibility.Hidden;
            }

            //Si se seleccionaron las dos opciones
            if (ComboBoxGarantia.SelectedIndex > -1 && ComboBoxImei.SelectedIndex > -1)
            {
                EspecificacionList.Clear();
                ChangeBetweenImei();
            }

        }

        //Si el usuario Agrega elementos a la tabla
        private void AgregarATablaClick(object sender, RoutedEventArgs e)
        {
            if (_Proveedor != null)
            {
                if ( TextBoxCantidad.Text != String.Empty  )
                {
                    for (int i=0; i< int.Parse (TextBoxCantidad.Text); i++  )
                    {
                        EspecificacionList.Add(new EspecificacionClass() {Numero = (i+1),  Descripcion = " ", Imei = " ", Proveedor = _Proveedor.Empresa, ProveedorId = _Proveedor.IdProveedor });
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


        //Esta Funcion se encarga de cambiar la apariencia de la ventana agregar Productos, segun si el producto tiene Imeis o no
        public void ChangeBetweenImei()
        {
            PanelImei.Visibility = Visibility.Visible;
            ProductosDatagrid.Visibility = Visibility.Visible;
            InsertarButton.Visibility = Visibility.Visible;
            
        }

        //Si el usuario crea un Proveedor, abrimos la ventana y obtenemos el dato
        private void CreateProveedorClick (object sender, RoutedEventArgs e)
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

        //private void 



        /*
        public void SetProductoDatatoView(Producto producto)
        {
            _producto = producto;

            //Seteo de datos a la vista
            Title = "Editar Producto";
            WindowTitle.Text = "Editar Producto";
            InsertarButton.Content = "Editar Producto";
            ProveedoresBtn.Content = "Editar Proveedor";

            MarcaTextBox.Text = producto.Marca;
            ModeloTextBox.Text = producto.Modelo;
            PrecioTextBox.Text = producto.Precio_Venta.ToString();
            CategoriaComboBox.SelectedIndex = GetIndexCategory(producto.Tipo_Producto);
           
            ImeiGridBtn.Content = "Editar Imeis";

            //Verificar si tiene imeis
            if (producto.Especificaciones_producto == null)
            {
                ImeiGridBtn.IsEnabled = true;
            }
            else
            {
                //Hacer visible el grid de imeis
                if (ImeiDatagrid.Visibility == Visibility.Hidden)
                {
                    ImeiDatagrid.Visibility = Visibility.Visible;

                    //Iterar sobre las especificaciones y agregar los imeis al grid
                    producto.Especificaciones_producto.ToList().ForEach(e => {
                        if (e.IMEI != null)
                        {
                            ImeiList.Add(new ImeiClass() { Imei = e.IMEI });
                        }
                    });

                    ImeiDatagrid.ItemsSource = ImeiList;
                }
            }
        }

        public int GetIndexCategory(string Category)
        {
            //Iterar sobre el contenido las propiedades del combobox categoria, para obtener una list<string> de las categorias
            List<string> Lista = CategoriaComboBox.Items.Cast<ComboBoxItem>()
                .Select(item => item.Content.ToString()).ToList();

            return Lista.IndexOf(Category);
        }

        //Observable for dependency property
        public ObservableCollection<ImeiClass> ImeiList
        {
            get { return (ObservableCollection<ImeiClass>)GetValue(ImeiProperty); }
            set { SetValue(ImeiProperty, value); }
        }

        //Set Dependency properties for datagrid
        public static DependencyProperty ImeiProperty =
            DependencyProperty.Register("Imei",
                typeof(ObservableCollection<ImeiClass>),
                typeof(MultiUsesProductoWindow),
                new PropertyMetadata(null));
        */
        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Acción del boton insertar
        private void BtnInsertarProducto(object sender, RoutedEventArgs e)
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
                //Si los campos fueron llenados
                if (MarcaTextBox.Text != String.Empty && ModeloTextBox.Text != String.Empty && PrecioTextBox.Text != String.Empty && CategoriaComboBox.Text != String.Empty)
                {
                    _Product = new Producto()
                    {
                        Marca = MarcaTextBox.Text,
                        Modelo = ModeloTextBox.Text,
                        Precio_Venta = double.Parse(PrecioTextBox.Text),
                        Tipo_Producto = CategoriaComboBox.Text,
                    };

                    //Insertamos si el producto tiene garantia o no
                    if (ComboBoxGarantia.Text == "Si")
                    {
                        _Product.Garantia_Disponible = 1;
                    }
                    else
                    {
                        _Product.Garantia_Disponible = 0; 
                    }
                    
                    //Insertamos si tiene opcion de credito este producto
                    if (ComboBoxCredito.Text == "Si")
                    {
                        _Product.Credito_Disponible = 1;
                    }
                    else
                    {
                        _Product.Credito_Disponible = 0;
                    }

                    ViewModel.AddProduct(_Product);

                    List<Especificacion_producto> ListaEspecificaciones = new List<Especificacion_producto>();

                    foreach (var i in EspecificacionList)
                    {
                        var ElementoProducto = new Especificacion_producto();

                        ElementoProducto.Producto = _Product;
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

                    EventoPaginacion();

                    if (MessageBox.Show("Se ha ingresado correctamente el producto, ¿desea seguir ingresando productos?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                    else
                    {
                        //Limpiamos los campos para volver a insertar
                        TextBoxCantidad.Text = String.Empty;
                        MarcaTextBox.Text = String.Empty;
                        ModeloTextBox.Text = String.Empty;
                        CategoriaComboBox.Text = String.Empty;
                        PrecioTextBox.Text = String.Empty;
                        ComboBoxImei.Text = String.Empty;
                        ComboBoxGarantia.Text = String.Empty;
                        ComboBoxCredito.Text = String.Empty;

                        EspecificacionList.Clear();

                        PanelImei.Visibility = Visibility.Hidden;
                        ProductosDatagrid.Visibility = Visibility.Hidden;
                        InsertarButton.Visibility = Visibility.Hidden;
                    }


                }

                else
                {
                    MessageBoxResult result = MessageBox.Show("Por favor Rellene los campos requeridos",
                                                      "Confirmation",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Exclamation);
                }
            }
            
        }

            /*

            //Obtiene el proveedor creado desde la ventana ProveedorWindow
            void EventoProveedor(Proveedor proveedor)
            {
                _Proveedor = proveedor;

                if (_Proveedor != null)
                {
                    ProveedorTextBox.Text = _Proveedor.Empresa;
                }
            }

            //Agregar proveedor
            private void BtnInsertarProveedor(object sender, RoutedEventArgs e)
            {
                //Iniciamos la ventana de crear de proveedor
                var window = new ProveedorWindow(ViewModel);

                //Subscribimos al evento
                //window.ActualizarProveedor += EventoProveedor;
                window.Show();
            }


            //Add the Imei Box
            private void BtnInsertarImei(object sender, RoutedEventArgs e)
            {
                if (ImeiDatagrid.Visibility == Visibility.Visible)
                {
                    ImeiDatagrid.Visibility = Visibility.Hidden;
                    ImeiCheck = false;
                }
                else
                {
                    //Limpiamos los anteriores Imei
                    if (ImeiList.Count > 0)
                        ImeiList.Clear();

                    ImeiDatagrid.Visibility = Visibility.Visible;
                    ImeiCheck = true;

                    //Intentamos agregar el numero de Imeis ingresados para que sean editables y validamos 
                    try
                    {
                        var cantidad = int.Parse(TextBoxCantidad.Text);

                        for (int i = 0; i < cantidad; i++)
                        {
                            ImeiList.Add(new ImeiClass() { Numero = (i + 1), Imei = " " });
                        }

                        ImeiDatagrid.ItemsSource = ImeiList;
                    }
                    catch
                    {
                        MessageBoxResult result = MessageBox.Show("Por favor Ingrese un numero correcto y que sea entero.",
                                                  "Confirmation",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Exclamation);
                    }
                }

            }

            //Acción del boton insertar
            private void BtnInsertarProducto(object sender, RoutedEventArgs e)
            {
                //Checkbox acerca de si el producto ingresado tiene acceso a comprarse con credito
                int CreditoBox = (CreditoCheckBox.IsChecked == true) ? 1 : 0;

                try
                {
                    //Ingresando el producto 
                    var product = new Producto()
                    {
                        Marca = MarcaTextBox.Text,
                        Modelo = ModeloTextBox.Text,
                        Credito_Disponible = CreditoBox,
                        Tipo_Producto = CategoriaComboBox.Text,
                        Precio_Venta = double.Parse(PrecioTextBox.Text)
                    };

                    //Ingresando sus especificaciones
                    var Especificaciones = new List<Especificacion_producto>();

                    try
                    {
                        //Si el producto tiene Imeis se agregan, de lo contrario no
                        var cantidad = int.Parse(TextBoxCantidad.Text);
                        if (ImeiCheck == true && ImeiList.Count() != 0)
                        {
                            foreach (var item in ImeiList)
                            {
                                Especificaciones.Add(new Especificacion_producto() { Producto = product, IMEI = item.Imei });
                            }
                        }
                        else
                        {
                            for (int i = 0; i < cantidad; i++)
                            {
                                Especificaciones.Add(new Especificacion_producto() { Producto = product });
                            }
                        }
                    }
                    catch
                    {
                        MessageBoxResult result = MessageBox.Show("Por favor Ingrese un numero de menor tamaño y que sea entero.",
                                                      "Confirmation",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Exclamation);
                    }

                    Especificaciones.ForEach(item =>
                    {
                        product.Especificaciones_producto.Add(item);
                    });

                    if (_producto != null)
                    {
                        //Llamar al viewmodel para agregarlo a la base de datos
                        product.IdProducto = _producto.IdProducto;
                        ViewModel.UpdateProduct(product);
                    }
                    else
                    {
                        //ViewModel.AddProduct(product, _Proveedor);
                        //ViewModel.AddProduct(product, Especificaciones);
                    }

                    //Llamamos a actualizar la paginacion
                    EventoPaginacion();

                    if (MessageBox.Show("Se ha ingresado correctamente el producto, ¿desea seguir ingresando productos?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                    else
                    {
                        //Limpiamos los campos para volver a insertar
                        TextBoxCantidad.Text = String.Empty;
                        MarcaTextBox.Text = String.Empty;
                        ModeloTextBox.Text = String.Empty;
                        CategoriaComboBox.Text = String.Empty;
                        PrecioTextBox.Text = String.Empty;

                        if (ImeiDatagrid.Visibility == Visibility.Visible)
                        {
                            ImeiDatagrid.Visibility = Visibility.Hidden;
                            ImeiCheck = false;
                            ImeiList.Clear();
                        }

                    }
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Por favor Ingrese Los campos correctamente",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Question);
                }
            }

            */
        }

    
    public class EspecificacionClass
    {
        public int Numero { get; set; }
        public string Imei { get; set; }
        public DateTime? Garantia { get; set; }
        public string Proveedor { get; set; }
        public int ProveedorId { get; set; }
        public string Descripcion { get; set; }
        public double Precio_Costo { get; set; }
    }
}
