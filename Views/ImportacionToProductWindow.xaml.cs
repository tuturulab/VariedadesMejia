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
    /// Lógica de interacción para ImportacionToProductWindow.xaml
    /// </summary>
    public partial class ImportacionToProductWindow : Window
    {
        PageViewModel ViewModel;
        MultiUsesProductoWindow window;
        DetalleProveedor Importacion;


        public ImportacionToProductWindow(PageViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            Importacion = ViewModel.SelectedImportacion;

            SeguimientoTextBox.Text = Importacion.Numero_Seguimiento;
        }

        public void EventoInsertarProductos(object sender, EventArgs e)
        {
            UtilidadProductos();
        }

        private void UtilidadProductos()
        {
            ViewModel.SearchProductosDeUnaImportacion();
        }

        private void BtnCompletar (object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnBorrarClick (object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnInsertarProducto (object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new MultiUsesProductoWindow(ViewModel);

            //Subscribimos al evento
            window.UpdateImportaciones += new EventHandler(EventoInsertarProductos);
            window.Show();
        }
    }
}
