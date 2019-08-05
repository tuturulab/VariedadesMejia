using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Variedades.Utils
{
    static class DatabaseService
    {
        //private readonly string dbServiceName = "MSSQL$SQLEXPRESS";

        static public void Start()
        {
            ServiceController sc = new ServiceController("MSSQL$SQLEXPRESS");

            if(sc.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch
                {
                    MessageBox.Show("Cannnot star database service");
                }
            }
            else
            {
                MessageBox.Show("Database service es already running");
            }
        }
        
        static public bool IsRunning()
        {
            ServiceController sc = new ServiceController("MSSQL$SQLEXPRESS");

            if(sc.Status == ServiceControllerStatus.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
