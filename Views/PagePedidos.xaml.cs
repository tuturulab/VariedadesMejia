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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Variedades.Models;
using Variedades.Utils;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para PagePedidos.xaml
    /// </summary>
    public partial class PagePedidos : Page
    {
        public PageViewModel ViewModel;
        static Paging PagedTable = new Paging();
        MultiUsesPedidoWindow window;

        DetallePedidoWindow windowPedido;

        //Numeros a mostrar de pagina
        public int NumeroPaginaActual;
        public int NumeroPaginaMax;

        public User thisUser;

        public PagePedidos(PageViewModel pageViewModel, User _user)
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

        private void BtnDetallePedido (object sender, RoutedEventArgs e)
        {
            Pedido pedido = ViewModel.SelectedPedido;
            windowPedido = new DetallePedidoWindow(ViewModel, pedido);

            windowPedido.Show();
        }

        //Botones de edicion
        private void BtnInsertarPedido(object sender, RoutedEventArgs e)
        {
           
            //Iniciamos la ventana de crear un pedido
            window = new MultiUsesPedidoWindow(ViewModel);

            //Subscribimos al evento
            window.UpdatePagination += new EventHandler(EventoPaginacion);
            window.Show();
        }

        private void BtnEditarPedido(object sender, RoutedEventArgs e)
        {
            //
        }


        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NextPedido(10);
            UtilidadPaginacion();
        }

        private void BtnPreviousClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousPedido(10);
            UtilidadPaginacion();
        }

        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstPedido(10);
            UtilidadPaginacion();
        }

        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LastPedido(10);
            UtilidadPaginacion();
        }

        /*
         * Función que se encarga de mostrar la pagina actual que se encuentra el usuario y validar que si esta 
         * Es La ultima página, o la primera, se desactive los botones.  
        */
        private void UtilidadPaginacion()
        {
            NumeroPaginaActual = (ViewModel.PagePedidosNumber() + 1);
            NumeroPaginaMax = (ViewModel.PagePedidosNumberMax());


            //Hotfix si se elimina el ultimo registro y se queda fuera de tabla
            if (NumeroPaginaActual > NumeroPaginaMax && NumeroPaginaMax != 0)
            {
                ViewModel.PreviousPedido(10);
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


        private void BtnBorrarPedido(object sender, RoutedEventArgs e)
        {
            //Obtenemos el Id del Cliente seleccionado 

            Pedido pedido = ViewModel.SelectedPedido;

            //Pestaña de confirmación

            if (MessageBox.Show(" Estás seguro que deseas eliminar el pedido Orden Pagare : " + pedido.IdPedido.ToString() + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //
            }
            else
            {
                ViewModel.DeletePedido(pedido);
                UtilidadPaginacion();
            }


        }


        //Barra de Busqueda

        private void PedidoSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string vacio = PedidosSearchBox.Text;

            if (vacio == string.Empty)
            {
                ViewModel.SearchPedido(vacio);
            }
            else
            {
                ViewModel.SearchPedido(vacio);
            }
        }
    }
}

