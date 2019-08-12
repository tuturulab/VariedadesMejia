using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Variedades.CefConfig;
using Variedades.Models;
using Variedades.Utils;

namespace Variedades
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Key license here
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTI5NDgzQDMxMzcyZTMyMmUzMG5sSzFWZS84V1Z6THR1d3MxemJRZXFJWnpCdnBwcHp1TTB0elg1dWJ3N3M9");
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            /*if (!DatabaseService.IsRunning())
            {
                MessageBox.Show("Porfavor iniciar el servicio de bases de datos");
                Shutdown(1);
                Debugger.Break();
                return;
            }*/

            /*
            using(var entities = new DbmejiaEntities())
            {
                try
                {
                    entities.Database.Connection.Open();
                    entities.Database.Connection.Close();
                }
                catch
                {
                    MessageBox.Show("Porfavor iniciar el servicio de bases de datos");
                    Shutdown(1);
                    return;
                }
            }

            */
            var settingsCef = new CefSettings();

            settingsCef.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CustomProtocolSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CustomProtocolSchemeHandlerFactory()
            });

            Cef.Initialize(settingsCef);
            base.OnStartup(e);
        }
    }
}
