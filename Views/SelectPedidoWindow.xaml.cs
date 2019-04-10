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
    /// Lógica de interacción para SelectPedidoWindow.xaml
    /// </summary>
    public partial class SelectPedidoWindow : Window
    {
        PageViewModel ViewModel;
        public event EventHandler EventSelectedPedido;

        private void ActivarEventoPedido()
        {
            EventSelectedPedido?.Invoke(this, EventArgs.Empty);
        }

        public SelectPedidoWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            ViewModel.FillPedidos();
        }

        private void BtnSelectPedido (object sender, RoutedEventArgs e)
        {
            //Necesito una manera de limpiar el cliente window selecccionado cada vez que se resetee la ventana, siempre se selecciona el 1 si no hay un item seleccionado
            var idSelected = ViewModel.SelectedPedidoWindow;

            if (idSelected == null)
            {
                MessageBoxResult result = MessageBox.Show("Por favor seleccione un pedido de la lista, del que desea realizar una venta ",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

            else
            {
                //Pasamos el dato a la ventana que lo invoque
                ActivarEventoPedido();

                this.Close();
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = SearchBar.Text;

            ViewModel.SearchPedido(filtro);
        }
    }
}
