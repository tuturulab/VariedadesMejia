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

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para AgregarVentaWindow.xaml
    /// </summary>
    public partial class AgregarVentaWindow : Window
    {
        public AgregarVentaWindow()
        {
            InitializeComponent();
        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnInsertarVenta(object sender, RoutedEventArgs e)
        {
            // do something
        }

        private void BtnInsertarPagos(object sender, RoutedEventArgs e)
        {
            // do something
        }
    }
}
