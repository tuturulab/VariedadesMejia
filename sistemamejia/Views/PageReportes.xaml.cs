using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using AngleSharp;
using AngleSharp.Html.Parser;
using Variedades.Models;

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageReportes.xaml
    /// </summary>
    public partial class PageReportes : Page
    {
        public PageViewModel viewModel;
        public PageReportes(PageViewModel pViewModel)
        {
            InitializeComponent();
            viewModel = pViewModel;
        }

        private void Reporte1Event(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            ///Html parse lib configs
            var configuration = Configuration.Default;
            var context = BrowsingContext.New(configuration);
            var parser = context.GetService<IHtmlParser>();

            //File path, in - out
            string date = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-tt");
            string filePathIn = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/" + button.Name + ".html");
            string filePathOut = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/" + button.Name + date + ".html");

            //Read file as string
            string source = System.IO.File.ReadAllText(filePathIn);

            //Parse string
            var document = parser.ParseDocument(source);

            IEnumerable<Venta> ventas = viewModel.GetAllTodayVentas();

            double montoTotal = 0;

            foreach(Venta venta in ventas)
            {
                montoTotal += venta.MontoVenta;
                //tr element
                var trElement = document.CreateElement("tr");
                //trElement.SetAttribute("class", "active");

                //td elements
                var tdPagare = document.CreateElement("td");
                tdPagare.TextContent = venta.Orden_Pagare;
                trElement.AppendChild(tdPagare);

                var tdFecha = document.CreateElement("td");
                tdFecha.TextContent = venta.Fecha_Venta.ToString();
                trElement.AppendChild(tdFecha);

                var tdClienteId = document.CreateElement("td");
                tdClienteId.TextContent = venta.Cliente.Cedula;
                trElement.AppendChild(tdClienteId);

                var tdTipoPago = document.CreateElement("td");
                tdTipoPago.TextContent = venta.Tipo_Venta;
                trElement.AppendChild(tdTipoPago);

                var tdCompletado = document.CreateElement("td");
                tdCompletado.TextContent = venta.VentaCompletada;
                trElement.AppendChild(tdCompletado);

                var tdCantidad = document.CreateElement("td");
                tdCantidad.TextContent = venta.CantidadProductos.ToString();
                trElement.AppendChild(tdCantidad);

                var tdMonto = document.CreateElement("td");
                tdMonto.TextContent = "$" + (venta.MontoVenta.ToString());
                trElement.AppendChild(tdMonto);

                //add to TBody
                document.GetElementsByClassName("bodyTSource").FirstOrDefault().AppendChild(trElement);
            }

            document.GetElementById("TotalAmount").TextContent = "$" + montoTotal.ToString();
            document.GetElementById("FechaSetter").TextContent = DateTime.Now.ToString("dd-MM-yyyy");

            System.IO.File.WriteAllText(filePathOut, document.DocumentElement.OuterHtml);
            //System.IO.File.Create

            //Process.Start(filePathOut);

            CefWindow cef = new CefWindow(button.Name, filePathOut);
            cef.Show();
        }

        private void Reporte2Event(object sender, RoutedEventArgs e)
        {
            PickRangedDate pickRangedDate = new PickRangedDate(viewModel);
            pickRangedDate.ReturnDates += IHaveRangesDates;
            pickRangedDate.Show();
        }

        private void IHaveRangesDates(object sender, EventArgs e)
        {

            ///Html parse lib configs
            var configuration = Configuration.Default;
            var context = BrowsingContext.New(configuration);
            var parser = context.GetService<IHtmlParser>();

            //File path, in - out
            string date = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-tt");
            string filePathIn = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/VentaRanged" + ".html");
            string filePathOut = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/VentaRanged" + date + ".html");

            //Read file as string
            string source = System.IO.File.ReadAllText(filePathIn);

            //Parse string
            var document = parser.ParseDocument(source);

            IEnumerable<Venta> ventas = viewModel.GetRangedVentas();

            double montoTotal = 0;

            foreach(Venta venta in ventas)
            {
                montoTotal += venta.MontoVenta;
                //tr element
                var trElement = document.CreateElement("tr");
                //trElement.SetAttribute("class", "active");

                //td elements
                var tdPagare = document.CreateElement("td");
                tdPagare.TextContent = venta.Orden_Pagare;
                trElement.AppendChild(tdPagare);

                var tdFecha = document.CreateElement("td");
                tdFecha.TextContent = venta.Fecha_Venta.ToString();
                trElement.AppendChild(tdFecha);

                var tdClienteId = document.CreateElement("td");
                tdClienteId.TextContent = venta.Cliente.Cedula;
                trElement.AppendChild(tdClienteId);

                var tdTipoPago = document.CreateElement("td");
                tdTipoPago.TextContent = venta.Tipo_Venta;
                trElement.AppendChild(tdTipoPago);

                var tdCompletado = document.CreateElement("td");
                tdCompletado.TextContent = venta.VentaCompletada;
                trElement.AppendChild(tdCompletado);

                var tdCantidad = document.CreateElement("td");
                tdCantidad.TextContent = venta.CantidadProductos.ToString();
                trElement.AppendChild(tdCantidad);

                var tdMonto = document.CreateElement("td");
                tdMonto.TextContent = "$" + (venta.MontoVenta.ToString());
                trElement.AppendChild(tdMonto);

                //add to TBody
                document.GetElementsByClassName("bodyTSource").FirstOrDefault().AppendChild(trElement);
            }

            document.GetElementById("TotalAmount").TextContent = "$" + montoTotal.ToString();
            document.GetElementById("FechaSetter").TextContent = DateTime.Now.ToString("dd-MM-yyyy");
            document.GetElementById("TFechas").TextContent = "De " + viewModel.PickedDate1.ToString("dd-MM-yyyy") + " a " + viewModel.PickedDate2.ToString("dd-MM-yyyy");

            System.IO.File.WriteAllText(filePathOut, document.DocumentElement.OuterHtml);
            //System.IO.File.Create

            //Process.Start(filePathOut);

            CefWindow cef = new CefWindow("VentaRanged", filePathOut);
            cef.Show();
        }
    }


}
