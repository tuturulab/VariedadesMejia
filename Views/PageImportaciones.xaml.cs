using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Variedades.Utils;
using Variedades.Models;


namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageProducts.xaml
    /// </summary>
    public partial class PageImportaciones : Page
    {
        public PageViewModel ViewModel;
        MultiUsesImportacionWindow window;

        //Iniciamos la ventana de crear una importación
        DetalleImportacionWindow detalleWindow;


        //static Paging PagedTable = new Paging();
        //AgregarImportacionWindow window;

        //Numeros a mostrar de pagina
        public int NumeroPaginaActual;
        public int NumeroPaginaMax;

        public User thisUser;

        public PageImportaciones(PageViewModel pageViewModel, User user_)
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

        //Botones de edicion
        private void BtnInsertarImportacion(object sender, RoutedEventArgs e)
        {
            if (thisUser.Role.Equals("Gerente") || thisUser.Role.Equals("Administrador"))
            {
                //Iniciamos la ventana de crear una importación
                window = new MultiUsesImportacionWindow(ViewModel);

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

        //Abre ventana detalles
        private void BtnDetalle (object sender, RoutedEventArgs e)
        {
            DetalleProveedor detalleProveedor = ViewModel.SelectedImportacion;
            detalleWindow = new DetalleImportacionWindow(ViewModel, detalleProveedor);

            detalleWindow.Show();

        }

        //Boton para completar la importacion al llegar
        private void BtnCompletarImportacion (object sender, RoutedEventArgs e)
        {
            DetalleProveedor Importacion = ViewModel.SelectedImportacion;

            if (MessageBox.Show("Seleccione si, en caso de que le haya llegado esta importacion, Numero Seguimiento: " + Importacion.Numero_Seguimiento , "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                //No
            }
            else
            {
                ViewModel.ChangeEstadoImportacion(Importacion);
                UtilidadPaginacion();
            }

        }



        private void BtnEditarImportacion(object sender, RoutedEventArgs e)
        {
            
        }


        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NextImportacion(3);
            UtilidadPaginacion();
        }

        private void BtnPreviousClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousImportacion(3);
            UtilidadPaginacion();
        }

        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstImportacion(3);
            UtilidadPaginacion();
        }

        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LastImportacion(3);
            UtilidadPaginacion();
        }

        /*
         * Función que se encarga de mostrar la pagina actual que se encuentra el usuario y validar que si esta 
         * Es La ultima página, o la primera, se desactive los botones.  
        */
        private void UtilidadPaginacion()
        {
            NumeroPaginaActual = (ViewModel.PageImportacionNumber() + 1);
            NumeroPaginaMax = (ViewModel.PageImportacionNumberMax());


            //Hotfix si se elimina el ultimo registro y se queda fuera de tabla
            if (NumeroPaginaActual > NumeroPaginaMax && NumeroPaginaMax != 0)
            {
                ViewModel.PreviousImportacion(3);
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
            Models.DetalleProveedor _import = ViewModel.SelectedImportacion;

            if (MessageBox.Show(" Estás seguro que deseas eliminar el pedido de importacion con N# Seguimiento: " + _import.Numero_Seguimiento + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //
            }
            else
            {
                ViewModel.DeleteImportacion(_import);
                UtilidadPaginacion();
            }

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = SearchBox.Text;

            ViewModel.SearchImportacionList(filtro);

        }
    }
}
