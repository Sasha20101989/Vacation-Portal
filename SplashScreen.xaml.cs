using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

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
                _mainWindow.Show();
            }
            if(status.Text == "Вас нет в базе данных")
            {
                Thread.Sleep(3000);
                Application.Current.Shutdown();
                Close();
            }
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for(int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }
    }
}
