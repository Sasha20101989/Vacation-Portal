using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private readonly PersonalView _personalView;
        private readonly FullCalendar _fullCalendar;

        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
            _personalView = new PersonalView();
            _fullCalendar = new FullCalendar();
        }

        public override void Execute(object parameter)
        {
            int remainder = _viewModel.SelectedItem.Count - _viewModel.CountSelectedDays;
            bool isMorePlanedDays = remainder >= 0;
            if (isMorePlanedDays)
            {
                if (!_viewModel.VacationsToAproval.Contains(_viewModel.PlannedItem))
                {
                    
                    _viewModel.SelectedItem.Count -= _viewModel.CountSelectedDays;
                    _viewModel.VacationsToAproval.Add(_viewModel.PlannedItem);

                        Range<DateTime> range = _viewModel.PlannedItem.Date_Start.To(_viewModel.PlannedItem.Date_end);
                        List<DateTime> dates = new List<DateTime>();
                        foreach (DateTime date in range.Step(x => x.AddDays(1)))
                        {
                        UniformGrid v = _fullCalendar.Content as UniformGrid;
                        UIElementCollection q = v.Children as UIElementCollection;
                        foreach (var value in q)
                        {
                           
                        }
                        dates.Add(date);
                        }

                   

                    
                    //                    foreach (Vacation date in dates)
                    //                    {
                    //                        if (date)
                    //                        {

                    //                        }
                    //                    }
                    //                    if (_viewModel.PlannedItem!=_viewModel.SelectedItem)
                    //                    {
                    //                        _viewModel.VacationsToAproval.Add(_viewModel.PlannedItem);
                    //                    }

                    //foreach (Vacation item1 in _viewModel.VacationsToAproval)
                    //{
                    //    cloneVacationsToAproval.Add(item1);
                    //}
                }
                //_personalView.ListView.Height = (_personalView.ListView.ActualHeight / _personalView.ListView.Items.Count) * 8.7;
            }   
        }
    }
}
