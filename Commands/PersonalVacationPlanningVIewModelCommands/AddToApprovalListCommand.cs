using MiscUtil.Collections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();

        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            Range<DateTime> range = _viewModel.ReturnRange(_viewModel.PlannedItem);

            _viewModel.WorkingDays.Clear();
            foreach(DateTime date in range.Step(x => x.AddDays(1)))
            {
                foreach(ObservableCollection<DayControl> month in _viewModel.Year)
                {
                    foreach(DayControl item in month)
                    {
                        Grid parentItem = item.Content as Grid;
                        UIElementCollection buttons = parentItem.Children as UIElementCollection;

                        for(int i = 0; i < buttons.Count; i++)
                        {
                            UIElement elem = buttons[i];
                            Button button = elem as Button;
                            TextBlock buttonTextBlock = button.Content as TextBlock;
                            int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                            int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag);
                            string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                            if(buttonNameOfDay == "Рабочий" && date.Day == buttonDay && date.Month == buttonMonth)
                            {
                                _viewModel.WorkingDays.Add(true);
                                break;
                            }
                            //else
                            //{
                            //    _viewModel.WorkingDays.Add(false);
                            //}
                        }
                    }
                }
            }
            _viewModel.VacationsToAproval.Add(_viewModel.PlannedItem);
            _viewModel.SelectedItemAllowance.Vacation_Days_Quantity -= _viewModel.CountSelectedDays;

            _viewModel.DisplayedDateString = "";
            _viewModel.ClicksOnCalendar = 0;

            if(_viewModel.SelectedItemAllowance.Vacation_Days_Quantity == 0)
            {
                for(int i = 0; i < _viewModel.VacationAllowances.Count; i++)
                {
                    if(_viewModel.VacationAllowances[i].Vacation_Days_Quantity > 0)
                    {
                        _viewModel.SelectedItemAllowance = _viewModel.VacationAllowances[i];
                        break;
                    }
                }
            }

            //_viewModel.VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));
            //TODO: исправить
            //TODO: Проверка если рабочий в выходной
            VacationsToAprovalClone = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));

            for(int i = VacationsToAprovalClone.Count - 1; i > 0; i--)
            {
                if(VacationsToAprovalClone[i].Name == VacationsToAprovalClone[i - 1].Name)
                {
                    if(VacationsToAprovalClone[i].Date_Start.AddDays(-1) == VacationsToAprovalClone[i - 1].Date_end)
                    {
                        _viewModel.WorkingDays.Add(true);
                        if(_viewModel.WorkingDays.Contains(true))
                        {
                            VacationsToAprovalClone[i].Date_Start = VacationsToAprovalClone[i - 1].Date_Start;
                            int countDays = GetCountDays(VacationsToAprovalClone[i]);
                            VacationsToAprovalClone[i].Count = countDays;
                            VacationsToAprovalClone.Remove(VacationsToAprovalClone[i - 1]);
                            _viewModel.ShowAlert("Несколько периодов объединены в один.");
                        }
                    }
                }
            }
            if(_viewModel.WorkingDays.Contains(true))
            {
                _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAprovalClone.OrderBy(i => i.Date_Start));
            } else
            {
                _viewModel.ShowAlert("В выбранном периоде, отсутствуют рабочие дни выбранного типа отпуска.");
                _viewModel.SelectedItemAllowance.Vacation_Days_Quantity += _viewModel.CountSelectedDays;
                _viewModel.VacationsToAproval.Remove(_viewModel.PlannedItem);
                _viewModel.ClearVacationData();
            }
        }

        private int GetCountDays(Vacation vacation)
        {
            Range<DateTime> range = _viewModel.ReturnRange(vacation);
            int count = 0;
            foreach(DateTime date in range.Step(x => x.AddDays(1)))
            {
                count++;
            }
            return count;
        }
    }
}
