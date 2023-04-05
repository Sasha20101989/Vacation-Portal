using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands {
    public class SaveDataModelCommand : AsyncComandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();
        private ObservableCollection<Vacation> VacationsToAproval { get; set; } = new ObservableCollection<Vacation>();
        private ObservableCollection<VacationAllowanceViewModel> VacationAllowances { get; set; } = new ObservableCollection<VacationAllowanceViewModel>();
        public SaveDataModelCommand(PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
        }
        public override async Task ExecuteAsync(object parameter) {
            _viewModel.IsSaving = true;
            _viewModel.IsEnabled = false;
            bool isPersonalView = App.SelectedMode == WindowMode.Personal || App.SelectedMode == WindowMode.Accounting;
            bool isSupervisorView = App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD;
            switch(App.SelectedMode) {
                case WindowMode.Subordinate:
                case WindowMode.HR_GOD:
                    VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.SelectedSubordinate.Subordinate_Vacations.OrderByDescending(i => i.Count));
                    VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(_viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances);
                    await _viewModel.UpdateDataForSubordinateAsync();
                    break;
                case WindowMode.Personal:
                    VacationsToAproval = new ObservableCollection<Vacation>(App.API.Person.User_Vacations.OrderByDescending(i => i.Count));
                    VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(App.API.Person.User_Vacation_Allowances);
                    _viewModel.UpdateDataForPerson();
                    break;
            }

            var timerInterval = TimeSpan.FromMilliseconds(50);
            var totalDuration = TimeSpan.FromSeconds(3);
            var started = DateTime.Now;
            while(DateTime.Now - started < totalDuration) {
                var currentProgressPercent = (DateTime.Now - started).TotalMilliseconds / totalDuration.TotalMilliseconds * 100.0;
                _viewModel.SaveProgress = currentProgressPercent;

                await Task.Delay(timerInterval);
            }

            VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAproval.OrderBy(i => i.DateStart));

            _viewModel.IsSaveComplete = true;
            _viewModel.IsSaving = false;
            _viewModel.SaveProgress = 0;

            Timer timer1 = new Timer(3000);
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Enabled = true;

            foreach(Vacation item in VacationsToAproval) {
                int countConflicts = 0;
                Vacation plannedVacation = new Vacation(item.Source, item.Id, item.Name, item.UserId, item.UserName, item.UserSurname, item.VacationId, item.Count, item.Color, item.DateStart, item.DateEnd, item.VacationStatusId, item.CreatorId);
                IEnumerable<VacationDTO> conflictingVacations = await App.VacationAPI.GetConflictingVacationAsync(plannedVacation);

                foreach(VacationDTO vacationDTO in conflictingVacations) {
                    countConflicts++;
                }
                if(countConflicts == 0) {
                    VacationAllowanceViewModel vacationAllowance = GetVacationAllowance(item.Name);
                    await _viewModel.UpdateVacationAllowance(item.UserId, item.VacationId, item.DateStart.Year, vacationAllowance.Vacation_Days_Quantity);
                    await App.VacationAPI.AddVacationAsync(plannedVacation);
                } else {
                    if(isSupervisorView) {
                        await App.VacationAPI.UpdateVacationStatusAsync(item, (int) Statuses.PassedToHR);
                    } else
                    {
                        await App.VacationAPI.UpdateVacationStatusAsync(item, (int) Statuses.OnApproval);
                    }
                }
            }
            App.API.PersonUpdated?.Invoke(null);
            App.API.GetPersonsWithVacationsOnApproval();
        }

        private VacationAllowanceViewModel GetVacationAllowance(string name) {
            foreach(VacationAllowanceViewModel item in VacationAllowances) {
                if(item.Vacation_Name == name) {
                    return item;
                }
            }
            return null;
        }
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e) {
            if(sender is Timer timer) {
                _viewModel.IsSaveComplete = false;
                _viewModel.IsEnabled = true;
                timer.Enabled = false;
            }
        }

    }
}
