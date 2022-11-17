using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private readonly PersonalView _personalView;

        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
            _personalView = new PersonalView();
        }

        public override void Execute(object parameter)
        {
            int remainder = _viewModel.SelectedItem.Count - _viewModel.CountSelectedDays;
            bool isMorePlanedDays = remainder >= 0;
            if (isMorePlanedDays)
            {
                Range<DateTime> range =_viewModel.ReturnRange(_viewModel.PlannedItem);

                List<bool> isGoToNext = new List<bool>();

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
                    _viewModel.VacationsToAproval.OrderBy(i => i.Date_Start);
                }
            }
        }
    }
}
