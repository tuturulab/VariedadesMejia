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

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageReportes.xaml
    /// </summary>
    public partial class PageReportes : Page
    {
        public PageReportes()
        {
            InitializeComponent();
        }

        private void Reporte1Event(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            CefWindow cef = new CefWindow(button.Name);
            cef.Show();
        }
    }
}
