using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private SampleError _sampleError = new SampleError();
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();

        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {

            //if (WorkingDays.Count > 1)
            //{
            //    if (WorkingDays.Contains(true))
            //    {
            //        button.Background = PlannedItem.Color;
            //    }
            //    else
            //    {
            //        ClicksOnCalendar = 0;
            //        CountSelectedDays = 0;
            //        DisplayedDateString = "";
            //        clearColorAndBlocked();
            //        WorkingDays.Clear();
            //        ShowAlert("В выбранном промежутке отсутствуют рабочие дни");
            //    }
            //}

            //int remainder = _viewModel.SelectedItem.Count - _viewModel.CountSelectedDays;
            //VacationsToAprovalClone.Clear();
            //VacationsToRemove.Clear();
            //bool isMorePlanedDays = remainder >= 0;
            //if (isMorePlanedDays)
            //{
            Range<DateTime> range = _viewModel.ReturnRange(_viewModel.PlannedItem);
            _viewModel.WorkingDays.Clear();
            foreach (DateTime date in range.Step(x => x.AddDays(1)))
            {
                foreach (ObservableCollection<DayControl> month in _viewModel.Year)
                {
                    foreach (DayControl item in month)
                    {
                        Grid parentItem = item.Content as Grid;
                        UIElementCollection buttons = parentItem.Children as UIElementCollection;
                        foreach (var elem in buttons)
                        {
                            Button button = elem as Button;
                            var buttonTextBlock = button.Content as TextBlock;
                            int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                            int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag);
                            string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                            if (buttonNameOfDay == "Рабочий" && date.Day == buttonDay && date.Month == buttonMonth)
                            {
                                _viewModel.WorkingDays.Add(true);
                            }
                            else
                            {
                                _viewModel.WorkingDays.Add(false);
                            }
                        }
                    }
                }
            }
            if (_viewModel.WorkingDays.Contains(true))
            {
                _viewModel.SelectedItem.Count -= _viewModel.CountSelectedDays;
                _viewModel.VacationsToAproval.Add(_viewModel.PlannedItem);

                _viewModel.DisplayedDateString = "";
                _viewModel.ClicksOnCalendar = 0;

                if (_viewModel.SelectedItem.Count == 0)
                {
                    for (int i = 0; i < _viewModel.VacationTypes.Count; i++)
                    {
                        if (_viewModel.VacationTypes[i].Count > 0)
                        {
                            _viewModel.SelectedItem = _viewModel.VacationTypes[i];
                            break;
                        }
                    }
                }

                _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));
                //TODO: исправить
                VacationsToAprovalClone = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));

                for (int i = VacationsToAprovalClone.Count - 1; i > 0; i--)
                {
                    if (VacationsToAprovalClone[i].Name == VacationsToAprovalClone[i - 1].Name)
                    {
                        if (VacationsToAprovalClone[i].Date_Start.AddDays(-1) == VacationsToAprovalClone[i - 1].Date_end)
                        {
                            VacationsToAprovalClone[i].Date_Start = VacationsToAprovalClone[i - 1].Date_Start;
                            int countDays = GetCountDays(VacationsToAprovalClone[i]);
                            VacationsToAprovalClone[i].Count = countDays;
                            VacationsToAprovalClone.Remove(VacationsToAprovalClone[i - 1]);
                            _sampleError.ErrorName.Text = "Несколько периодов объединены в один";
                            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", _viewModel.ExtendedClosingEventHandler);
                        }

                    }
                }
                _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAprovalClone.OrderBy(i => i.Date_Start));
                //}
            }
            else
            {
                _viewModel.ShowAlert("В выбранном периоде отсутствуют рабочие дни");
            }

        }

        private int GetCountDays(Vacation vacation)
        {
            Range<DateTime> range = _viewModel.ReturnRange(vacation);
            int count = 0;
            foreach (DateTime date in range.Step(x => x.AddDays(1)))
            {
                count++;
            }
            return count;
        }
    }
}
