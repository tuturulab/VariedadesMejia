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
using Variedades.Models;
using System.Windows.Shapes;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para SelectFatherProduct.xaml
    /// </summary>
    public partial class SelectFatherProduct : Window
    {
        PageViewModel ViewModel;
        //Evento Actualizar Importaciones
        public event EventHandler UpdateImportaciones;

        AddToExistentProductWindow windowProduct;

        Proveedor_producto Proveedor_Producto;

        public SelectFatherProduct(PageViewModel viewModel, Proveedor_producto proveedor_)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            ViewModel.FillProductoFatherFullList();

            Proveedor_Producto = proveedor_;
        }

        public void EventoInsertarImportacion(object sender, EventArgs e)
        {
            EventoImportacion();
        }

        //Pasa productos insertados a la ventana ImportacionToProductWindow
        private void EventoImportacion()
        {
            UpdateImportaciones?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        public void BtnSelectProduct (object sender, RoutedEventArgs e)
        {
            Models.Producto producto = ViewModel.SelectedFatherProduct;

            //Iniciamos la ventana de crear un producto
            windowProduct = new AddToExistentProductWindow(ViewModel, producto, Proveedor_Producto);

            //Subscribimos al evento
            windowProduct.UpdateSelect += new EventHandler(EventoInsertarImportacion);
            windowProduct.Show();
        }
    }
}
