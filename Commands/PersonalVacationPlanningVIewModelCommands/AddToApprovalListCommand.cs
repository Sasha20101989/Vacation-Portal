﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.DTOs;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : AsyncComandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        public List<Vacation> VacationsToAprovalClone { get; set; } = new List<Vacation>();
        public ObservableCollection<Vacation> VacationsToApproval = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> PlannedSourceVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        private void UpdateWorkingDays(Vacation plannedItem)
        {
            _viewModel.Calendar.WorkingDays.Clear();
            foreach(DateTime date in plannedItem.DateRange)
            {
                foreach(ObservableCollection<DayControl> month in _viewModel.Calendar.Year)
                {
                    foreach(DayControl item in month)
                    {
                        Grid parentItem = item.Content as Grid;
                        UIElementCollection buttons = parentItem.Children;

                        for(int i = 0; i < buttons.Count; i++)
                        {
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
                                date.Year == buttonYear)
                            {
                                _viewModel.Calendar.WorkingDays.Add(true);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void SetVacationsAndVacationAllowances(string selectedMode, int currentYear, Vacation plannedItem)
        {
            if(selectedMode == WindowMode.Subordinate)
            {
                VacationsToApproval = new ObservableCollection<Vacation>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacations.Where(f => f.DateStart.Year == currentYear));

                _viewModel.SelectedSubordinate.Subordinate_Vacations.Add(plannedItem);

                VacationsToApproval = _viewModel.SelectedSubordinate.Subordinate_Vacations;

                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == currentYear));

            } else if(selectedMode == WindowMode.Personal)
            {
                VacationsToApproval = new ObservableCollection<Vacation>(
                    App.API.Person.User_Vacations.Where(f => f.DateStart.Year == currentYear));

                App.API.Person.User_Vacations.Add(plannedItem);

                VacationsToApproval = App.API.Person.User_Vacations;

                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == currentYear));
            }
        }

        private async Task<List<Vacation>> MergeVacationsAsync(ObservableCollection<Vacation> vacationsToApproval)
        {
            List<Vacation> mergedVacations = new List<Vacation>();
            foreach(Vacation vacation in vacationsToApproval.OrderBy(v => v.DateStart))
            {
                Vacation lastVacation = mergedVacations.LastOrDefault();
                Vacation firstVacation = mergedVacations.FirstOrDefault();
                if(lastVacation != null &&
                                    lastVacation.Name == vacation.Name &&
                                    lastVacation.VacationStatusName != MyEnumExtensions.ToDescriptionString(Statuses.OnApproval) &&
                                    vacation.VacationStatusName != MyEnumExtensions.ToDescriptionString(Statuses.OnApproval) &&
                                    lastVacation.DateEnd.AddDays(1) == vacation.DateStart)
                {
                    if(lastVacation.Source == "Planned" || vacation.Source == "Planned")
                    {
                        mergedVacations.Add(vacation);
                    } else
                    {
                        _viewModel.Calendar.WorkingDays.Add(true);
                        int countHolidays = 0;

                        foreach(DateTime date in lastVacation.DateRange)
                        {
                            for(int h = 0; h < App.HolidayAPI.Holidays.Count; h++)
                            {
                                if(date == App.HolidayAPI.Holidays[h].Date)
                                {
                                    countHolidays++;
                                }
                            }
                        }
                        lastVacation.Count += vacation.Count - countHolidays;
                        await App.VacationAPI.DeleteVacationAsync(lastVacation);//TODO: если source == planned То не удалять
                        await App.VacationAPI.DeleteVacationAsync(vacation);
                        lastVacation.DateEnd = vacation.DateEnd;
                        lastVacation.VacationStatusId = (int) Statuses.BeingPlanned;
                        _viewModel.ShowAlert("Несколько периодов объединены в один.");
                    }

                } else
                {
                    mergedVacations.Add(vacation);
                }
            }
            return mergedVacations.ToList();
        }

        private async Task<List<Vacation>> GetConflictingVacationsAsync(IEnumerable<Vacation> vacations)
        {
            List<Vacation> conflictFreeVacations = new List<Vacation>();
            foreach(Vacation vacation in vacations)
            {
                var conflictingVacations = await App.VacationAPI.GetConflictingVacationAsync(vacation);
                if(!conflictingVacations.Any())
                {
                    conflictFreeVacations.Add(vacation);
                }
            }
            return conflictFreeVacations;
        }
        private async Task UpdateVacationAllowanceAsync(int vacationId, int year, int vacationDaysQuantity)
        {
            VacationAllowanceViewModel allowance = VacationAllowances.FirstOrDefault(va => va.Vacation_Id == vacationId && va.Vacation_Year == year);
            await App.VacationAllowanceAPI.UpdateVacationAllowanceAsync(allowance.User_Id_SAP, allowance.Vacation_Id, allowance.Vacation_Year, allowance.Vacation_Days_Quantity);
        }
        private async Task UpdateVacationDataAsync(string selectedMode, List<Vacation> freeVacations, ObservableCollection<Vacation> plannedSourceVacations, PersonalVacationPlanningViewModel viewModel)
        {

            if(selectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                freeVacations.Union(plannedSourceVacations);
                viewModel.SelectedSubordinate.Subordinate_Vacations = new ObservableCollection<Vacation>(freeVacations.OrderBy(i => i.DateStart));
                await viewModel.UpdateDataForSubordinateAsync();
            } else if(selectedMode == WindowMode.Personal)
            {
                freeVacations.AddRange(plannedSourceVacations);
                App.API.Person.User_Vacations = new ObservableCollection<Vacation>(freeVacations.OrderBy(i => i.DateStart));
                viewModel.UpdateDataForPerson();
            }
        }
        public override async Task ExecuteAsync(object parameter)
        {
            //Метод содержит ряд шагов, необходимых для обновления данных отпусков в приложении, и,
            //в конечном итоге, выполняет одно из двух действий, в зависимости от того,
            //есть ли в выбранном периоде рабочие дни выбранного типа отпуска.
            //Если рабочие дни есть, метод добавляет отпуски, которые не конфликтуют с другими уже запланированными отпусками,
            //в список отпусков и обновляет соответствующие данные в приложении.
            //Если же рабочих дней нет, метод удаляет запланированный отпуск из списка и обновляет соответствующие данные в приложении
            string selectedMode = App.SelectedMode;
            UpdateWorkingDays(_viewModel.PlannedItem);
            SetVacationsAndVacationAllowances(selectedMode, _viewModel.CurrentYear, _viewModel.PlannedItem);

            _viewModel.SelectedItemAllowance.Vacation_Days_Quantity -= _viewModel.Calendar.CountSelectedDays;

            List<Vacation> mergedVacations = await MergeVacationsAsync(VacationsToApproval);

            PlannedSourceVacations = new ObservableCollection<Vacation>(mergedVacations.Where(f => f.Source == "Planned"));

            if(_viewModel.Calendar.WorkingDays.Contains(true))
            {
                List<Vacation> freeVacations = await GetConflictingVacationsAsync(mergedVacations.Where(mv => mv.Source != "Planned"));

                foreach(Vacation conflictFreeVacation in freeVacations)
                {
                    var updatedAllowance = VacationAllowances.FirstOrDefault(va => va.Vacation_Name == conflictFreeVacation.Name);
                    await UpdateVacationAllowanceAsync(conflictFreeVacation.VacationId, conflictFreeVacation.DateStart.Year, updatedAllowance.Vacation_Days_Quantity);
                    await App.VacationAPI.AddVacationAsync(conflictFreeVacation);
                }
               
                await UpdateVacationDataAsync(selectedMode, freeVacations, PlannedSourceVacations, _viewModel);

                _viewModel.PlannedIndex = 0;
                _viewModel.PlannedVacationString = "";
                _viewModel.Calendar.ClicksOnCalendar = 0;

                if(_viewModel.SelectedItemAllowance.Vacation_Days_Quantity == 0)
                {
                    VacationAllowanceViewModel selectedAllowance = VacationAllowances.FirstOrDefault(a => a.Vacation_Days_Quantity > 0);
                    if(selectedAllowance != null)
                    {
                        _viewModel.SelectedItemAllowance = selectedAllowance;
                    }
                }
            } else
            {
                _viewModel.ShowAlert("В выбранном периоде, отсутствуют рабочие дни выбранного типа отпуска.");
                _viewModel.SelectedItemAllowance.Vacation_Days_Quantity += _viewModel.Calendar.CountSelectedDays;
                if(selectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
                {
                    _viewModel.SelectedSubordinate.Subordinate_Vacations.Remove(_viewModel.PlannedItem);

                    await _viewModel.Calendar.ClearVacationData(_viewModel.SelectedSubordinate.Subordinate_Vacations);
                    await _viewModel.UpdateDataForSubordinateAsync();
                } else if(selectedMode == WindowMode.Personal)
                {
                    App.API.Person.User_Vacations.Remove(_viewModel.PlannedItem);

                    await _viewModel.Calendar.ClearVacationData(App.API.Person.User_Vacations);
                    _viewModel.UpdateDataForPerson();
                }
            }
        }

    }
}
