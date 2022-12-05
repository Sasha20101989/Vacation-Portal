using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class SaveDataModelCommand : AsyncComandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();
        private Vacation CheckedVacation { get; set; }
        public SaveDataModelCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            DateTime started = DateTime.Now;
            _viewModel.IsSaving = true;
            _viewModel.IsEnabled = false;
            _ = new DispatcherTimer(
                TimeSpan.FromMilliseconds(50),
                DispatcherPriority.Normal,
                new EventHandler((o, e) =>
                {
                    long totalDuration = started.AddSeconds(3).Ticks - started.Ticks;
                    long currentProgress = DateTime.Now.Ticks - started.Ticks;
                    double currentProgressPercent = 100.0 / totalDuration * currentProgress;

                    _viewModel.SaveProgress = currentProgressPercent;

                    if(_viewModel.SaveProgress >= 100)
                    {
                        CheckedVacation = null;
                        _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));
                        _viewModel.IsSaveComplete = true;
                        _viewModel.IsSaving = false;

                        _viewModel.SaveProgress = 0;
                        if(o is DispatcherTimer timer)
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
            if(sender is Timer timer)
            {
                _viewModel.IsSaveComplete = false;
                _viewModel.IsEnabled = true;
                timer.Enabled = false;
            }
        }
    }
}
