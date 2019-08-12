using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
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
using Variedades.CefConfig;
using System.ComponentModel;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Diagnostics;

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for CefWindow.xaml
    /// </summary>
    public partial class CefWindow : Window
    {
        ChromiumWebBrowser browser;
        string templateName;
        public CefWindow(string typeReport, string pagePath)
        {
            InitializeComponent();
            templateName = typeReport;
            //Cef settings
            //var settingsCef = new CefSettings();

            //settingsCef.RegisterScheme(new CefCustomScheme
            //{
            //    SchemeName = CustomProtocolSchemeHandlerFactory.SchemeName,
            //    SchemeHandlerFactory = new CustomProtocolSchemeHandlerFactory()
            //});

            //Cef.EnableHighDPISupport();
            //Cef.Initialize(settingsCef);

            //Wee cannot initialize cef two times
            browser = new ChromiumWebBrowser(pagePath);


            //Set browser settings
            BrowserSettings settings = new BrowserSettings();
            settings.FileAccessFromFileUrls = CefState.Enabled;
            settings.UniversalAccessFromFileUrls = CefState.Enabled;
            settings.Javascript = CefState.Enabled;

            browser.BrowserSettings = settings;
            browserContainer.Child = browser;

            //browser.LoadingStateChanged += initalizedBrowser;
            browser.IsBrowserInitializedChanged += IsBrowserInitializedChangedHandler;
            browser.LoadingStateChanged += LoadingStateChangeHandler;
            
        }

        public void LoadingStateChangeHandler(object sender, LoadingStateChangedEventArgs args)
        {
            if(args.IsLoading == false)
            {
                Dispatcher.Invoke(() =>
                {
                    ProgressBar.Visibility = Visibility.Hidden;
                    //browserContainer.Child = browser;
                });
                //this.ProgressBar.Visibility = Visibility.Hidden;
                //browserContainer.Child = browser;
            }
        }

        public void IsBrowserInitializedChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            //Task.Delay(4000);
            //browser.ShowDevTools();
            //string resultPage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/" + templateName + ".html");
            //string actualPage = System.IO.File.ReadAllText(resultPage);
            //browser.Load(resultPage);

            //string pagePath = @"D:\Github\VariedadesMejia\sistemamejia\bin\x86\Debug\bundle\TemplateOne.html";
            //string page = System.IO.File.ReadAllText(pagePath);
            //browser.LoadHtml(page, "http://www.example.com/");
        }

        //private void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{

        //    // the browser control is initialized, now load the html
        //    BrowserSettings settings = new BrowserSettings();
        //    settings.FileAccessFromFileUrls = CefState.Enabled;
        //    settings.UniversalAccessFromFileUrls = CefState.Enabled;
        //    settings.Javascript = CefState.Enabled;

        //    cefBrowser.BrowserSettings = settings;

        //    string pagePath = @"D:\source\bootstrap-4.0.0\docs\4.0\examples\dashboard\index.html";
        //    string page = System.IO.File.ReadAllText(pagePath);

        //    //browser.ShowDevTools();

        //    cefBrowser.LoadHtml(page, "http://www.example.com/");
        //}

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            browser.Dispose();
            //browser = null;
            //Cef.Shutdown();
            base.OnClosing(e);
        }

        ~CefWindow()
        {
            //Clear memory allocations used by CEF
            browser.Dispose();
            //Cef.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument document = new PdfDocument();

            PdfPage page = document.Pages.Add();

            PdfGraphics pdfGraphics = page.Graphics;

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);

            pdfGraphics.DrawString("Testing!!", font, PdfBrushes.Black, new PointF(0,0));

            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Pdf/DocTest.pdf");

            document.Save(filePath);

            Process.Start(filePath);
        }
    }
}
