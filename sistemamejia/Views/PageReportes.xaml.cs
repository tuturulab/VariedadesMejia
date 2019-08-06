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

            //File path
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./Resources/" + button.Name + ".html");

            //Read file as string
            string source = System.IO.File.ReadAllText(filePath);

            //Parse string
            var document = parser.ParseDocument(source);

            //tr Element
            var trElement = document.CreateElement("tr");
            trElement.SetAttribute("class", "active");

            //td elements
            var td1 = document.CreateElement("td");
            td1.TextContent = "From C#";
            trElement.AppendChild(td1);
            var td2 = document.CreateElement("td");
            td2.TextContent = "From C#";
            trElement.AppendChild(td2);
            var td3 = document.CreateElement("td");
            td3.TextContent = "From C#";
            trElement.AppendChild(td3);
            var td4 = document.CreateElement("td");
            td4.TextContent = "From C#";
            trElement.AppendChild(td4);
            var td5 = document.CreateElement("td");
            td5.TextContent = "From C#";
            trElement.AppendChild(td5);

            document.GetElementsByClassName("bodyTSource").FirstOrDefault().AppendChild(trElement);

            System.IO.File.WriteAllText(filePath, document.DocumentElement.OuterHtml);
            //System.IO.File.Create

            CefWindow cef = new CefWindow(button.Name);
            cef.Show();
        }
    }


}
