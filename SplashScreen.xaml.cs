using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private readonly Window _mainWindow;
        private Person _person;
        public SplashScreen(Window mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if(progressBar.Value == 100)
            {
                if(_person != null)
                {
                    Close();
                    _mainWindow.Show();
                } else
                {
                    status.Text = "Вас нет в базе данных";
                }
            }
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action) async delegate
            {
                _person = await App.API.LoginAsync(Environment.UserName);
            });

            for(int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }
    }
}
