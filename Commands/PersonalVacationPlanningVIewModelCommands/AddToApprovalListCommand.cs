using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MiscUtil.Collections;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private string NameVacation { get; set; }
        private DateTime FirstDate { get; set; }
        private DateTime SecondDate { get; set; }
        private int CountDays { get; set; }
        public ObservableCollection<Vacation> VacationsToAprovalClone { get; set; } = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToRemove { get; set; } = new ObservableCollection<Vacation>();

        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            int remainder = _viewModel.SelectedItem.Count - _viewModel.CountSelectedDays;
            VacationsToAprovalClone.Clear();
            VacationsToRemove.Clear();
            bool isMorePlanedDays = remainder >= 0;
            if (isMorePlanedDays)
            {
                Range<DateTime> range = _viewModel.ReturnRange(_viewModel.PlannedItem);

                List<bool> isGoToNext = new List<bool>();
                FirstDate = new DateTime();
                SecondDate = new DateTime();
                foreach (DateTime planedDate in range.Step(x => x.AddDays(1)))
                {
                    for (int i = 0; i < _viewModel.VacationsToAproval.Count; i++)
                    {
                        Vacation existingVacation = _viewModel.VacationsToAproval[i];

                        Range<DateTime> rangeExistingVacation = _viewModel.ReturnRange(existingVacation); ;

                        foreach (DateTime existingDate in rangeExistingVacation.Step(x => x.AddDays(1)))
                        {
                            if (existingDate == planedDate)
                            {
                                isGoToNext.Add(false);
                            }
                        }
                        if (isGoToNext.Contains(false))
                        {
                            break;
                        }
                    }
                }

                if (!isGoToNext.Contains(false))
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
                }
                else
                {
                    MessageBox.Show("Залетел в ошибку");
                }
                _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));
                VacationsToAprovalClone = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderBy(i => i.Date_Start));

                for (int i = VacationsToAprovalClone.Count - 1; i > 0; i--)
                {
                    //начинаем с последнего элемента
                    //если дата начала текущего элемента минус 1 день, равна дате конца предыдущего элемена
                    //то дата начала текущего элемента равна дате начала предыдущего лемента
                    //удалить предыдущий элемент
                    if (VacationsToAprovalClone[i].Date_Start.AddDays(-1) == VacationsToAprovalClone[i-1].Date_end)
                    {
                        
                        VacationsToAprovalClone[i].Date_Start = VacationsToAprovalClone[i - 1].Date_Start;
                        CountDays = GetCountDays(VacationsToAprovalClone[i]);
                        VacationsToAprovalClone[i].Count = CountDays;
                        VacationsToAprovalClone.Remove(VacationsToAprovalClone[i - 1]);
                        
                    }
                }
                _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAprovalClone.OrderBy(i => i.Date_Start));
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
