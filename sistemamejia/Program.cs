using System;

namespace Variedades
{
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            var application = new App();
            application.InitializeComponent();
            return application.Run();
        }
    }
}
