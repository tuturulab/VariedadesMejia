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
using Variedades.Views;

namespace Variedades.Models
{
    /// <summary>
    /// Lógica de interacción para DetalleImportacionWindow.xaml
    /// </summary>
    public partial class DetalleImportacionWindow : Window
    {
        PageViewModel ViewModel;
        DetalleProveedor Importacion;

        DetallePedidoWindow window;

        Pedido MainPedido;

        public DetalleImportacionWindow(PageViewModel viewModel, DetalleProveedor _Importacion)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            Importacion = _Importacion;

            FillCampos();
        }

        public void FillCampos()
        {
            LlegadaTextBox.Text = Importacion.Llegada;
            PrecioCostoTextBox.Text = Importacion.Precio_Costo.ToString();
            EstadoTextBox.Text = Importacion.EstadoEncargo;
            SeguimientoTextBox.Text = Importacion.Numero_Seguimiento;


            ViewModel.FillProductosDeUnaImportacion(Importacion);

            Pedido pedido = Importacion.Pedido;

            if (pedido == null)
            {
                PedidoPanel.Visibility = Visibility.Hidden;
            }

            else
            {
                PedidoPanel.Visibility = Visibility.Visible;
                MainPedido = pedido;
            }
        }

        private void BtnDetallePedido (object sender, RoutedEventArgs e)
        {
            
            window = new DetallePedidoWindow(ViewModel, MainPedido);

            window.Show();

        }
    }
}
