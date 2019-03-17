﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
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
        AgregarProductoWindow window;
        //test user github

        //Numeros a mostrar de pagina
        public int NumeroPaginaActual;
        public int NumeroPaginaMax;

        public PageProducts(PageViewModel pageViewModel)
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
        private void BtnInsertarProducto(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new AgregarProductoWindow(ViewModel);

            //Subscribimos al evento
            window.UpdatePagination += new EventHandler(EventoPaginacion);
            window.Show();
        }

        private void BtnEditarProducto(object sender, RoutedEventArgs e)
        {
            //Obtenemos el Id del Producto seleccionado 
            /*object item = product_table.SelectedItem;
            string IdProducto = (product_table.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            string Nombre = (product_table.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text + " " +
                (product_table.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;*/

            var producto = ViewModel.SelectedProduct;

            //Iniciamos la ventana de crear un producto
            window = new AgregarProductoWindow(ViewModel, producto);

            window.UpdatePagination += new EventHandler(EventoPaginacion);

            window.Show();
        }


        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NextProduct(3);
            UtilidadPaginacion();
        }

        private void BtnPreviousClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousProduct(3);
            UtilidadPaginacion();
        }

        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstProduct(3);
            UtilidadPaginacion();
        }

        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LastProduct(3);
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
                ViewModel.PreviousProduct(3);
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
            //Obtenemos el Id del Producto seleccionado 
            object item = product_table.SelectedItem;
            string IdProducto = (product_table.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            string Nombre = (product_table.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text + " " +
                (product_table.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

            //Pestaña de confirmación

            if (MessageBox.Show(" Estás seguro que deseas eliminar el producto: " + Nombre + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //
            }
            else
            {
                ViewModel.DeleteProduct(int.Parse(IdProducto));
                UtilidadPaginacion();
            }

        }
    }
}
