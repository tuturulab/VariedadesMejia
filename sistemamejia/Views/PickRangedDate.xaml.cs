using System;
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

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PickRangedDate.xaml
    /// </summary>
    public partial class PickRangedDate : Window
    {
        public EventHandler ReturnDates;
        public PageViewModel viewModel;
        public PickRangedDate(PageViewModel model)
        {
            InitializeComponent();
            viewModel = model;
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PickedDate1 = dateTimePicker1.Value ?? DateTime.Now;
            viewModel.PickedDate2 = dateTimePicker2.Value ?? DateTime.Now;
            ReturnDates?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
