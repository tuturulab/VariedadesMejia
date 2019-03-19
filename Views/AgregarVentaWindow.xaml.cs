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
    /// Lógica de interacción para AgregarVentaWindow.xaml
    /// </summary>
    public partial class AgregarVentaWindow : Window
    {
        PageViewModel ViewModel;
        SelectClientWindow window;
        MultiUsesClienteWindow window2;

        public Cliente cliente;
        public Venta venta;
        public List<Especificacion_producto> Especificacion_Productos;

        public AgregarVentaWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

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
            
            

        }

        private void BtnInsertarPagos(object sender, RoutedEventArgs e)
        {
            // do something
        }

        //Recibiendo el id creado
        public void EventoActualizarCliente(object sender, EventArgs e)
        {
            //Obtenemos el cliente seleccionado de la ventana SelectClient
            cliente = ViewModel.SelectedClientWindow;
            ClienteTextBox.Text = cliente.Nombre;
            
        }

        public void EventoInsertarCliente (object sender, EventArgs e)
        {
            //Console.WriteLine(window2.cliente);

            //cliente = window2.cliente;
            //ClienteTextBox.Text = cliente.Nombre;
        }
        

        private void BtnSelectClient(object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window = new SelectClientWindow(ViewModel);

            //Subscribimos al evento
            window.EventSelectedClient += new EventHandler(EventoActualizarCliente);
            window.Show();
        }

        private void BtnCreateClient (object sender, RoutedEventArgs e)
        {
            //Iniciamos la ventana de crear un producto
            window2 = new MultiUsesClienteWindow(ViewModel);

            //Subscribimos al evento
            window2.PassClient += new EventHandler(EventoInsertarCliente);
           
            window2.Show();
        }
    }
}
