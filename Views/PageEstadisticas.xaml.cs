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
using Variedades.Models;
using Variedades.ViewModels;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para PageEstadisticas.xaml
    /// </summary>
    public partial class PageEstadisticas : Page
    {
        //private StatictisModel model;
        private DbmejiaEntities _context;
        public PageEstadisticas()
        {
            InitializeComponent();
            //model = new StatictisModel();
            //DataContext = model;

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;

            _context = new DbmejiaEntities();
            //Calculate

            List<Producto> productos = _context.Producto.ToList();
            _context.Venta.ToList().ForEach(item =>
            {

            });

        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
