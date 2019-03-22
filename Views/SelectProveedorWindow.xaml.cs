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
    /// Lógica de interacción para SelectProveedorWindow.xaml
    /// </summary>
    public partial class SelectProveedorWindow : Window
    {
        PageViewModel ViewModel;

        public event EventHandler EventSelectedProveedor;

        //Evento donde pasa el proveedor seleccionado a la ventana que lo llamo
        private void ActivarEventoProveedor()
        {
            EventSelectedProveedor?.Invoke(this, EventArgs.Empty);
        }

        public SelectProveedorWindow(PageViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            ViewModel.FillProveedorFullList();
        }

        private void BtnSelectProveedor (object sender, RoutedEventArgs e)
        {
            //Llamamos al evento para cerrar la ventana
            ActivarEventoProveedor();
            this.Close();
        }

        //Barra de Busqueda
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string busqueda = SearchBar.Text;

            if (busqueda == string.Empty)
            {
                ViewModel.SearchProveedorList(string.Empty);
            }
            else
            {
                ViewModel.SearchProveedorList(busqueda);
            }
        }
    }
}
