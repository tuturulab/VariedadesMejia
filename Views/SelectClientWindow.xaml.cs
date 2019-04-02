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
using Variedades.Models;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para SelectClientWindow.xaml
    /// </summary>
    public partial class SelectClientWindow : Window
    {
        PageViewModel ViewModel;
        public event EventHandler EventSelectedClient;
        
        private void ActivarEventoClient()
        {
            EventSelectedClient?.Invoke(this, EventArgs.Empty);
        }

        public SelectClientWindow( PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            //Restablecemos el valor a nulo 
            ViewModel.SelectedClientWindow = null;
            InitializeComponent();

            ViewModel.FillClientesFullCollection();
        }

        private void BtnSelectClient(object sender, RoutedEventArgs e)
        { 
            //Necesito una manera de limpiar el cliente window selecccionado cada vez que se resetee la ventana, siempre se selecciona el 1 si no hay un item seleccionado
            var idSelected = ViewModel.SelectedClientWindow;

            if (idSelected == null )
            {
                MessageBoxResult result = MessageBox.Show("Por favor seleccione un cliente de la lista, del que desea realizar una venta ",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

            else
            {
                //Pasamos el dato a la ventana que lo invoque
                ActivarEventoClient();

                this.Close();
            }
        }
        //NOT YET 
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string busqueda = SearchBar.Text;

            ViewModel.SearchClienteFullList(busqueda);

        }
    }
}
