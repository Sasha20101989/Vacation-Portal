using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.HostBuilders;
using Vacation_Portal.MVVM.Views;
using Vacation_Portal.Services.Providers;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal
{
    public sealed partial class App : Application
    {
        private readonly IHost _host;
        public static ILunchRepository API { get; private set; }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();
        public App()
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            _host = Host.CreateDefaultBuilder()
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
            API = new LunchRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());

            MainWindow = _host.Services.GetRequiredService<MainWindow>();

            SplashScreen splashScreen = new SplashScreen(MainWindow);

            splashScreen.Show();

            base.OnStartup(e);
        }
    }
}
