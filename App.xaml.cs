using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Vacation_Portal.HostBuilders;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {      
          Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            _host = Host.CreateDefaultBuilder()
                .AddServices()
                .CreateDbConnectionFactory()
                .AddMainWindow()
                .Build();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
