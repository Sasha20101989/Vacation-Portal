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
            bool isPersonalView = App.SelectedMode == WindowMode.Personal;
            bool isSupervisorView = App.SelectedMode == WindowMode.Subordinate;
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

            VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAproval.OrderBy(i => i.Date_Start));

            _viewModel.IsSaveComplete = true;
            _viewModel.IsSaving = false;
            _viewModel.SaveProgress = 0;

            Timer timer1 = new Timer(3000);
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Enabled = true;

            foreach(Vacation item in VacationsToAproval) {
                int countConflicts = 0;
                Vacation plannedVacation = new Vacation(item.Id, item.Name, item.User_Id_SAP, item.User_Name, item.User_Surname, item.Vacation_Id, item.Count, item.Color, item.Date_Start, item.Date_end, item.Vacation_Status_Id, item.Creator_Id);
                IEnumerable<VacationDTO> conflictingVacations = await App.VacationAPI.GetConflictingVacationAsync(plannedVacation);

                foreach(VacationDTO vacationDTO in conflictingVacations) {
                    countConflicts++;
                }
                if(countConflicts == 0) {
                    VacationAllowanceViewModel vacationAllowance = GetVacationAllowance(item.Name);
                    await _viewModel.UpdateVacationAllowance(item.User_Id_SAP, item.Vacation_Id, item.Date_Start.Year, vacationAllowance.Vacation_Days_Quantity);
                    await App.VacationAPI.AddVacationAsync(plannedVacation);
                } else {
                    item.Vacation_Status_Id = (int) Statuses.OnApproval;
                    if(isSupervisorView) {
                        item.Vacation_Status_Id = (int) Statuses.Approved;
                    }
                    await App.VacationAPI.UpdateVacationStatusAsync(item.Vacation_Id, item.Vacation_Status_Id);
                }
            }
            if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                await _viewModel.UpdateDataForSubordinateAsync();
            } else if(App.SelectedMode == WindowMode.Personal) {
                _viewModel.UpdateDataForPerson();
            }
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
