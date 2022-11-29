using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        PersonalVacationPlanningViewModel _viewModel;
        private SampleError _sampleError = new SampleError();
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();
        private Vacation CheckedVacation { get; set; }
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
            
            bool is14DaysPlaned = false;
            bool is7DaysPlaned = false;
            VacationsToAprovalClone = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderByDescending(i => i.Count));
            for (int i = 0; i < VacationsToAprovalClone.Count; i++)
            {
                int countFirstPeriod = 0;
                if (VacationsToAprovalClone[i].Name == "Основной")
                {
                    Range<DateTime> range = _viewModel.ReturnRange(VacationsToAprovalClone[i]);
                    foreach (DateTime planedDate in range.Step(x => x.AddDays(1)))
                    {
                        countFirstPeriod++;
                        if (countFirstPeriod >= 14)
                        {
                            is14DaysPlaned = true;
                            CheckedVacation = VacationsToAprovalClone[i];
                        }
                    }
                }
                if (is14DaysPlaned)
                {
                    break;
                }
            }
            if (is14DaysPlaned)
            {
                for (int i = 0; i < VacationsToAprovalClone.Count; i++)
                {
                    int countSecondPeriod = 0;
                    if (VacationsToAprovalClone[i].Name == "Основной" && VacationsToAprovalClone[i] != CheckedVacation)
                    {
                        Range<DateTime> range = _viewModel.ReturnRange(VacationsToAprovalClone[i]);
                        foreach (DateTime planedDate in range.Step(x => x.AddDays(1)))
                        {
                            countSecondPeriod++;
                            if (countSecondPeriod >= 7)
                            {
                                is7DaysPlaned = true;
                            }
                        }
                        if (is7DaysPlaned)
                        {
                            GoToSave();
                            break;
                        }
                        else
                        {
                            _viewModel.ShowAlert("Среди периодов планового основного отпуска, должен быть один длительностью не менее 7 дней");
                            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", _viewModel.ExtendedClosingEventHandler);
                            GoToSave();
                        }
                    }
                }
            }
            else
            {
                _viewModel.ShowAlert("Среди периодов планового основного отпуска, должен быть один длительностью не менее 14 дней");
                Task<object> result = DialogHost.Show(_sampleError, "RootDialog", _viewModel.ExtendedClosingEventHandler);
            }
        }

        private void GoToSave()
        {
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
                        CheckedVacation = null;
                        _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAprovalClone.OrderBy(i => i.Date_Start));
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
            Task.Run(() => { });
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
