﻿using Microsoft.Extensions.DependencyInjection;
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

namespace Vacation_Portal {
    public sealed partial class App : Application {
        private readonly IHost _host;
        public static ILunchRepository API { get; private set; }
        public static IStateRepository StateAPI { get; private set; }
        public static IVacationRepository VacationAPI { get; private set; }
        public static IVacationAllowanceRepository VacationAllowanceAPI { get; private set; }
        public static IHolidayRepository HolidayAPI { get; private set; }
        public static ICalendarRepository CalendarAPI { get; private set; }
        public static IAssetsRepository AssetsAPI { get; private set; }
        
        public static IInformationSplashScreenService SplashScreenService { get; private set; }
        
        public static IDependencyDetector DependencyDetector { get; private set; }
        public static SplashScreen SplashScreen { get; set; }
        public static string SelectedMode { get; set; }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();
        public App() {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            _host = Host.CreateDefaultBuilder()
                .CreateDbConnectionFactory()
                .AddMainWindow()
                .Build();
        }

        protected override void OnExit(ExitEventArgs e) {
            _host.StopAsync();
            //DependencyDetector.StopDependencyPerson();
            //DependencyDetector.StopDependencyPlannedHoliday();
            //DependencyDetector.StopDependencyPlannedVacation();
        }
        protected override void OnStartup(StartupEventArgs e) {
            _host.Start();

            API = new LunchRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            StateAPI = new StateRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            VacationAPI = new VacationRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            VacationAllowanceAPI = new VacationAllowanceRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            HolidayAPI = new HolidayRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            CalendarAPI = new CalendarRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            AssetsAPI = new AssetsRepository(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            DependencyDetector = new DependencyDetector(_host.Services.GetRequiredService<SqlDbConnectionFactory>());
            SplashScreenService = new InformationSplashScreenService();
            HolidayAPI.LoadHolidays.Execute(new object());
            //DependencyDetector.StartDependencyPerson();
            //DependencyDetector.StartDependencyPlannedHoliday();
            //DependencyDetector.StartDependencyPlannedVacation();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            SplashScreen = new SplashScreen(MainWindow);

            SplashScreen.Show();



            base.OnStartup(e);
        }
    }
}
