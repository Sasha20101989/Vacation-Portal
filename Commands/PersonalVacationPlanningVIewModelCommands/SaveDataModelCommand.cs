using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class SaveDataModelCommand : AsyncComandBase
    {
        PersonalVacationPlanningViewModel _viewModel;
        public SaveDataModelCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            if (_viewModel.IsSaveComplete)
            {
                _viewModel.IsSaveComplete = false;
                return;
            }
            if (_viewModel.SaveProgress != 0)
            {
                return;
            }
            var started = DateTime.Now;
            _viewModel.IsSaving = true;
            _viewModel.IsEnabled = false;
            new DispatcherTimer(
                TimeSpan.FromMilliseconds(50),
                DispatcherPriority.Normal,
                new EventHandler((o, e) =>
                {
                    var totalDuration = started.AddSeconds(3).Ticks - started.Ticks;
                    var currentProgress = DateTime.Now.Ticks - started.Ticks;
                    var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                    _viewModel.SaveProgress = currentProgressPercent;

                    if (_viewModel.SaveProgress >= 100)
                    {
                        _viewModel.IsSaveComplete = true;
                        _viewModel.IsSaving = false;
                        
                        _viewModel.SaveProgress = 0;
                        if (o is DispatcherTimer timer)
                        {
                            timer.Stop();
                        }
                        Timer timer1 = new Timer(3000);
                        timer1.Elapsed += Timer1_Elapsed;
                        timer1.Enabled = true;
                    }
                }), Dispatcher.CurrentDispatcher);
            //TODO: сохранение в базу данных
            await Task.Run(() => { });
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sender is Timer timer)
            {
                _viewModel.IsSaveComplete = false;
                _viewModel.IsEnabled = true;
                timer.Enabled = false;
            } 
        }
    }
}
