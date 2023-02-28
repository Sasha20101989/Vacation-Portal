using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vacation_Portal.DbContext;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.HostBuilders {
    public static class AddServicesHostBuilderExtensions {
        public static IHostBuilder CreateDbConnectionFactory(this IHostBuilder hostBuilder) {
            hostBuilder.ConfigureServices((hostContext, services) => {
                services.AddSingleton(new SqlDbConnectionFactory(hostContext.Configuration.GetConnectionString("Home")));
            });
            return hostBuilder;
        }
        public static IHostBuilder AddMainWindow(this IHostBuilder hostBuilder) {
            hostBuilder.ConfigureServices((hostContext, services) => {
                services.AddSingleton((s) => new MainWindowViewModel());
                services.AddSingleton(s => new MainWindow() {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            });
            return hostBuilder;
        }
    }
}
