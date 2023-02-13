using System;
using System.Windows.Media;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class InformationSplashScreenService : IInformationSplashScreenService
    {
        public void AccessSetting(string userRole, string Name)
        {
            if(userRole == "Worker")
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.UserGreeting.Text = "Здравствуйте, " + Name + "!";
                    App.SplashScreen.status.Text = "Ваше приложение с доступом сотрудника";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "Accounting")
            {
                App.API.Person.Is_Accounting = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.UserGreeting.Text = "Здравствуйте, " + Name + "!";
                    App.SplashScreen.status.Text = "Ваше приложение с доступом табельщика";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "Supervisor")
            {
                App.API.Person.Is_Supervisor = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.UserGreeting.Text = "Здравствуйте, " + Name + "!";
                    App.SplashScreen.status.Text = "Ваше приложение с доступом руководителя";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "HR")
            {
                App.API.Person.Is_HR = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.UserGreeting.Text = "Здравствуйте, " + Name + "!";
                    App.SplashScreen.status.Text = "Ваше приложение с доступом HR сотрудника";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
            if(userRole == "HR Admin")
            {
                App.API.Person.Is_HR_GOD = true;
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.UserGreeting.Text = "Здравствуйте, " + Name + "!";
                    App.SplashScreen.status.Text = "Ваше приложение с доступом бога";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
            }
        }
    }
}
