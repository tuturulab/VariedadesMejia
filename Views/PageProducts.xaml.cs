using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Variedades.Models;
using Variedades.Utils;

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageProducts.xaml
    /// </summary>
    public partial class PageProducts : Page
    {
        public PageViewModel ViewModel;
        static Paging PagedTable = new Paging();
        MultiUsesProductoWindow window;

        AddToExistentProductWindow window3;

        //test user github

        //Numeros a mostrar de pagina
        public int NumeroPaginaActual;
        public int NumeroPaginaMax;

        public User thisUser;

        public PageProducts(PageViewModel pageViewModel, User user_)
        {
            InitializeComponent();

            //Obtener el viewmodel de la ventana principal y lo incializamos
            ViewModel = pageViewModel;
            DataContext = ViewModel;

            UtilidadPaginacion();

            thisUser = user_;
        }

        public void EventoPaginacion(object sender, EventArgs e)
        {
            UtilidadPaginacion();
        }

        //Pdf export method
        /*private void ExportToPdf(object sender, RoutedEventArgs e)
        {

            Document document = new Document(PageSize.A4);
            PdfWriter.GetInstance(document, new FileStream(@"c:\outputs\sample.pdf", FileMode.Create));
            document.Open();

            IEnumerable<Producto> itemsOnGrid = product_table.ItemsSource as IEnumerable<Producto>;

            //Tabla de header
            PdfPTable tableHeader = new PdfPTable(4);

            tableHeader.AddCell(new PdfPCell(new Phrase("IMEI", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLUE))));
            tableHeader.AddCell(new PdfPCell(new Phrase("Marca", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLUE))));
            tableHeader.AddCell(new PdfPCell(new Phrase("Modelo", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLUE))));
            tableHeader.AddCell(new PdfPCell(new Phrase("Precio", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.BLUE))));

            document.Add(tableHeader);


            //Count numero de items en ienumerable
            int countProducts = 0;
            using (IEnumerator<Producto> enumerator = itemsOnGrid.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    countProducts++;
            }

            //body tabla -- datos de todos los productos
            PdfPTable tableBody = new PdfPTable(4);

            //add items to body
            foreach (Producto p in itemsOnGrid)
            {
                //Debug.WriteLine(p.Marca);
                tableBody.AddCell(new PdfPCell(new Phrase(p.Imei_Disponible.ToString())));
                tableBody.AddCell(new PdfPCell(new Phrase(p.Marca)));
                tableBody.AddCell(new PdfPCell(new Phrase(p.Modelo)));
                tableBody.AddCell(new PdfPCell(new Phrase(p.Precio_Venta.ToString())));
            }

            document.Add(tableBody);
            document.Close();

            Process.Start(@"c:\outputs\sample.pdf");
        }*/

        //Botones de edicion
        private void BtnInsertarProducto(object sender, RoutedEventArgs e)
        {
            if (thisUser.Role.Equals("Gerente") || thisUser.Role.Equals("Administrador"))
            {
                //Iniciamos la ventana de crear un producto
                window = new MultiUsesProductoWindow(ViewModel);

                //Subscribimos al evento
                window.UpdatePagination += new EventHandler(EventoPaginacion);
                window.Show();
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Usted no tiene derechos para acceder a esta opción",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);

            }

                
        }

        //Botones de edicion
        private void BtnAgregarExistencia(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window3 = new AddToExistentProductWindow(ViewModel, ViewModel.SelectedProduct);

            //Subscribimos al evento
            window3.UpdatePagination += new EventHandler(EventoPaginacion);
            window3.Show();
        }

        private void BtnEditarProducto(object sender, RoutedEventArgs e)
        {
            //Obtenemos el Id del Producto seleccionado 
            /*object item = product_table.SelectedItem;
            string IdProducto = (product_table.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            string Nombre = (product_table.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text + " " +
                (product_table.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;*/

            var producto = ViewModel.SelectedProduct;

            //Iniciamos la ventana de crear o editar un producto
            var window = new EditProductWindow(ViewModel, producto);

            window.UpdatePagination += new EventHandler(EventoPaginacion);

            window.Show();
        }


        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NextProduct(10);
            UtilidadPaginacion();
        }

        private void BtnPreviousClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousProduct(10);
            UtilidadPaginacion();
        }

        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstProduct(10);
            UtilidadPaginacion();
        }

        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LastProduct(10);
            UtilidadPaginacion();
        }

        /*
         * Función que se encarga de mostrar la pagina actual que se encuentra el usuario y validar que si esta 
         * Es La ultima página, o la primera, se desactive los botones.  
        */
        private void UtilidadPaginacion()
        {

            NumeroPaginaActual = (ViewModel.PageProductsNumber() + 1);
            NumeroPaginaMax = (ViewModel.PageProductsNumberMax());


            //Hotfix si se elimina el ultimo registro y se queda fuera de tabla
            if (NumeroPaginaActual > NumeroPaginaMax && NumeroPaginaMax != 0)
            {
                ViewModel.PreviousProduct(10);
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

        

        private void BtnBorrarClick(object sender, RoutedEventArgs e)
        {
            //Obtenemos el Producto seleccionado 
            
            var ProductoSeleccionado = ViewModel.SelectedProduct; 

            //Pestaña de confirmación

            if (MessageBox.Show(" Estás seguro que deseas eliminar el producto: " + ProductoSeleccionado.Marca + " " + ProductoSeleccionado.Modelo + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //
            }
            else
            {
                ViewModel.DeleteProduct( ProductoSeleccionado);
                UtilidadPaginacion();
            }

        }


        

        //Boton de Busqueda
       


        private void ProductSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string vacio = ProductSearchBox.Text;

            if (vacio == string.Empty)
            {
                ViewModel.SearchProduct(vacio);
            }
            else
            {
                

                ViewModel.SearchProduct(vacio);
            }

            UtilidadPaginacion();
            
        }

        private void BtnDetalle_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
