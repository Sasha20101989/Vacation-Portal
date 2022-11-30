using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
        public SplashScreen(Window mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(member, value))
            {
                return false;
            }
            member = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                Close();
                _mainWindow.ShowDialog();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Task.Run(async () => await GetUserAsync());

            for(int i = 0; i <= 100; i++)
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
            if(person != null)
            {
                person.GetSettings();
            }
            //person.AddPages(_viewModel);
        }
    }
}
