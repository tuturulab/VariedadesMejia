﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Variedades.Models;

namespace Variedades.Views
{
    /// <summary>
    /// Lógica de interacción para MultiUsesClienteWindow.xaml
    /// </summary>
    public partial class MultiUsesClienteWindow : Window
    {
        //Evento de Actualizar Paginacion
        public event EventHandler UpdatePagination;

        //Evento de Pasar cliente
        public event EventHandler PassClient;


        ObservableCollection<Telefonos> TelefonosList = new ObservableCollection<Telefonos>();

        public PageViewModel ViewModel;
        public Cliente cliente;

        
        public MultiUsesClienteWindow(PageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            EventoPasarCliente();
        }

        //Si la ventana de agregar Cliente es llamada desde ventas o pedido
        private void EventoPasarCliente()
        {
            PassClient?.Invoke(this, EventArgs.Empty);
        }
        
        //Validación
        private void EventoPaginacion()
        {
            UpdatePagination?.Invoke(this, EventArgs.Empty);
        }

        public void BtnInsertarCliente(object sender, RoutedEventArgs e)
        {
            try
            {

                if (String.IsNullOrEmpty(NombreTextBox.Text) == false)
                {
                    if (String.IsNullOrEmpty(DiaPago1TextBox.Text) == true || int.Parse(DiaPago1TextBox.Text) >31 || int.Parse(DiaPago1TextBox.Text) < 1)
                    {
                        MessageBoxResult result = MessageBox.Show("Por Favor Ingrese almenos un dia de pago, y asegurese de que sea entre 1 y 30 dias", "Confirmation",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Exclamation);
                    }
                    
                    else
                    {
                        //Ingresando el Cliente
                        cliente = new Cliente()
                        {
                            Nombre = NombreTextBox.Text,
                            Email = EmailTextBox.Text,
                            Domicilio = DomicilioTextBox.Text,
                            Tipo_Pago = "Cordobas",
                            Compania = CompañiaTextBox.Text,
                            Fecha_Pago_1 = int.Parse(DiaPago1TextBox.Text),
                        };


                        //Parametro opcional
                        if (String.IsNullOrEmpty (DiaPago2TextBox.Text) == false )
                        {
                            cliente.Fecha_Pago_2 = int.Parse(DiaPago2TextBox.Text);
                        }
                        
                        var Telefonos = new List<Telefono>();
                        //Si el producto tiene Imeis se agregan, de lo contrario no
                        var Cantidad = int.Parse(CantidadTextBox.Text);
                        foreach (var item in TelefonosList)
                        {
                            Telefonos.Add(new Telefono() { Cliente = cliente, Numero = item.Numero, Empresa = item.Empresa, Tipo_Numero = item.Tipo_Numero });
                        }


                        //Agregamos a la base de datos y actualizamos la paginación
                        ViewModel.AddClient(cliente, Telefonos);

                        EventoPaginacion();

                        //Si no se le subscribio un evento por tanto fue llamado desde la pagina cliente
                        if (PassClient == null)
                        {
                            if (MessageBox.Show("Se ha ingresado correctamente el cliente, ¿desea seguir ingresando clientes?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            {
                                this.Close();
                            }
                            else
                            {
                                //Limpiamos los campos para seguir insertando
                                NombreTextBox.Text = String.Empty;
                                EmailTextBox.Text = String.Empty;
                                DomicilioTextBox.Text = String.Empty;
                                TipoPagoComboBox.Text = String.Empty;

                            }
                        }

                        //Si fue llamado desde una subventana
                        else
                        {
                            EventoPasarCliente();
                            this.Close();
                        }
                        
                    }
                    
                }

                else
                {
                    MessageBoxResult result = MessageBox.Show("Ingrese el nombre del cliente por favor",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
                }

            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Error al ingresar en la base de datos",
                                                 "Confirmation",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation);
            }

        }

        //Validar que en los campos numericos solo se escriban numeros
        public void TextBoxNumerico(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Agregar telefonos
        private void BtnInsertarTelefono(object sender, RoutedEventArgs e)
        {
            //Limpiamos la anterior lista
            TelefonosList.Clear();

            //Generamos la cantidad de registros a agregar
            var cantidad = int.Parse(CantidadTextBox.Text);

            for (int i = 0; i < cantidad; i++)
            {
                TelefonosList.Add(new Telefonos() { IdNumero = (i + 1), Numero = " ", Tipo_Numero = " ", Empresa = " " });
            }

            //Los seteamos en el datagris
            TelefonoDatagrid.ItemsSource = TelefonosList;
        }

    }

    //Clase para generar la lista de Telefonos
    public class Telefonos
    {
        public int IdNumero { get; set; }
        public string Numero { get; set; }
        public string Tipo_Numero { get; set; }
        public string Empresa { get; set; }

    }
}
