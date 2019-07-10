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
    /// Lógica de interacción para DetalleVentaWindow.xaml
    /// </summary>
    public partial class DetalleVentaWindow : Window
    {
        PageViewModel ViewModel;
        Venta VentaRealizada;

        public DetalleVentaWindow(PageViewModel viewModel, Venta venta_)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;

            VentaRealizada = venta_;

            ViewModel.FillProductosDeUnaVenta(VentaRealizada.IdVenta);

            FillData();
        }

        public void FillData()
        {
            PagareTextBox.Text = VentaRealizada.IdVenta.ToString();
            ClienteTextBox.Text = VentaRealizada.NombreCliente;
            MontoTextBox.Text = VentaRealizada.MontoVenta.ToString();

            double dineroPagado = 0;

            foreach (var i in VentaRealizada.Pagos)
            {
                dineroPagado = dineroPagado + i.Monto;
            }

            SaldoTextBox.Text = (VentaRealizada.MontoVenta - dineroPagado).ToString();


        }
    }
}
