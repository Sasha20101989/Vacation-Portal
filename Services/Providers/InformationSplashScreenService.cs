using System;
using System.Windows.Media;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class InformationSplashScreenService : IInformationSplashScreenService
    {
        public void AccessSetting(string userRole)
        {
            if(userRole == "Worker")
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Приложение с доступом сотрудника";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "Accounting")
            {
                App.API.Person.Is_Accounting = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Приложение с доступом табельщика";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "Supervisor")
            {
                App.API.Person.Is_Supervisor = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Приложение с доступом руководителя";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "HR")
            {
                App.API.Person.Is_HR = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Приложение с доступом HR сотрудника";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
        }
    }
}
