using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private readonly Window _mainWindow;
        public SplashScreen(Window mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (progressBar.Value == 100)
            {
                Close();
                _mainWindow.ShowDialog();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Run(async () => await GetUserAsync());
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(80);
            }
        }

        public async Task GetUserAsync()
        {
            await App.API.LoginAsync(Environment.UserName);
            await Task.Delay(3000);
            OnLoginSuccesed(App.API.Person);
        }

        private void OnLoginSuccesed(Person person)
        {
            if (person != null)
            {
                person.GetSettings();
            }
            //person.AddPages(_viewModel);
        }
    }
}
