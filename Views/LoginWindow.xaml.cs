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

namespace Variedades
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MainWindow mainWindow;

        public LoginWindow()
        {
            InitializeComponent();
        }

        //Start main window
        private void LoginButton(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text == "admin" && PassTextBox.Password == "1234")
            {
                //Iniciamos la ventana de crear un producto
                mainWindow = new MainWindow();

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
