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
    /// Lógica de interacción para SelectProductWindow.xaml
    /// </summary>
    public partial class SelectProductWindow : Window
    {
        PageViewModel ViewModel;

        List<Especificacion_producto> ProductosNoComprados;
        List<Producto> ProductosPadres = new List<Producto>();


        //Agregar los productos a otra ventana al ser llamada
        public event EventHandler UpdateProduct;

        private void EventoPasarProducto()
        {
            UpdateProduct?.Invoke(this, EventArgs.Empty);
        }


        public SelectProductWindow(PageViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;
    
        }


        public void InsertInTreeView()
        {
            foreach (var i in ProductosPadres)
            {
                TreeViewItem newChild = new TreeViewItem();
                newChild.Header = i.Marca + " " + i.Modelo;
                //ProductTreeView.Items.Add(newChild);

                foreach (var x in ProductosNoComprados)
                {
                    if (i == x.Producto)
                    {
                        TreeViewItem child = new TreeViewItem();
                        child.Header = x.Descripcion;

                        newChild.Items.Add(child);
                    }
                }
            }
        }


        public void FillProductosParent()
        {
            //Search the parents products
            foreach (var i in ProductosNoComprados)
            {
                int numPadres = 0;

                //Compare if already exists
                foreach (var x in ProductosPadres)
                {
                    if (i.Producto.IdProducto != x.IdProducto)
                    {
                        numPadres++;
                    }
                }

                //if it is not repeated, then add to the list
                if (numPadres == ProductosPadres.Count)
                {
                    ProductosPadres.Add(i.Producto);
                }

            }

        }

        private void BtnSelectProduct(object sender, RoutedEventArgs e)
        {
            
            if (TipoSeleccionComboBox.SelectedIndex == 0)
            {
                SelectProduct();
            }

            else
            {
                
                if ( Int32.Parse (StockTextBox.Text) > ViewModel.SelectedProductParent.Especificaciones_producto.Where(t => t.Vendido.Equals("No")).Count() )
                {
                    MessageBoxResult result = MessageBox.Show("Por favor seleccione una cantidad menor al stock disponible del producto seleccionado.",
                                                     "Confirmation",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation);
                }

                else
                {
                    var num = 0;

                    for (int i=0; i<Int32.Parse (StockTextBox.Text); i++)
                    {
                        var idSelected = ViewModel.SelectedProductParent;
                     
                        foreach (var z in idSelected.Especificaciones_producto.Where(t => t.Vendido.Equals("No")))
                        {
                            if (num < Int32.Parse(StockTextBox.Text) )
                            {
                                ViewModel.ProductosHijosSeleccionados.Add(z);
                                num++;
                            }
                            
                        }
                        
                    }

                    EventoPasarProducto();
                    this.Close();
                   
                }

            }
            
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = SearchBar.Text;

            ViewModel.SearchProductoselect(filtro);
        }

        private void ParentTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.FilLProductosHijos();
        }

        private void StockTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            if (StockTextBox.Text.Length > 2)
                e.Handled = true;
            else
                e.Handled = false;
            
        }

        private void TipoSeleccionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TipoSeleccionComboBox.SelectedIndex == 1)
            {
                PanelSeleccion.Visibility = Visibility.Hidden;
                SelectStockPanel.Visibility = Visibility.Visible; 
            }
            else
            {
                PanelSeleccion.Visibility = Visibility.Visible;

                if (SelectStockPanel != null)
                {
                    SelectStockPanel.Visibility = Visibility.Hidden;
                }
            }
        }

        private void SelectProduct ()
        {
            var idSelected = ViewModel.SelectedProductHijo;

            ViewModel.ProductosHijosSeleccionados.Add(idSelected);

            if (idSelected == null)
            {
                MessageBoxResult result = MessageBox.Show("Por favor seleccione un producto de la lista, del que desea realizar una venta ",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

            else
            {
                //Pasamos el dato a la ventana que lo invoque
                EventoPasarProducto();

                this.Close();
            }
        }

        private void ChildTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectProduct();
        }
    }
}
