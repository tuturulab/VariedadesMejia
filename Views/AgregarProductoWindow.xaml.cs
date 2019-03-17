using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Lógica de interacción para VentanaAgregarProducto.xaml
    /// </summary
    public partial class AgregarProductoWindow : Window
    {
        bool ImeiCheck = true;
        public PageViewModel ViewModel;
        public Proveedor _Proveedor;
        private Producto _producto;

        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        public AgregarProductoWindow(PageViewModel viewModel, Producto producto = null)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
            _Proveedor = new Proveedor();
            ImeiList = new ObservableCollection<ImeiClass>();

            //Si el ID no es 0, entonces la ventana de agregar producto, pasara a ser de editar producto
            if (producto != null)
            {
                SetProductoDatatoView(producto);
            }
        }

        public void SetProductoDatatoView(Producto producto)
        {
            _producto = producto;

            //Seteo de datos a la vista
            Title = "Editar Producto";
            InsertarButton.Content = "Editar Producto";
            MarcaTextBox.Text = producto.Marca;
            ModeloTextBox.Text = producto.Modelo;
            PrecioTextBox.Text = producto.Precio_Venta.ToString();
            CategoriaComboBox.SelectedIndex = GetIndexCategory(producto.Tipo_Producto);
            TextBoxCantidad.Text = producto.Cantidad_Disponible.ToString();
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
                typeof(AgregarProductoWindow),
                new PropertyMetadata(null));

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

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
            window.ActualizarProveedor += EventoProveedor;
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
                    Cantidad_Disponible = int.Parse(TextBoxCantidad.Text),
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

                if (_Proveedor != null)
                {
                    //Llamar al viewmodel para agregarlo a la base de datos
                    ViewModel.AddProduct(product, Especificaciones, _Proveedor);
                }
                else
                {
                    ViewModel.AddProduct(product, Especificaciones);
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
    }

    public class ImeiClass
    {
        public int Numero { get; set; }
        public string Imei { get; set; }
    }
}
