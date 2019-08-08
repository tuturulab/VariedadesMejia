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

            var trElement = document.CreateElement("tr");

            IEnumerable<Venta> ventas = viewModel.GetAllTodayVentas();

            foreach(Venta venta in ventas)
            {
                //tr element
                trElement = document.CreateElement("tr");
                //trElement.SetAttribute("class", "active");

                //td elements
                var tdPagare = document.CreateElement("td");
                tdPagare.TextContent = venta.Orden_Pagare;
                trElement.AppendChild(tdPagare);

                var tdTipoPago = document.CreateElement("td");
                tdTipoPago.TextContent = venta.Tipo_Venta;
                trElement.AppendChild(tdTipoPago);

                var tdClienteId= document.CreateElement("td");
                tdClienteId.TextContent = venta.Cliente.Cedula;
                trElement.AppendChild(tdClienteId);

                var tdCantidad = document.CreateElement("td");
                tdCantidad.TextContent = venta.CantidadProductos.ToString();
                trElement.AppendChild(tdCantidad);

                var tdCompletado = document.CreateElement("td");
                tdCompletado.TextContent = venta.VentaCompletada;
                trElement.AppendChild(tdCompletado);

                var tdMonto = document.CreateElement("td");
                tdMonto.TextContent = "$" + (venta.MontoVenta.ToString());
                trElement.AppendChild(tdMonto);

            }

            document.GetElementsByClassName("bodyTSource").FirstOrDefault().AppendChild(trElement);

            Debug.WriteLine(trElement.ToHtml().ToString());

            System.IO.File.WriteAllText(filePathOut, document.DocumentElement.OuterHtml);
            //System.IO.File.Create

            CefWindow cef = new CefWindow(button.Name, filePathOut);
            cef.Show();
        }
    }


}
