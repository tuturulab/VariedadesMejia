using ChromeCustom;
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
using Variedades.ViewModels;
using Variedades.Views;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : SWWindow
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

        // Hidde maximize button - Temporal fix
        // Try to figure out whty if i put the same code in the constructor does not work
        // I tried calling the base class constructor thought but nothing
        //
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            MaximizeButton.Visibility = Visibility.Hidden;
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
