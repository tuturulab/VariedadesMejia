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
using Variedades.Views;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MainWindow mainWindow;
        public PageViewModel MainViewModel;
        CrearCuentaWindow createAccWindow;

        public LoginWindow()
        {
            InitializeComponent();
            MainViewModel = new PageViewModel();
            DataContext = MainViewModel;

            bool firstExecution = MainViewModel.CheckIfAccountsExist();

            if (firstExecution == false)
            {
                CreateFirstAccout();
            }

        }

        private void CreateFirstAccout ()
        {
            createAccWindow = new CrearCuentaWindow(MainViewModel, true);

            createAccWindow.Show();
            this.Close();
        }

        //Start main window
        private void LoginButton(object sender, RoutedEventArgs e)
        {

            var user = MainViewModel.Login(UserTextBox.Text, PassTextBox.Password);
            
            if (user != null)
            {
                //Iniciamos la ventana de crear un producto
                mainWindow = new MainWindow(MainViewModel, user);

                //Abrimos
                mainWindow.Show();
                this.Close();
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Datos Incorrectos, por favor intente nuevamente",
                                                  "Confirmation",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Exclamation);
            }
        }
    }
}
