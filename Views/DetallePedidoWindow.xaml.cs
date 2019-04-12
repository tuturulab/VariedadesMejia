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
    /// Lógica de interacción para DetallePedidoWindow.xaml
    /// </summary>
    public partial class DetallePedidoWindow : Window
    {
        PageViewModel ViewModel;
        Pedido Pedido;


        public DetallePedidoWindow(PageViewModel viewModel, Pedido _pedido)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = viewModel;

            Pedido = _pedido;

            FillCampos();
        }

        public void FillCampos()
        {

            ClienteTextBox.Text = Pedido.NombreCliente;
            FechaPedidoTextBox.Text = Pedido.Fecha_Pedido.ToString();
            EstadoTextBox.Text = Pedido.Estado_Pedido;

            ViewModel.FillProductosDeUnPedido(Pedido);
        }

    }
}
