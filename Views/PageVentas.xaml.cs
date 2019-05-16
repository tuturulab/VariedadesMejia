using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Variedades.Utils;
using Variedades.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Diagnostics;

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageProducts.xaml
    /// </summary>
    public partial class PageVentas : System.Windows.Controls.Page
    {
        public PageViewModel ViewModel;
        static Paging PagedTable = new Paging();

        MultiUsesVentaWindow window;
        DetalleVentaWindow ventaWindow;
        AgregarPagoWindow windowPagos;

        //Numeros a mostrar de pagina
        public int NumeroPaginaActual;
        public int NumeroPaginaMax;

        public PageVentas(PageViewModel pageViewModel)
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
        private void BtnInsertarVenta(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new MultiUsesVentaWindow(ViewModel);

            //Subscribimos al evento
            window.UpdatePagination += new EventHandler(EventoPaginacion);
            window.Show();
        }

        private void BtnExportarExcel (object sender, RoutedEventArgs e)
        {
            string FileName;

            //string filetoOpen;
            try
            {
                String Fecha = DateTime.Now.ToString("dd-MM-yyyy");
                String FilePath = "Ventas " + Fecha + ".xlsx ";
                FileName = FilePath;

                //filetoOpen = FilePath;

                using (SpreadsheetDocument spreedDoc = SpreadsheetDocument.Create(FilePath,
                DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart wbPart = spreedDoc.WorkbookPart;
                    if (wbPart == null)
                    {
                        wbPart = spreedDoc.AddWorkbookPart();
                        wbPart.Workbook = new Workbook();
                    }

                    //Pagina 1
                    string sheetName = "Ventas al Crédito Pendientes";
                    WorksheetPart worksheetPart = null;
                    worksheetPart = wbPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();

                    // Create custom widths for columns
                    Columns lstColumns = new Columns();

                    lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 15, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 8, Max = 8, Width = 25, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 9, Max = 9, Width = 25, CustomWidth = true });


                    worksheetPart.Worksheet = new Worksheet(lstColumns, sheetData);


                    if (wbPart.Workbook.Sheets == null)
                    {
                        wbPart.Workbook.AppendChild<Sheets>(new Sheets());
                    }

                    var sheet = new Sheet()
                    {
                        Id = wbPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = sheetName
                    };

                    var workingSheet = ((WorksheetPart)wbPart.GetPartById(sheet.Id)).Worksheet;

                    
                    List<Venta> VentasPendientes = ViewModel.VentasList.Where(t => t.VentaCompletada == "No").ToList();
                    
                    Row row1 = new Row();
                    row1.RowIndex = (UInt32)1;
                    row1.AppendChild(AddCellWithText("Nombre Cliente"));
                    row1.AppendChild(AddCellWithText("Cedula Cliente"));
                    row1.AppendChild(AddCellWithText("Empresa"));
                    row1.AppendChild(AddCellWithText("N# Pagos"));
                    row1.AppendChild(AddCellWithText("Fecha Ultimo Pago"));
                    row1.AppendChild(AddCellWithText("Monto Pagado a la fecha"));
                    row1.AppendChild(AddCellWithText("Saldo Pendiente"));
                    row1.AppendChild(AddCellWithText("Monto Total"));
                    row1.AppendChild(AddCellWithText("Productos Comprado"));

                    sheetData.AppendChild(row1);

                    int rowindex = 3;

                    foreach (var emp in VentasPendientes)
                    {
                        Row row = new Row();
                        row.RowIndex = (UInt32)rowindex;

                        row.AppendChild(AddCellWithText(emp.Cliente.Nombre));
                        row.AppendChild(AddCellWithText(emp.Cliente.Cedula));
                        row.AppendChild(AddCellWithText(emp.Cliente.Compania));

                        
                        
                        double montoPago = 0;

                        if (emp.Pagos.Count() == 0 )
                        {
                            
                            row.AppendChild(AddCellWithText("0"));
                            row.AppendChild(AddCellWithText( "No tiene"  ));
                            row.AppendChild(AddCellWithText(montoPago.ToString()));
                            row.AppendChild(AddCellWithText(emp.MontoVenta.ToString()));
                        }
                        else
                        {
                            row.AppendChild(AddCellWithText(emp.Pagos.Count().ToString()));
                            row.AppendChild(AddCellWithText(emp.Pagos.Last().Fecha_Pago.ToString()));
                            
                            foreach (var i in emp.Pagos)
                            {
                                montoPago = i.Monto;
                            }

                            row.AppendChild(AddCellWithText(montoPago.ToString()));

                            row.AppendChild(AddCellWithText((emp.MontoVenta - montoPago).ToString()));

                        }
                        
                        row.AppendChild(AddCellWithText(emp.MontoVenta.ToString()));

                        String productos = "";

                        if (emp.Especificaciones_producto.Count() == 0  )
                        {
                            row.AppendChild(AddCellWithText( "El producto no está registrado"));
                        }
                        else
                        {
                            var cantidadProductos = emp.Especificaciones_producto.Count();

                            //Mostramos los productos comprados
                            foreach (var i in emp.Especificaciones_producto)
                            {
                                if (cantidadProductos > 1)
                                {
                                    productos = productos + i.Nombre + ", ";
                                }

                                else
                                {
                                    productos = i.Nombre;
                                }
                            }

                            row.AppendChild(AddCellWithText(productos));
                        }

                       

                        sheetData.AppendChild(row);
                        rowindex++;
                    }

                    wbPart.Workbook.Sheets.AppendChild(sheet);

                    //Hoja 2

                    WorksheetPart worksheetPart2 = null;
                    worksheetPart2 = wbPart.AddNewPart<WorksheetPart>();
                    var sheetData2 = new SheetData();

                    // Create custom widths for columns
                    Columns lstColumns2 = new Columns();

                    lstColumns2.Append(new Column() { Min = 1, Max = 1, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 2, Max = 2, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 3, Max = 3, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 4, Max = 4, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 5, Max = 5, Width = 15, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 6, Max = 6, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 7, Max = 7, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 8, Max = 8, Width = 25, CustomWidth = true });
                    lstColumns2.Append(new Column() { Min = 9, Max = 9, Width = 25, CustomWidth = true });


                    worksheetPart2.Worksheet = new Worksheet(lstColumns2, sheetData2);


                    if (wbPart.Workbook.Sheets == null)
                    {
                        wbPart.Workbook.AppendChild<Sheets>(new Sheets());
                    }

                    var sheet2 = new Sheet()
                    {
                        Id = wbPart.GetIdOfPart(worksheetPart2),
                        SheetId = 2,
                        Name = "Ventas al crédito completadas"
                    };

                    List<Venta> VentasCreditoCompletadas = ViewModel.VentasList.Where(t => t.VentaCompletada == "Si" && t.Tipo_Venta == "Crédito").ToList();               

                    var workingSheet2 = ((WorksheetPart)wbPart.GetPartById(sheet2.Id)).Worksheet;

                    Row row2 = new Row();
                    row2.RowIndex = (UInt32)1;
                    row2.AppendChild(AddCellWithText("Nombre Cliente"));
                    row2.AppendChild(AddCellWithText("Cedula Cliente"));
                    row2.AppendChild(AddCellWithText("Empresa"));
                    row2.AppendChild(AddCellWithText("N# Pagos"));
                    row2.AppendChild(AddCellWithText("Fecha Ultimo Pago"));
                    row2.AppendChild(AddCellWithText("Monto Pagado a la fecha"));
                    row2.AppendChild(AddCellWithText("Saldo Pendiente"));
                    row2.AppendChild(AddCellWithText("Monto Pagado"));
                    row2.AppendChild(AddCellWithText("Productos Comprados"));

                    sheetData2.AppendChild(row2);

                    int rowindex2 = 3;

                    foreach (var emp in VentasCreditoCompletadas)
                    {
                        Row row = new Row();
                        row.RowIndex = (UInt32)rowindex2;

                        row.AppendChild(AddCellWithText(emp.Cliente.Nombre));
                        row.AppendChild(AddCellWithText(emp.Cliente.Cedula));
                        row.AppendChild(AddCellWithText(emp.Cliente.Compania));


                        row.AppendChild(AddCellWithText(emp.Pagos.Count().ToString()));
                        row.AppendChild(AddCellWithText(emp.Pagos.Last().Fecha_Pago.ToString()));

                        double montoPago = 0;

                        foreach (var i in emp.Pagos)
                        {
                            montoPago = i.Monto;
                        }

                        row.AppendChild(AddCellWithText(montoPago.ToString()));

                        row.AppendChild(AddCellWithText((emp.MontoVenta - montoPago).ToString()));

                        row.AppendChild(AddCellWithText(emp.MontoVenta.ToString()));


                        String productos = "";

                        if (emp.Especificaciones_producto.Count() == 0)
                        {
                            row.AppendChild(AddCellWithText("El producto no está registrado"));
                        }
                        else
                        {
                            var cantidadProductos = emp.Especificaciones_producto.Count();

                            //Mostramos los productos comprados
                            foreach (var i in emp.Especificaciones_producto)
                            {
                                if (cantidadProductos > 1)
                                {
                                    productos = productos + i.Nombre + ", ";
                                }

                                else
                                {
                                    productos = i.Nombre;
                                }
                            }

                            row.AppendChild(AddCellWithText(productos));

                            sheetData2.AppendChild(row);
                            rowindex2++;
                        }

                        wbPart.Workbook.Sheets.AppendChild(sheet2);
                    }
                    //Hoja 3

                    WorksheetPart worksheetPart3 = null;
                    worksheetPart3 = wbPart.AddNewPart<WorksheetPart>();
                    var sheetData3 = new SheetData();

                    // Create custom widths for columns
                    Columns lstColumns3 = new Columns();

                    lstColumns3.Append(new Column() { Min = 1, Max = 1, Width = 25, CustomWidth = true });
                    lstColumns3.Append(new Column() { Min = 2, Max = 2, Width = 25, CustomWidth = true });
                    lstColumns3.Append(new Column() { Min = 3, Max = 3, Width = 25, CustomWidth = true });
                    lstColumns3.Append(new Column() { Min = 4, Max = 4, Width = 25, CustomWidth = true });
                    lstColumns3.Append(new Column() { Min = 5, Max = 5, Width = 25, CustomWidth = true });
                    lstColumns3.Append(new Column() { Min = 6, Max = 6, Width = 25, CustomWidth = true });

                    worksheetPart3.Worksheet = new Worksheet(lstColumns3, sheetData3);


                    if (wbPart.Workbook.Sheets == null)
                    {
                        wbPart.Workbook.AppendChild<Sheets>(new Sheets());
                    }

                    var sheet3 = new Sheet()
                    {
                        Id = wbPart.GetIdOfPart(worksheetPart3),
                        SheetId = 3,
                        Name = "Ventas al Contado completadas"
                    };
               
                    List<Venta> VentasContadoCompletadas = ViewModel.VentasList.Where(t => t.VentaCompletada == "Si" && t.Tipo_Venta == "Contado").ToList();


                    var workingSheet3 = ((WorksheetPart)wbPart.GetPartById(sheet3.Id)).Worksheet;

                    Row row3 = new Row();
                    row3.RowIndex = (UInt32)1;
                    row3.AppendChild(AddCellWithText("Nombre Cliente"));
                    row3.AppendChild(AddCellWithText("Cedula Cliente"));
                    row3.AppendChild(AddCellWithText("Empresa"));
                    row3.AppendChild(AddCellWithText("Fecha Venta"));
                    row3.AppendChild(AddCellWithText("Monto Total"));
                    row3.AppendChild(AddCellWithText("Productos Comprados"));

                    sheetData3.AppendChild(row3);

                    int rowindex3 = 3;

                    foreach (var emp in VentasContadoCompletadas)
                    {
                        Row row = new Row();
                        row.RowIndex = (UInt32)rowindex3;

                        //Validando si esta venta fue minima y no tuvo cliente
                        if (emp.Cliente == null )
                        {
                            row.AppendChild(AddCellWithText("No fue Ingresado"));
                            row.AppendChild(AddCellWithText("No fue Ingresado"));
                            row.AppendChild(AddCellWithText("No fue Ingresado"));
                        }

                        else
                        {
                            row.AppendChild(AddCellWithText(emp.Cliente.Nombre));
                            row.AppendChild(AddCellWithText(emp.Cliente.Cedula));
                            row.AppendChild(AddCellWithText(emp.Cliente.Compania));
                        }
                                
                        row.AppendChild(AddCellWithText(emp.Pagos.Last().Fecha_Pago.ToString()));
                        
                        row.AppendChild(AddCellWithText(emp.MontoVenta.ToString()));


                        String productos = "";

                        if (emp.Especificaciones_producto.Count() == 0)
                        {
                            row.AppendChild(AddCellWithText("El producto no está registrado"));
                        }
                        else
                        {
                            var cantidadProductos = emp.Especificaciones_producto.Count();

                            //Mostramos los productos comprados
                            foreach (var i in emp.Especificaciones_producto)
                            {
                                if (cantidadProductos > 1)
                                {
                                    productos = productos + i.Nombre + ", ";
                                }

                                else
                                {
                                    productos = i.Nombre;
                                }
                            }

                            row.AppendChild(AddCellWithText(productos));


                            sheetData3.AppendChild(row);
                            rowindex3++;
                        }
                    }

                    wbPart.Workbook.Sheets.AppendChild(sheet3);


                }

                MessageBoxResult result = MessageBox.Show("Se ha generado correctamente la hoja de excel, con la fecha actual, en la la carpeta bin/release",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Information);


                //Start the process

                //Get current process Path
                System.Diagnostics.Process process = new System.Diagnostics.Process();

                string file = System.IO.Path.Combine(Environment.CurrentDirectory, FileName);

                process.StartInfo.FileName = file;
                process.Start();
                //openexcel
                //Process.Start(filetoOpen); 
            }

            catch
            {
                MessageBoxResult result = MessageBox.Show("Hubo un error al generar la hoja de Excel, revise la documentación por favor.",
                                                  "Confirmation",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Exclamation);
            }
           

        }

        static Cell AddCellWithText(string text)
        {
            Cell c1 = new Cell();
            
            c1.DataType = CellValues.InlineString;

            InlineString inlineString = new InlineString();
            Text t = new Text();
            t.Text = text;
            inlineString.AppendChild(t);

            c1.AppendChild(inlineString);

            return c1;
        }

        private void BtnEditarVenta(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnAgregarPago (object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de detallar pagos
            Venta venta = ViewModel.SelectedVenta;
            windowPagos = new AgregarPagoWindow(ViewModel, venta);

            windowPagos.UpdatePagination += new EventHandler(EventoPaginacion);
            windowPagos.Show();
        }

        private void BtnDetalleVenta (object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de detallar un producto
            Venta venta = ViewModel.SelectedVenta;

            ventaWindow = new DetalleVentaWindow(ViewModel, venta );
            ventaWindow.Show();
        }


        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NextVenta(10);
            UtilidadPaginacion();
        }

        private void BtnPreviousClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousVenta(10);
            UtilidadPaginacion();
        }

        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstVenta(10);
            UtilidadPaginacion();
        }

        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LastVenta(10);
            UtilidadPaginacion();
        }

        /*
         * Función que se encarga de mostrar la pagina actual que se encuentra el usuario y validar que si esta 
         * Es La ultima página, o la primera, se desactive los botones.  
        */
        private void UtilidadPaginacion()
        {
            NumeroPaginaActual = (ViewModel.PageVentasNumber() + 1);
            NumeroPaginaMax = (ViewModel.PageVentasNumberMax());


            //Hotfix si se elimina el ultimo registro y se queda fuera de tabla
            if (NumeroPaginaActual > NumeroPaginaMax && NumeroPaginaMax != 0)
            {
                ViewModel.PreviousVenta(10);
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
            var venta = ViewModel.SelectedVenta;

            //Pestaña de confirmación

            if (MessageBox.Show(" Estás seguro que deseas eliminar la venta: del dia " + venta.Fecha_Venta + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //
            }
            else
            {
                ViewModel.DeleteVenta(venta);
                UtilidadPaginacion();
            }

        }

        private void VentaSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string FiltradoVenta = VentaSearchBox.Text;
            if (FiltradoVenta == string.Empty)
            {
                ViewModel.SearchVenta(FiltradoVenta);
            }
            else
            {
                ViewModel.SearchVenta(FiltradoVenta);
            }
        }
    }
}
