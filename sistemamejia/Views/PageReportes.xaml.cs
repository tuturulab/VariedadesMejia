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

            CefWindow cef = new CefWindow(filePathOut);
            cef.Show();
        }

        private void Reporte2Event(object sender, RoutedEventArgs e)
        {
            PickRangedDate pickRangedDate = new PickRangedDate(viewModel);
            pickRangedDate.ReturnDates += IHaveRangesDates;
            pickRangedDate.Show();
        }


        /* Reporte Ventas ganancias y perdidas */


        private void GananciasPepega(object sender, EventArgs e)
        {

            ///Html parse lib configs
            var configuration = Configuration.Default;
            var context = BrowsingContext.New(configuration);
            var parser = context.GetService<IHtmlParser>();

            //File path, in - out
            string date = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-tt");
            string filePathIn = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/VentasEstadisticas" + ".html");
            string filePathOut = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/VentasEstadisticas" + date + ".html");

            //Read file as string
            string source = System.IO.File.ReadAllText(filePathIn);

            //Parse string
            var document = parser.ParseDocument(source);

            /* Get List */

            var montoTotalEsperado = viewModel.getMontoTotalEsperado() ;

            var montoPagos = viewModel.getTotalPagos();


            var hiddenInput = document.CreateElement("input");


            hiddenInput.SetAttribute("type", "hidden");
            hiddenInput.SetAttribute("id", "monto1");
            hiddenInput.SetAttribute("value", montoTotalEsperado.ToString() );
            
            //document.AppendChild(hiddenInput);

            document.Body.AppendChild(hiddenInput);

            // hola 

            var hiddenInput2 = document.CreateElement("input");


            hiddenInput2.SetAttribute("type", "hidden");
            hiddenInput2.SetAttribute("id", "monto2");
            hiddenInput2.SetAttribute("value", montoPagos.ToString() );

            //document.AppendChild(hiddenInput);

            document.GetElementById("contenidobase").InnerHtml = " </br> </br>  <h1 style='color: blue' > Beneficios esperados y Montos abonados </h1> </br> <h4> Monto Esperado: "+montoPagos.ToString() + "</h4> <h4> Montos abonados: "+ montoPagos.ToString() + "  </h4> </br></br></br></br></br></br></br></br></br></br></br></br></br></br></br></br></br>v   </br></br></br></br></br></br> ";

            document.Body.AppendChild(hiddenInput2);


            //viewModel.Vent



            //IEnumerable<Venta> ventas = viewModel.GetRangedVentas();

            //var tdFecha = document.CreateElement("script");
            //tdFecha.Append("alert('hola')");

            /*

            var sourceCode = @"<!DOCTYPE html>
                <html lang='en'>
                < head >
                    < meta charset = 'UTF-8' >
 
                     < meta name = 'viewport' content = 'width=device-width, initial-scale=1.0' >
    
                        < meta http - equiv = 'X-UA-Compatible' content = 'ie=edge' >
         
                             < title > Ventas Diarias </ title >
            

                                < !--< link href = 'Spectre/spectre.min.css' rel = 'stylesheet' /> -->
                
                                    < !--< link href = 'Spectre/spectre-exp.min.css' rel = 'stylesheet' /> -->
                    
                                        < !--< link href = 'Spectre/spectre-icons.min.css' rel = 'stylesheet' /> -->
                        
                                            < link href = 'Styles/ventadiariastyles.css' rel = 'stylesheet' />
                           
                                               < link href = 'ChartJs/chart.min.css' rel = 'stylesheet' />
                              


                                              </ head >
                                              < body >
                              

                                                  < div id = 'top' class='row'>
                        <div class='column left'>
                            <img src = './Img/logo.png' style='width: 70%'>
                        </div>
                        <div class='column middle'>
                            <center id = 'logo' >
                                < h1 style='color: white'> Variedades Mejía</h1>
                                <h3 style = 'color: white' > Reporte de ingresos de ventas </h3>
                            </center>
                        </div>
                        <div style = 'color: white; font-family:Arial' class='column right'>
                            <center id = 'logo'>
                                < h2 > Fecha:</h2>
                                <h3 id = 'FechaSetter' ></ h3 >
                            </ center >
                        </ div >
                    </ div >

                    < div class='container2'>
                        <div class='table-wrapper'>
           
                            <div class=''>
                                <div id = 'color' >

                                </ div >
                            </ div >


                        </ div >
                    </ div >


                    < div class='row footer' style='font-family:Arial'>
                        <div style = 'background: white ' class='column-footer left'>
                            <center id = 'logo' >
                                < h2 style='color: black;'>Firma Propietario</h2>

                                <hr style = 'background: black; margin-top: 60px; width: 70%' />
 
                             </ center >
 
                         </ div >
 
                         < div class='column-footer middle'>
                            <center style = 'margin-top: 80px' >
                                < h4 style='color: black;'> No valido sin firma</h4>
                            </center>
                        </div>
                        <div class='column-footer right'>
                            <center id = 'logo' >
                                < h3 style='color: black'>
                                    Monto Total
                                </h3>

                                <h4 id = 'TotalAmount' style= 'color: black;' ></ h4 >
                            </ center >
                        </ div >
                    </ div >

                    < script type= 'text/javascript' src= 'ChartJs/Chart.min.js' ></ script >
                    < script >

                        var ctx = Document.getElementById('color');

                        var myPieChart = new Chart(ctx, {
                            type: 'pie',
                            data: data,
                            options: options
                        });
                    </script>    


                </body>
                </html>";

        

            var document = await context.OpenAsync(req => req.Content(source));
            */


            var my_awesome_script = document.CreateElement("script");

            //my_awesome_script.setAttribute('src', 'http://example.com/site.js');

            my_awesome_script.SetAttribute("src", "test.js" );

            //document.head.appendChild(my_awesome_script);


            document.GetElementById("FechaSetter").TextContent = DateTime.Now.ToString("dd-MM-yyyy");

            document.Body.AppendChild(my_awesome_script);

            //trElement.AppendChild(tdFecha);
            //document.Append(tdFecha);

            System.IO.File.WriteAllText(filePathOut, document.DocumentElement.OuterHtml);

            CefWindow cef = new CefWindow(filePathOut);
            cef.Show();
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

            CefWindow cef = new CefWindow(filePathOut);
            cef.Show();
        }
    }


}
