using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.DTOs;
using Vacation_Portal.Extensions;
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
        public override async Task ExecuteAsync(object parameter)
        {
            _viewModel.IsSaving = true;
            _viewModel.IsEnabled = false;
            bool isPersonalView = App.SelectedMode == WindowMode.Personal;
            bool isSupervisorView = App.SelectedMode == WindowMode.Subordinate;
            switch(App.SelectedMode) {
                case WindowMode.Subordinate:
                case WindowMode.HR_GOD:
                    VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.SelectedSubordinate.Subordinate_Vacations.OrderByDescending(i => i.Count));
                    VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(_viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances);
                    _viewModel.UpdateDataForSubordinate();
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

            #region Merged vacations
            var vacationsToMerge = GroupVacationsByDateRange(VacationsToAproval);
            var vacationsToAdd = new List<Vacation>();

            foreach(var vacation in VacationsToAproval) {
                if(!vacation.UsedForMerging) {
                    vacationsToAdd.Add(vacation);
                }
            }

            foreach(var vacation in vacationsToAdd) {
                vacationsToMerge.Add(new List<Vacation> { vacation });
            }
            var vacationsToRemove = new List<Vacation>();

            foreach(var vacation in VacationsToAproval) {
                    if(!vacationsToMerge.SelectMany(g => g).Any(v => v.Id == vacation.Id)) {
                        vacationsToRemove.Add(vacation);
                    }
            }

            foreach(var vacation in vacationsToRemove) {
                await App.API.DeleteVacationAsync(vacation);
            }
            //await MergeVacationsByDateRange(vacationsToMerge, App.SelectedMode == WindowMode.Personal, App.SelectedMode == WindowMode.Subordinate);
            #endregion

            foreach(Vacation item in VacationsToAproval) {
                int countConflicts = 0;
                Vacation plannedVacation = new Vacation(item.Id, item.Name, item.User_Id_SAP, item.User_Name, item.User_Surname, item.Vacation_Id, item.Count, item.Color, item.Date_Start, item.Date_end, item.Vacation_Status_Id, item.Creator_Id);
                IEnumerable<VacationDTO> conflictingVacations = await App.API.GetConflictingVacationAsync(plannedVacation);

                foreach(VacationDTO vacationDTO in conflictingVacations) {
                    countConflicts++;
                }
                if (countConflicts == 0)
                {
                    VacationAllowanceViewModel vacationAllowance = GetVacationAllowance(item.Name);
                    await _viewModel.UpdateVacationAllowance(item.User_Id_SAP, item.Vacation_Id, item.Date_Start.Year, vacationAllowance.Vacation_Days_Quantity);
                    await App.API.AddVacationAsync(plannedVacation);
                    if(isPersonalView) {
                        _viewModel.UpdateDataForPerson();
                    } else if(isSupervisorView) {
                        _viewModel.UpdateDataForSubordinate();
                    }
                } else {
                    item.Vacation_Status_Id = (int)Statuses.OnApproval;
                    if(isSupervisorView) {
                        item.Vacation_Status_Id = (int) Statuses.Approved;
                    }
                    await App.API.UpdateVacationStatusAsync(item);
                    if(isPersonalView) {
                        _viewModel.UpdateDataForPerson();
                    } else if(isSupervisorView) {
                        _viewModel.UpdateDataForSubordinate();
                    }
                }
            }
            if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                _viewModel.UpdateDataForSubordinate();
            } else if(App.SelectedMode == WindowMode.Personal) {
                _viewModel.UpdateDataForPerson();
            }

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

        private List<List<Vacation>> GroupVacationsByDateRange(ObservableCollection<Vacation> vacations) {
            var groups = vacations
                .Where(v => v.Vacation_Status_Id == (int) Statuses.OnApproval)
                .GroupBy(v => (v.Name))
                .Select(group => {
                    Vacation firstVacation = group.First();
                    var mergedVacation = new Vacation(
                        firstVacation.Id,
                        firstVacation.Name,
                        firstVacation.User_Id_SAP,
                        firstVacation.User_Name,
                        firstVacation.User_Surname,
                        firstVacation.Vacation_Id,
                        group.Sum(v => v.Count),
                        firstVacation.Color,
                        group.Min(v => v.Date_Start),
                        group.Max(v => v.Date_end),
                        firstVacation.Vacation_Status_Id,
                        firstVacation.Creator_Id
                    );
                
                    // Установить флаг, что отпуск использован при объединении
                    foreach(var vacation in group) {
                        vacation.UsedForMerging = true;
                    }
                
                    return mergedVacation;
                })
                .GroupBy(v => v.Name)
                .Select(g => g.Cast<Vacation>().ToList())
                .ToList();

            return groups;
        }

        private async Task MergeVacationsByDateRange(List<List<Vacation>> vacationsToMerge, bool isPersonalView, bool isSupervisorView) {
            foreach(var vacations in vacationsToMerge) {
                if(vacations.Count() > 1) {
                    var vacationList = vacations.ToList();
                    var vacationToSave = new Vacation(
                        vacationList[0].Id,
                        vacationList[0].Name,
                        vacationList[0].User_Id_SAP,
                        vacationList[0].User_Name,
                        vacationList[0].User_Surname,
                        vacationList[0].Vacation_Id,
                        vacationList.Sum(x => x.Count),
                        vacationList[0].Color,
                        vacationList.Min(x => x.Date_Start),
                        vacationList.Max(x => x.Date_end),
                        (int) Statuses.OnApproval,
                        vacationList[0].Creator_Id
                    );

                    foreach(var vacation in vacationList) {
                        await App.API.DeleteVacationAsync(vacation);
                    }

                    var countConflicts = (await App.API.GetConflictingVacationAsync(vacationToSave)).Count();

                    if(countConflicts == 0) {
                        var vacationAllowance = GetVacationAllowance(vacationToSave.Name);
                        await _viewModel.UpdateVacationAllowance(vacationToSave.User_Id_SAP, vacationToSave.Vacation_Id, vacationToSave.Date_Start.Year, vacationAllowance.Vacation_Days_Quantity);
                        await App.API.AddVacationAsync(vacationToSave);

                        if(isPersonalView) {
                            _viewModel.UpdateDataForPerson();
                        } else if(isSupervisorView) {
                            _viewModel.UpdateDataForSubordinate();
                        }
                    } else {
                        vacationToSave.Vacation_Status_Id = (int) Statuses.OnApproval;

                        if(isSupervisorView) {
                            vacationToSave.Vacation_Status_Id = (int) Statuses.Approved;
                        }

                        await App.API.AddVacationAsync(vacationToSave);

                        if(isPersonalView) {
                            _viewModel.UpdateDataForPerson();
                        } else if(isSupervisorView) {
                            _viewModel.UpdateDataForSubordinate();
                        }
                    }
                } else {
                    var vacation = vacations.First();

                    var countConflicts = (await App.API.GetConflictingVacationAsync(vacation)).Count();

                    if(countConflicts == 0) {
                        var vacationAllowance = GetVacationAllowance(vacation.Name);
                        await _viewModel.UpdateVacationAllowance(vacation.User_Id_SAP, vacation.Vacation_Id, vacation.Date_Start.Year, vacationAllowance.Vacation_Days_Quantity);

                        if(isPersonalView) {
                            _viewModel.UpdateDataForPerson();
                        } else if(isSupervisorView) {
                            _viewModel.UpdateDataForSubordinate();
                        }
                    } else {
                        vacation.Vacation_Status_Id = (int) Statuses.OnApproval;

                        if(isSupervisorView) {
                            vacation.Vacation_Status_Id = (int) Statuses.Approved;
                        }

                        await App.API.UpdateVacationStatusAsync(vacation);

                        if(isPersonalView) {
                            _viewModel.UpdateDataForPerson();
                        } else if(isSupervisorView) {
                            _viewModel.UpdateDataForSubordinate();
                        }
                    }
                }
            }
        }
    }
}
