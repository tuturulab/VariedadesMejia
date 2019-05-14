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
    /// Lógica de interacción para CrearCuentaWindow.xaml
    /// </summary>
    public partial class CrearCuentaWindow : Window
    {
        PageViewModel ViewModel;
        string role;
        MainWindow mainWindow;

        public CrearCuentaWindow(PageViewModel viewModel, bool firstLoad = false)
        {
            InitializeComponent();
            ViewModel = viewModel;

            //Create a admin account by default
            if (firstLoad == true)
            {
                role = "Administrador";
                TypeAccountSelector.Visibility = Visibility.Hidden;
            }

        }

        private void CreateAccount (Models.User user)
        {
            var newUser = ViewModel.CreateAccount(user);

            if (newUser != null)
            {
                mainWindow = new MainWindow(ViewModel, newUser);
                mainWindow.Show();
                this.Close();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text != String.Empty && NombreTextBox.Text != String.Empty && PassTextBox.Password != String.Empty )
            {
                if (role != "Administrador")
                {
                    if (CategoriaComboBox.SelectedIndex != -1)
                    {
                        var user = new Models.User();
                        user.Email = UserTextBox.Text;
                        user.Password = PassTextBox.Password;
                        user.Role = CategoriaComboBox.Text;
                        user.Nombre = NombreTextBox.Text;
                        CreateAccount(user);
                    }

                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Seleccione un rol de la cuenta, puede leer el manual para ver más detalles",
                                                "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
                    }
                }

                else
                {
                    var user = new Models.User();
                    user.Email = UserTextBox.Text;
                    user.Password = PassTextBox.Password;
                    user.Role = role;
                    user.Nombre = NombreTextBox.Text;
                    CreateAccount(user);
                }
            }
            
            else
            {
                MessageBoxResult result = MessageBox.Show("Por favor ingrese los datos correctamente",
                                                "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
            }
        }
    }
}
