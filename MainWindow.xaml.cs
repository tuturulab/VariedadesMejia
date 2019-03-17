﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

        public MainWindow()
        {
            InitializeComponent();
            //Instantiate Viewmodel
            MainViewModel = new PageViewModel();
            DataContext = MainViewModel;

        }

        //Hide and show sidebar menu
        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
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
                    var PaginaClientes = new Views.PageClientes(MainViewModel);
                    ContentMain.Navigate(PaginaClientes);
                    break;

                case "ItemProducts":
                    //var PaginaProductos = new Views.PageProducts { DataContext = this.DataContext, ViewModel = MainViewModel };
                    var PaginaProductos = new Views.PageProducts(MainViewModel);
                    ContentMain.Navigate(PaginaProductos);
                    break;

                case "ItemSales":
                    var PaginaVentas = new Views.PageVentas (MainViewModel);
                    ContentMain.Navigate(PaginaVentas);
                    break;

                case "ItemImports":
                    var PaginaImportaciones = new Views.PageImportaciones() ;
                    ContentMain.Navigate(new Views.PageImportaciones());
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