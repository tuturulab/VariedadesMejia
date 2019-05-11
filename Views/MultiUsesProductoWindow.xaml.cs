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

        //Evento Actualizar Importaciones
        public event EventHandler UpdateImportaciones;

        //Cambia la ventana segun si tiene imei o no, 0 No se ha seleccionado, 1 = Si hay Imei , 2 = No hay Imei
        public int ImeiActivate = 0;

        ObservableCollection<EspecificacionClass> EspecificacionList;

        ObservableCollection<Especificacion_producto> EspecificacionesToEditProductoList;
        private Proveedor_producto ImportacionProducto;

        public MultiUsesProductoWindow(PageViewModel viewModel, Producto producto = null, Proveedor_producto proveedor_ = null)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            EspecificacionList = new ObservableCollection<EspecificacionClass>();
            ProductosDatagrid.ItemsSource = EspecificacionList;

           
            if (proveedor_ != null)
            {
                ImportacionProducto = proveedor_;
            }
                
        }

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        
        //Pasa productos insertados a la ventana ImportacionToProductWindow
        private void EventoImportacion()
        {
            UpdateImportaciones?.Invoke(this, EventArgs.Empty);
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
                PanelGarantia.Visibility = Visibility.Visible;
            }

            else
            {
                GarantiaColumn.Visibility = Visibility.Hidden;
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

        
        //Si el usuario Agrega elementos a la tabla
        private void AgregarATablaClick(object sender, RoutedEventArgs e)
        {
            if (_Proveedor != null)
            {
                if ( TextBoxCantidad.Text != String.Empty  )
                {
                    for (int i=0; i< int.Parse (TextBoxCantidad.Text); i++  )
                    {
                        EspecificacionList.Add(new EspecificacionClass()
                        {
                            Descripcion = " ",
                            Imei = " ",
                            Proveedor = _Proveedor.Empresa,
                            ProveedorId = _Proveedor.IdProveedor
                        });
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

        //Delete
        private void BtnBorrarClick(object sender, RoutedEventArgs e)
        {
            var product = ViewModel.SelectedEspecificacionProductoInProductoWindow;
            EspecificacionList.Remove(product);
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
                MessageBoxResult result = MessageBox.Show("Por favor ingrese almenos un producto con estas características",
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
                        Tipo_Producto = CategoriaComboBox.Text
                    };

                    if (string.IsNullOrWhiteSpace(TextBoxGarantiaVenta.Text) != true  && ComboBoxGarantia.Text.Equals("Si")  )
                    {
                        _Product.Garantia = int.Parse(TextBoxGarantiaVenta.Text);
                    }

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
                    if(ComboBoxCredito.Text == "Si")
                    {
                        _Product.Credito_Disponible = 1;
                    }
                    else
                    {
                        _Product.Credito_Disponible = 0;
                    }

                    //Insertamos si tiene Imei este producto
                    if (ComboBoxImei.Text == "Si")
                    {
                        _Product.Imei_Disponible = 1;
                    }
                    else
                    {
                        _Product.Imei_Disponible = 0;
                    }


                    ViewModel.AddProduct(_Product);

                    List<Especificacion_producto> ListaEspecificaciones = new List<Especificacion_producto>();

                    foreach (var i in EspecificacionList)
                    {
                        var ElementoProducto = new Especificacion_producto();

                        ElementoProducto.Producto = _Product;
                        ElementoProducto.Descripcion = i.Descripcion;

                        ElementoProducto.Garantia_Original = i.Garantia;
                        ElementoProducto.PrecioCosto = i.Precio_Costo;
                        ElementoProducto.Proveedor = ViewModel.GetProveedor(i.ProveedorId);
                        ElementoProducto.Vendido = "No";
                       
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

                    if (ImportacionProducto == null)
                    {
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
                            TextBoxGarantiaVenta.Text = String.Empty;                            

                            EspecificacionList.Clear();

                            PanelImei.Visibility = Visibility.Hidden;
                            ProductosDatagrid.Visibility = Visibility.Hidden;
                            InsertarButton.Visibility = Visibility.Hidden;
                            PanelGarantia.Visibility = Visibility.Hidden;
                        }
                    }

                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Se ha insertado y seleccionado correctamente, clickee para cerrar esta ventana",
                                                      "Confirmation",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Exclamation);

                        EventoImportacion();

                        this.Close();
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

        private void MarcaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = MarcaTextBox.Text;

            if (textBox == String.Empty)
                MarcaCheck.Visibility = Visibility.Visible;
            else
                MarcaCheck.Visibility = Visibility.Hidden;
        }



        private void ComboBoxSelectable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxGarantia.SelectedIndex != -1 && ComboBoxCredito.SelectedIndex != -1 && ComboBoxImei.SelectedIndex != -1 && PrecioTextBox.Text != String.Empty)
                selectableCheck.Visibility = Visibility.Hidden;
        }

        private void PrecioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = PrecioTextBox.Text;

            if (textBox != String.Empty && ComboBoxGarantia.SelectedIndex != -1 && ComboBoxCredito.SelectedIndex != -1 && ComboBoxImei.SelectedIndex != -1  )
                selectableCheck.Visibility = Visibility.Hidden;
            else
                selectableCheck.Visibility = Visibility.Visible;
        }

        private void ModeloTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ModeloTextBox.Text != String.Empty)
                ModeloCheck.Visibility = Visibility.Hidden;
            else
                ModeloCheck.Visibility = Visibility.Visible;
        }

        private void CategoriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriaComboBox.SelectedIndex != -1)
                TipoProductoCheck.Visibility = Visibility.Hidden;
            else
                TipoProductoCheck.Visibility = Visibility.Visible;
        }
    }

    public class EspecificacionClass
    {
        public string Imei { get; set; }
        public DateTime? Garantia { get; set; }
        public string Proveedor { get; set; }
        public int ProveedorId { get; set; }
        public string Descripcion { get; set; }
        public double Precio_Costo { get; set; }
    }
}
