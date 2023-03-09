﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.DTOs;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands {
    public class AddToApprovalListCommand : CommandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private string SelectedMode { get; set; }
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();

        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter) {
            SelectedMode = App.SelectedMode;

            _viewModel.Calendar.WorkingDays.Clear();
            foreach(DateTime date in _viewModel.PlannedItem.DateRange) {
                foreach(ObservableCollection<DayControl> month in _viewModel.Calendar.Year) {
                    foreach(DayControl item in month) {
                        Grid parentItem = item.Content as Grid;
                        UIElementCollection buttons = parentItem.Children;

                        for(int i = 0; i < buttons.Count; i++) {
                            UIElement elem = buttons[i];
                            Button button = elem as Button;
                            TextBlock buttonTextBlock = button.Content as TextBlock;
                            int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                            int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[0]);
                            int buttonYear = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[1]);
                            string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                            if((buttonNameOfDay == "Рабочий" || buttonNameOfDay == "Рабочий в выходной") &&
                                date.Day == buttonDay &&
                                date.Month == buttonMonth &&
                                date.Year == buttonYear) {
                                _viewModel.Calendar.WorkingDays.Add(true);
                                break;
                            }
                        }
                    }
                }
            }

            ObservableCollection<Vacation> VacationsToApproval = new ObservableCollection<Vacation>();
            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            if(SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                VacationsToApproval = new ObservableCollection<Vacation>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacations.Where(f => f.Date_Start.Year == _viewModel.CurrentYear));
                _viewModel.SelectedSubordinate.Subordinate_Vacations.Add(_viewModel.PlannedItem);
                VacationsToApproval = _viewModel.SelectedSubordinate.Subordinate_Vacations;
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else if(SelectedMode == WindowMode.Personal) {
                VacationsToApproval = new ObservableCollection<Vacation>(
                    App.API.Person.User_Vacations.Where(f => f.Date_Start.Year == _viewModel.CurrentYear));
                App.API.Person.User_Vacations.Add(_viewModel.PlannedItem);
                VacationsToApproval = App.API.Person.User_Vacations;
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            }

            _viewModel.SelectedItemAllowance.Vacation_Days_Quantity -= _viewModel.Calendar.CountSelectedDays;

            
            List<Vacation> mergedVacations = new List<Vacation>();

            foreach(Vacation vacation in VacationsToApproval.OrderBy(v => v.Date_Start)) {
                Vacation lastVacation = mergedVacations.LastOrDefault();
                Vacation firstVacation = mergedVacations.FirstOrDefault();

                if(lastVacation != null &&
                    lastVacation.Name == vacation.Name &&
                    (lastVacation.Vacation_Status_Name != MyEnumExtensions.ToDescriptionString(Statuses.OnApproval) && vacation.Vacation_Status_Name != MyEnumExtensions.ToDescriptionString(Statuses.OnApproval)) &&
                    lastVacation.Date_end.AddDays(1) == vacation.Date_Start) {
                    _viewModel.Calendar.WorkingDays.Add(true);
                    int countHolidays = 0;

                    foreach(DateTime date in lastVacation.DateRange) {
                        for(int h = 0; h < App.API.Holidays.Count; h++) {
                            if(date == App.API.Holidays[h].Date) {
                                countHolidays++;
                            }
                        }
                    }
                    lastVacation.Count += vacation.Count - countHolidays;
                    await App.API.DeleteVacationAsync(lastVacation);
                    await App.API.DeleteVacationAsync(vacation);
                    lastVacation.Date_end = vacation.Date_end;
                    lastVacation.Vacation_Status_Id = (int)Statuses.BeingPlanned;
                    _viewModel.ShowAlert("Несколько периодов объединены в один.");
                } else {
                    mergedVacations.Add(vacation);
                }
            }

            VacationsToAprovalClone = new ObservableCollection<Vacation>(mergedVacations);

            if(_viewModel.Calendar.WorkingDays.Contains(true)) {
                List<Vacation> conflictFreeVacations = new List<Vacation>();
                foreach(Vacation item in VacationsToAprovalClone) {
                    Vacation plannedVacation = new Vacation(item.Id, item.Name, item.User_Id_SAP, item.User_Name, item.User_Surname, item.Vacation_Id, item.Count, item.Color, item.Date_Start, item.Date_end, item.Vacation_Status_Id, item.Creator_Id);
                    IEnumerable<VacationDTO> conflictingVacations = await App.API.GetConflictingVacationAsync(plannedVacation);
                    if(!conflictingVacations.Any()) {
                        conflictFreeVacations.Add(item);
                    }
                }

                foreach(Vacation conflictFreeVacation in conflictFreeVacations) {
                    VacationAllowanceViewModel updatedAllowance = VacationAllowances.FirstOrDefault(a => a.Vacation_Name == conflictFreeVacation.Name);
                    await _viewModel.UpdateVacationAllowance(conflictFreeVacation.User_Id_SAP, conflictFreeVacation.Vacation_Id, conflictFreeVacation.Date_Start.Year, updatedAllowance.Vacation_Days_Quantity);
                    await App.API.AddVacationAsync(conflictFreeVacation);
                }

                if(SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                    _viewModel.SelectedSubordinate.Subordinate_Vacations = new ObservableCollection<Vacation>(VacationsToAprovalClone.OrderBy(i => i.Date_Start));
                    _viewModel.UpdateDataForSubordinate();
                } else if(App.SelectedMode == WindowMode.Personal) {
                    App.API.Person.User_Vacations = new ObservableCollection<Vacation>(VacationsToAprovalClone.OrderBy(i => i.Date_Start));
                    _viewModel.UpdateDataForPerson();
                }

                _viewModel.PlannedIndex = 0;
                _viewModel.PlannedVacationString = "";
                _viewModel.Calendar.ClicksOnCalendar = 0;

                if(_viewModel.SelectedItemAllowance.Vacation_Days_Quantity == 0) {
                    VacationAllowanceViewModel selectedAllowance = VacationAllowances.FirstOrDefault(a => a.Vacation_Days_Quantity > 0);
                    if(selectedAllowance != null) {
                        _viewModel.SelectedItemAllowance = selectedAllowance;
                    }
                }

            } else {
                _viewModel.ShowAlert("В выбранном периоде, отсутствуют рабочие дни выбранного типа отпуска.");
                _viewModel.SelectedItemAllowance.Vacation_Days_Quantity += _viewModel.Calendar.CountSelectedDays;
                if(SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                    _viewModel.SelectedSubordinate.Subordinate_Vacations.Remove(_viewModel.PlannedItem);
                    _viewModel.Calendar.ClearVacationData(_viewModel.SelectedSubordinate.Subordinate_Vacations);
                    _viewModel.UpdateDataForSubordinate();
                } else if(App.SelectedMode == WindowMode.Personal) {
                    App.API.Person.User_Vacations.Remove(_viewModel.PlannedItem);
                    _viewModel.Calendar.ClearVacationData(App.API.Person.User_Vacations);
                    _viewModel.UpdateDataForPerson();
                }
            }
        }
    }
}
