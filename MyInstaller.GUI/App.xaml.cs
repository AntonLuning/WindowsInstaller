using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyInstaller.GUI
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new();

            if (e.Args.Contains("-l") || e.Args.Contains("-L"))
            {   // Run with logging information
                
            }

            window.Show();
        }
    }
}
