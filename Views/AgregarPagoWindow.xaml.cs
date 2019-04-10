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
    /// Lógica de interacción para AgregarPagoWindow.xaml
    /// </summary>
    public partial class AgregarPagoWindow : Window
    {
        PageViewModel ViewModel;
        Venta venta;

        double saldo = 0;

        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }


        public AgregarPagoWindow(PageViewModel viewModel, Venta venta_)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = ViewModel;
            venta = venta_;

            FillData();
        }

        public void FillData()
        {
            TotalTextBox.Text = venta.MontoVenta.ToString();

            double dineroPagado = 0;

            foreach (var i in venta.Pagos)
            {
                dineroPagado = dineroPagado + i.Monto;
            }

            saldo = (venta.MontoVenta - dineroPagado);

            SaldoTextBox.Text = (saldo).ToString();
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnInsertarPago (object sender, RoutedEventArgs e)
        {
            if (AbonoTextBox.Text != String.Empty)
            {
                double Abono = Double.Parse(AbonoTextBox.Text);

                if (Abono > saldo)
                {
                    MessageBoxResult result = MessageBox.Show("Por favor ingrese un pago menor al saldo restante indicado",
                                                "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
                }

                else
                {
                    Pago pago = new Pago();
                    pago.Venta = venta;
                    pago.Monto = Abono;
                    pago.Fecha_Pago = DateTime.Now;

                    ViewModel.AddPago(pago);

                    ViewModel.VerificarVentaEstado(venta);

                    MessageBoxResult result = MessageBox.Show("Se ha ingresado correctamente el pago",
                                               "Confirmation",
                                               MessageBoxButton.OK,
                                               MessageBoxImage.Exclamation);

                    EventoPaginacion();

                    this.Close();
                }

            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Ingrese el monto a pagar por favor",
                                                "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
            }
        }
    }
}
