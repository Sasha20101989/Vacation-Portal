using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands {
    public class DayClickCommand : AsyncComandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private readonly CustomCalendar _customCalendar;

        public DayClickCommand(PersonalVacationPlanningViewModel viewModel, CustomCalendar customCalendar) {
            _viewModel = viewModel;
            _customCalendar = customCalendar;
        }

        public override bool CanExecute(object parameter) {
            return true;
        }

        public override async Task ExecuteAsync(object parameter) {
            if(App.CalendarAPI.IsCalendarPlannedOpen || _viewModel.CurrentYear == DateTime.Now.Year) {
                if(_viewModel.SelectedItemAllowance != null) {
                    ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
                    ObservableCollection<Vacation> VacationsToApproval = new ObservableCollection<Vacation>();
                    if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                        if(_viewModel.SelectedSubordinate != null) {
                            VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                            _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
                            VacationsToApproval = new ObservableCollection<Vacation>(_viewModel.SelectedSubordinate.Subordinate_Vacations.Where(f => f.DateStart.Year == _customCalendar.CurrentYear && f.VacationStatusId != (int)Statuses.ForTranfser));
                        } else {
                            _viewModel.ShowAlert("Сначала выберете подчинённого!");
                        }
                    } else if(App.SelectedMode == WindowMode.Personal) {
                        VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                            App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
                        VacationsToApproval = new ObservableCollection<Vacation>(App.API.Person.User_Vacations.Where(f => f.DateStart.Year == _customCalendar.CurrentYear && f.VacationStatusId != (int) Statuses.ForTranfser));
                    }
                    for(int i = 0; i < VacationAllowances.Count; i++) {
                        if(VacationAllowances[i].Vacation_Days_Quantity > 0) {
                            _customCalendar.CalendarClickable = true;
                            break;
                        } else {
                            _customCalendar.CalendarClickable = false;
                        }
                    }
                    if(_customCalendar.CalendarClickable) {
                        _customCalendar.ClicksOnCalendar++;
                        int countHolidays = 0;
                        int availableQuantity = 0;
                        DateTime newDate = new DateTime();

                        if(_customCalendar.ClicksOnCalendar >= 3) {
                            _customCalendar.PaintButtons();
                            _customCalendar.FirstSelectedDate = newDate;
                            _customCalendar.SecondSelectedDate = newDate;
                            _customCalendar.ClicksOnCalendar = 1;
                            await _customCalendar.UpdateColorAsync(VacationsToApproval);
                        }

                        if(_customCalendar.ClicksOnCalendar == 1) {
                            _customCalendar.FirstSelectedDate = new DateTime(_customCalendar.SelectedYear, _customCalendar.SelectedMonth, _customCalendar.SelectedDay);
                            _customCalendar.CountSelectedDays = _customCalendar.FirstSelectedDate.Subtract(_customCalendar.FirstSelectedDate).Days + 1;
                            if(_customCalendar.CountSelectedDays <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                                if(_customCalendar.SelectedNameDay != "Праздник") {
                                    _customCalendar.DayAddition = _customCalendar.GetDayAddition(_customCalendar.CountSelectedDays);
                                    _viewModel.PlannedVacationString = _customCalendar.DayAddition + ": " + _customCalendar.FirstSelectedDate.ToString("d.MM.yyyy");
                                    int statusId = (int) Statuses.BeingPlanned;
                                    if(App.SelectedMode == WindowMode.Personal) {
                                        _viewModel.PlannedItem = new Vacation("Draft", 0, _viewModel.SelectedItemAllowance.Vacation_Name, App.API.Person.Id_SAP, App.API.Person.Name, App.API.Person.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, _customCalendar.CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, _customCalendar.FirstSelectedDate, _customCalendar.FirstSelectedDate, statusId, Environment.UserName);
                                    } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                                        _viewModel.PlannedItem = new Vacation("Draft", 0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedSubordinate.Id_SAP, _viewModel.SelectedSubordinate.Name, _viewModel.SelectedSubordinate.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, _customCalendar.CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, _customCalendar.FirstSelectedDate, _customCalendar.FirstSelectedDate, statusId, Environment.UserName);
                                    }
                                } else {
                                    _viewModel.ShowAlert("Этот день является праздничным, начните планирование отпуска с другого дня");
                                }
                            } else {
                                _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                            }
                        } else {
                            _customCalendar.SecondSelectedDate = new DateTime(_customCalendar.SelectedYear, _customCalendar.SelectedMonth, _customCalendar.SelectedDay);
                            IEnumerable<DateTime> dateRange = _customCalendar.ReturnDateRange(_customCalendar.FirstSelectedDate, _customCalendar.SecondSelectedDate);

                            foreach(DateTime date in dateRange) {
                                for(int h = 0; h < App.HolidayAPI.Holidays.Count; h++) {
                                    if(date == App.HolidayAPI.Holidays[h].Date) {
                                        //TODO: Что делать если праздник выходной баг issue#141
                                        // 6 не залетает
                                        //if(date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                                        //{
                                            countHolidays++;
                                       // }
                                    }
                                }
                            }
                            //foreach(DateTime date in dateRange)
                            //{
                            //    for(int h = 0; h < App.HolidayAPI.Holidays.Count; h++)
                            //    {
                            //        if(date == App.HolidayAPI.Holidays[h].Date)
                            //        {
                            //            //TODO: Что делать если праздник выходной баг issue#141
                            //            //if(date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                            //            //{
                            //            countHolidays++;
                            //            //}
                            //        }
                            //    }
                            //}
                            if(_customCalendar.SecondSelectedDate > _customCalendar.FirstSelectedDate) {
                                _customCalendar.CountSelectedDays = _customCalendar.SecondSelectedDate.Subtract(_customCalendar.FirstSelectedDate).Days + 1;
                                availableQuantity = _customCalendar.CountSelectedDays - countHolidays;
                                if(availableQuantity <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                                    if(_customCalendar.SelectedNameDay != "Праздник") {
                                        _customCalendar.DayAddition = _customCalendar.GetDayAddition(_customCalendar.CountSelectedDays);
                                        _viewModel.PlannedVacationString = _customCalendar.DayAddition + ": " + _customCalendar.FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + _customCalendar.SecondSelectedDate.ToString("dd.MM.yyyy");

                                        int statusId = (int) Statuses.BeingPlanned;
                                        if(App.SelectedMode == WindowMode.Personal) {
                                            _viewModel.PlannedItem = new Vacation("Draft", 0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedItemAllowance.User_Id_SAP, App.API.Person.Name, App.API.Person.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, _customCalendar.CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, _customCalendar.FirstSelectedDate, _customCalendar.SecondSelectedDate, statusId, Environment.UserName);
                                        } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                                            _viewModel.PlannedItem = new Vacation("Draft", 0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedSubordinate.Id_SAP, _viewModel.SelectedSubordinate.Name, _viewModel.SelectedSubordinate.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, _customCalendar.CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, _customCalendar.FirstSelectedDate, _customCalendar.SecondSelectedDate, statusId, Environment.UserName);
                                        }
                                    } else {
                                        _viewModel.ShowAlert("Этот день является праздничным, закончите планирование отпуска другим днём");
                                    }
                                } else {
                                    _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                                }
                            } else {
                                _customCalendar.CountSelectedDays = _customCalendar.FirstSelectedDate.Subtract(_customCalendar.SecondSelectedDate).Days + 1;
                                availableQuantity = _customCalendar.CountSelectedDays - countHolidays;
                                if(availableQuantity <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                                    if(_customCalendar.SelectedNameDay != "Праздник") {

                                        _customCalendar.DayAddition = _customCalendar.GetDayAddition(_customCalendar.CountSelectedDays);
                                        _viewModel.PlannedVacationString = _customCalendar.DayAddition + ": " + _customCalendar.SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + _customCalendar.FirstSelectedDate.ToString("dd.MM.yyyy");

                                        int statusId = (int) Statuses.BeingPlanned;
                                        if(App.SelectedMode == WindowMode.Personal) {
                                            _viewModel.PlannedItem = new Vacation("Draft", 0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedItemAllowance.User_Id_SAP, App.API.Person.Name, App.API.Person.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, _customCalendar.CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, _customCalendar.SecondSelectedDate, _customCalendar.FirstSelectedDate, statusId, Environment.UserName);
                                        } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                                            _viewModel.PlannedItem = new Vacation("Draft", 0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedSubordinate.Id_SAP, _viewModel.SelectedSubordinate.Name, _viewModel.SelectedSubordinate.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, _customCalendar.CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, _customCalendar.SecondSelectedDate, _customCalendar.FirstSelectedDate, statusId, Environment.UserName);
                                        }
                                    } else {
                                        _viewModel.ShowAlert("Этот день является праздничным, закончите планирование отпуска с другим днём");
                                    }
                                } else {
                                    _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                                }
                            }
                        }
                        if(availableQuantity <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                            if(_viewModel.PlannedItem != null) {
                                List<bool> isGoToNext = new List<bool>();

                                foreach(DateTime planedDate in _viewModel.PlannedItem.DateRange) {
                                    for(int i = 0; i < VacationsToApproval.Count; i++) {
                                        Vacation existingVacation = VacationsToApproval[i];

                                        foreach(DateTime existingDate in existingVacation.DateRange) {
                                            if(existingDate == planedDate) {
                                                isGoToNext.Add(false);
                                            }
                                        }
                                        if(isGoToNext.Contains(false)) {
                                            break;
                                        }
                                    }
                                }

                                if(!isGoToNext.Contains(false)) {
                                    _customCalendar.PaintButtons();
                                } else {
                                    _viewModel.ShowAlert("Пересечение отпусков не допустимо");
                                    _customCalendar.PaintButtons();
                                    _customCalendar.FirstSelectedDate = newDate;
                                    _customCalendar.SecondSelectedDate = newDate;
                                    _customCalendar.ClicksOnCalendar = 0;
                                    _customCalendar.CountSelectedDays = 0;
                                    _viewModel.PlannedVacationString = "";
                                    await _customCalendar.UpdateColorAsync(VacationsToApproval);
                                }
                            }
                        } else {

                            _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                            _customCalendar.PaintButtons();
                            _customCalendar.FirstSelectedDate = newDate;
                            _customCalendar.SecondSelectedDate = newDate;
                            _customCalendar.ClicksOnCalendar = 0;
                            _customCalendar.CountSelectedDays = 0;
                            _viewModel.PlannedVacationString = "";
                            await _customCalendar.UpdateColorAsync(VacationsToApproval);
                        }
                    }
                } else {
                    _viewModel.ShowAlert("Выберете тип отпуска");
                }
            } else {
                _viewModel.ShowAlert($"Планирование будет доступно c {App.CalendarAPI.DateUnblockPlanning:dd.MM.yyyy}");
            }
        }
    }
}
