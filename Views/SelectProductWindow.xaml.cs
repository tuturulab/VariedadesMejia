using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para SelectProductWindow.xaml
    /// </summary>
    public partial class SelectProductWindow : Window
    {
        PageViewModel ViewModel;

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

            ViewModel.FillEspecificacionesProducts();
        }

        private void BtnSelectProduct(object sender, RoutedEventArgs e)
        {
            var idSelected = ViewModel.SelectedProductWindow;

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
    }
}
