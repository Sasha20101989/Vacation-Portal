using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class SaveDataModelCommand : AsyncComandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();
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
            foreach(Vacation item in _viewModel.VacationsToAproval)
            {
                item.Status = "На согласовании";
                int countConflicts = 0;
                Vacation plannedVacation = new Vacation(item.Name, item.User_Id_SAP, item.Vacation_Id, item.Count, item.Color, item.Date_Start, item.Date_end, item.Status);
                IEnumerable<VacationDTO> conflictingVacations = await App.API.GetConflictingVacationAsync(plannedVacation);

                foreach(VacationDTO vacationDTO in conflictingVacations)
                {
                    countConflicts++;
                }
                if(countConflicts == 0)
                {
                    await App.API.AddVacationAsync(plannedVacation);
                    VacationAllowanceViewModel vacation = GetVacationAllowance(item.Name);
                    await _viewModel.UpdateVacationAllowance(item.Vacation_Id, item.Date_Start.Year, vacation.Vacation_Days_Quantity);
                } else
                {
                    _viewModel.ShowAlert("Такой отпуск уже существует");
                }
            }
        }

        private VacationAllowanceViewModel GetVacationAllowance(string name)
        {
            foreach(VacationAllowanceViewModel item in _viewModel.VacationAllowances)
            {
                if(item.Vacation_Name == name)
                {
                    return item;
                }
            }
            return null;
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
