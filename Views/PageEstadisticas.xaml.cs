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

            List<Especificacion_producto> productos = _context.Especificacion_producto.Where(t => t.Vendido.Equals("Si")).ToList();
            int NumeroCelulares = 0;
            int NumeroLaptop = 0;
            int NumeroOtros = 0;
            int NumeroTablets = 0;
            int NumeroAccesorios = 0;

            foreach (var pr in productos)
            {
                if (pr.Tipo_Producto.Equals("Celular"))
                    NumeroCelulares++;
                else if (pr.Tipo_Producto.Equals("Tablet"))
                    NumeroTablets++;
                else if (pr.Tipo_Producto.Equals("Laptop"))
                    NumeroLaptop++;
                else if (pr.Tipo_Producto.Equals("Accesorio"))
                    NumeroAccesorios++;
                else
                    NumeroOtros++;
            }

            Celular_Label.Values = new ChartValues<double> { NumeroCelulares };
            Tablet_Label.Values = new ChartValues<double> { NumeroTablets };
            Laptop_Label.Values = new ChartValues<double> { NumeroLaptop };
            Acc_Label.Values = new ChartValues<double> { NumeroAccesorios };

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
