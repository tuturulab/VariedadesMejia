using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Variedades.ViewModels;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para PageEstadisticas.xaml
    /// </summary>
    public partial class PageEstadisticas : Page
    {
        private StatictisModel model;
        public PageEstadisticas()
        {
            InitializeComponent();
            model = new StatictisModel();
            DataContext = model;
        }
    }
}
