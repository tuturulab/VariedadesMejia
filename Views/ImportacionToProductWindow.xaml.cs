using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para ImportacionToProductWindow.xaml
    /// </summary>
    public partial class ImportacionToProductWindow : Window
    {
        PageViewModel ViewModel;
        MultiUsesProductoWindow window;
        public Proveedor_producto Importacion;


        int IdProveedorProducto;

        public ImportacionToProductWindow(PageViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            var DetalleImportacion = ViewModel.SelectedImportacion;

            foreach (var i in DetalleImportacion.Proveedor_Productos)
            {
                IdProveedorProducto = i.Idproveedor_producto;
                Importacion = i;
            }

            SeguimientoTextBox.Text = DetalleImportacion.Numero_Seguimiento;

            ViewModel.SetProductosImportados(DetalleImportacion);
        }

        public void EventoInsertarProductos(object sender, EventArgs e)
        {
            UtilidadProductos();
        }

        private void UtilidadProductos()
        {
            ViewModel.SearchProductosDeUnaImportacion(IdProveedorProducto);
        }

        private void BtnCompletar (object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnBorrarClick (object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Esta seguro que desea eliminar este producto?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {

            }
            else
            {
                ViewModel.DeleteEspecificacionProducto();
            }
            
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnSelectProducto (object sender, RoutedEventArgs e)
        {
            //
        }

        private void BtnInsertarProducto (object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new MultiUsesProductoWindow(ViewModel, null , Importacion );

            //Subscribimos al evento
            window.UpdateImportaciones += new EventHandler(EventoInsertarProductos);
            window.Show();
        }
    }
}
