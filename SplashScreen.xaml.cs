using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal {
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window {
        private readonly Window _mainWindow;
        private Person _person;
        public SplashScreen(Window mainWindow) {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // Запускаем асинхронную операцию в фоновом потоке
            _ = Task.Run(async () =>
              {
                // Получаем персону из API
                App.Current.Dispatcher.Invoke((Action) async delegate
                  {
                      _person = await App.API.LoginAsync(Environment.UserName);
                  });
                // Инициализируем таймер
                var timer = new System.Diagnostics.Stopwatch();
                  timer.Start();
                // Обновляем прогресс бар до тех пор, пока персона не будет получена
                while(_person == null)
                  {
                    // Рассчитываем прогресс на основе времени, прошедшего с начала операции
                    var progress = (int) (timer.ElapsedMilliseconds / 400);
                    // Обновляем прогресс бар
                    progressBar.Dispatcher.Invoke(() => {
                        progressBar.Value = progress;
                        if(progress > 100)
                        {
                            progressBar.IsIndeterminate = true;
                            status.Dispatcher.Invoke(() =>
                            {
                                status.Text = "Не волнуйтесь, всё еще в процессе.";
                            });
                        }else if(progress > 120)
                        {
                            status.Dispatcher.Invoke(() =>
                            {
                                status.Text = "Это всё из-за слабого соединения.";
                            });
                        } else if(progress > 130)
                        {
                            status.Dispatcher.Invoke(() =>
                            {
                                status.Text = "Потерпите еще не много.";
                            });
                        }
                    });
                    // Ждем некоторое время перед следующим обновлением
                    
                    await Task.Delay(10);
                  }
                // Останавливаем таймер
                timer.Stop();
                  // Если персона получена, закрываем окно и открываем главное окно приложения
                  _ = status.Dispatcher.Invoke(async () =>
                      {
                          if(_person != null)
                          {
                              App.SplashScreenService.AccessSetting(_person.User_Role, _person.Name);
                              progressBar.Value = 100;
                              progressBar.IsIndeterminate = false;
                              await Task.Delay(3000);
                              Close();
                              _mainWindow.Show();
                          } else
                          {
                              status.Text = "Вас нет в базе данных";
                          }
                      });
              });
        }
    }
}
