using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Variedades.Models;

namespace Variedades
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PageViewModel MainViewModel;
        public User thisUser;


        public MainWindow(PageViewModel viewModel, User user)
        {
            InitializeComponent();
            //Instantiate Viewmodel
            MainViewModel = viewModel;
            DataContext = MainViewModel;

            thisUser = user;

            using(var context = new DbmejiaEntities())
            {
                try
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Verifica tus servicios de base de datos");
                    this.Close();
                }
            }

            //Pagina Inicial
            var PaginaEstadisticas = new Views.PageEstadisticas();
            ContentMain.Navigate(PaginaEstadisticas);
        }

        private void BtnOpenManual(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            string file = System.IO.Path.Combine(Environment.CurrentDirectory, @"Manual.pdf");
                
            process.StartInfo.FileName = file;
            process.Start();   
        }

        //Hide and show sidebar menu
        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            //Codigo para generar un diagrama de Db para tener una foto 
            /*using (var ctx = new Models.DbmejiaEntities())
            {
                using (var writer = new XmlTextWriter("./Model.edmx", Encoding.Default))
                {
                    EdmxWriter.WriteEdmx(ctx, writer);
                }
            } */           
            

            if (Sidebar.Width == new GridLength(1, GridUnitType.Star))
            {
                Duration duration = new Duration(TimeSpan.FromMilliseconds(500));

                var animation = new GridLengthAnimation
                {
                    Duration = duration,
                    From = new GridLength(1, GridUnitType.Star),
                    To = new GridLength(0, GridUnitType.Star)
                };

                Sidebar.BeginAnimation(ColumnDefinition.WidthProperty, animation);
            }
            else
            {
                Duration duration = new Duration(TimeSpan.FromMilliseconds(500));

                var animation = new GridLengthAnimation
                {
                    Duration = duration,
                    From = new GridLength(0, GridUnitType.Star),
                    To = new GridLength(1, GridUnitType.Star)
                };

                Sidebar.BeginAnimation(ColumnDefinition.WidthProperty, animation);
            }
        }

        //Change content usercontrol from sidebar menu
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Change usercontrol 
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemClients":
                    MainViewModel.UpdateClients(10);
                    var PaginaClientes = new Views.PageClientes(MainViewModel);
                    ContentMain.Navigate(PaginaClientes);
                    break;

                case "ItemProducts":
                    MainViewModel.UpdateProducts(10);
                    var PaginaProductos = new Views.PageProducts(MainViewModel, thisUser);
                    ContentMain.Navigate(PaginaProductos);
                    break;

                case "ItemSales":
                    MainViewModel.UpdateVentas(10);
                    var PaginaVentas = new Views.PageVentas (MainViewModel);
                    ContentMain.Navigate(PaginaVentas);
                    break;

                case "ItemImports":
                    MainViewModel.UpdateImportacion(10);
                    var PaginaImportaciones = new Views.PageImportaciones(MainViewModel, thisUser) ;
                    ContentMain.Navigate(PaginaImportaciones);
                    break;

                case "ItemPedidos":
                    MainViewModel.UpdatePedido(10);
                    var PaginaPedidos = new Views.PagePedidos(MainViewModel , thisUser);
                    ContentMain.Navigate(PaginaPedidos);
                    break;


                case "ItemStats":

                    ContentMain.Navigate(new Views.PageEstadisticas());
                    break;

                default:
                    break;
            }
        }
    }
}
