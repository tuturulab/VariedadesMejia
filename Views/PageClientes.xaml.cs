using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Variedades.Utils;

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageClientes.xaml
    /// </summary>
    public partial class PageClientes : Page
    {
        public PageViewModel ViewModel;
        static Paging PagedTable = new Paging();
        MultiUsesClienteWindow window;

        //Numeros a mostrar de pagina
        public int NumeroPaginaActual;
        public int NumeroPaginaMax;

        public PageClientes(PageViewModel pageViewModel)
        {
            InitializeComponent();

            //Obtener el viewmodel de la ventana principal y lo incializamos
            ViewModel = pageViewModel;
            DataContext = ViewModel;

            UtilidadPaginacion();
        }

        public void EventoPaginacion(object sender, EventArgs e)
        {
            UtilidadPaginacion();
        }

        //Botones de edicion
        private void BtnInsertarCliente(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new MultiUsesClienteWindow (ViewModel);

            //Subscribimos al evento
            window.UpdatePagination += new EventHandler(EventoPaginacion);
            window.Show();
        }

        private void BtnEditarCliente(object sender, RoutedEventArgs e)
        {
            //Obtenemos el Id del Producto seleccionado 
            /*object item = product_table.SelectedItem;
            string IdProducto = (product_table.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            string Nombre = (product_table.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text + " " +
                (product_table.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;*/

            //var producto = ViewModel.SelectedProduct;

            //Iniciamos la ventana de crear un producto
            //window = new MultiUsesProductoWindow(producto) { DataContext = this.DataContext };



            //window.UpdatePagination += new EventHandler(EventoPaginacion);

            //window.Show();


        }


        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NextClient(3);
            UtilidadPaginacion();
        }

        private void BtnPreviousClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousClient(3);
            UtilidadPaginacion();
        }

        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstClient(3);
            UtilidadPaginacion();
        }

        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LastClient(3);
            UtilidadPaginacion();
        }

        /*
         * Función que se encarga de mostrar la pagina actual que se encuentra el usuario y validar que si esta 
         * Es La ultima página, o la primera, se desactive los botones.  
        */
        private void UtilidadPaginacion()
        {
            NumeroPaginaActual = (ViewModel.PageClientesNumber() + 1);
            NumeroPaginaMax = (ViewModel.PageClientesNumberMax());


            //Hotfix si se elimina el ultimo registro y se queda fuera de tabla
            if (NumeroPaginaActual > NumeroPaginaMax && NumeroPaginaMax != 0)
            {
                ViewModel.PreviousClient(3);
                NumeroPaginaActual--;
            }

            //En caso de que no hayan registros
            if (NumeroPaginaMax == 0)
            {
                PageInfo.Content = "No Existen registros disponibles";
            }

            else
            {
                PageInfo.Content = "Mostrando página " + NumeroPaginaActual + " de " + NumeroPaginaMax;
            }

            //Validacion para desactivar botones de la paginacion
            if (NumeroPaginaActual == 1)
            {
                BtnPrevious.IsEnabled = false;
                BtnFirst.IsEnabled = false;
            }
            else
            {
                BtnPrevious.IsEnabled = true;
                BtnFirst.IsEnabled = true;
            }

            if (NumeroPaginaActual == NumeroPaginaMax || (NumeroPaginaActual == 1 && NumeroPaginaMax == 0))
            {
                BtnNext.IsEnabled = false;
                BtnLast.IsEnabled = false;
            }

            else
            {
                BtnNext.IsEnabled = true;
                BtnLast.IsEnabled = true;
            }

        }


        private void BtnBorrarCliente(object sender, RoutedEventArgs e)
        {
            //Obtenemos el Id del Cliente seleccionado 


            object item = client_table.SelectedItem;
            string IdCliente = (client_table.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            string Nombre = (client_table.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text + " " +
                (client_table.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

            //Pestaña de confirmación

            if (MessageBox.Show(" Estás seguro que deseas eliminar al cliente: " + Nombre + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //
            }
            else
            {
                ViewModel.DeleteClient(int.Parse(IdCliente));
                UtilidadPaginacion();
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string FiltradoCliente = SearchClientBox.Text;

            ViewModel.SearchClient(FiltradoCliente);
        }
    }
}
