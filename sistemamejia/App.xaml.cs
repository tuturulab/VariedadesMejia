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
        protected override void OnStartup(StartupEventArgs e)
        {

#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show("When running this outside of Visual Studio " +
                                "please make sure you compile in `Release` mode.", "Warning");
            }
#endif

            if (!DatabaseService.IsRunning())
            {
                MessageBox.Show("Porfavor iniciar el servicio de bases de datos");
                Shutdown(1);
                Debugger.Break();
                return;
            }

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
