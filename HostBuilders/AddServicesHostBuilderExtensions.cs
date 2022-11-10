using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vacation_Portal.DbContext;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views;
using Vacation_Portal.Services.Providers;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => {
                services.AddSingleton<ISettingsProvider, SettingsProvider>();
                services.AddSingleton<IUserProvider, UserProvider>();
                services.AddSingleton<IDepartmentProvider, DepartmentProvider>();
            });
            return hostBuilder;
        }
        public static IHostBuilder CreateDbConnectionFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) => {
                services.AddSingleton(new SqlDbConnectionFactory(hostContext.Configuration.GetConnectionString("Default")));
            });
            return hostBuilder;
        }      
        public static IHostBuilder AddMainWindow(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) => {
                services.AddSingleton<VacationSummary>();
                services.AddSingleton((s) => new MainWindowViewModel(s.GetRequiredService<VacationSummary>()));
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            });
            return hostBuilder;
        }
    }
}
