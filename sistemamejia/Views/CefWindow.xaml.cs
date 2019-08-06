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

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for CefWindow.xaml
    /// </summary>
    public partial class CefWindow : Window
    {
        ChromiumWebBrowser browser;
        public CefWindow()
        {
            InitializeComponent();

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
            string initialPage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./bundle/welcome.html");
            browser = new ChromiumWebBrowser(initialPage);


            //Set browser settings
            BrowserSettings settings = new BrowserSettings();
            settings.FileAccessFromFileUrls = CefState.Enabled;
            settings.UniversalAccessFromFileUrls = CefState.Enabled;
            settings.Javascript = CefState.Enabled;

            browser.BrowserSettings = settings;
            browserContainer.Child = browser;
            //browser.LoadingStateChanged += initalizedBrowser;
            browser.IsBrowserInitializedChanged += IsBrowserInitializedChangedHandler;
        }

        public void IsBrowserInitializedChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Task.Delay(4000);
            //browser.ShowDevTools();
            //string resultPage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./bundle/templateone.html");
            //string actualPage = System.IO.File.ReadAllText(resultPage);
           // browser.Load(resultPage);

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
    }
}
