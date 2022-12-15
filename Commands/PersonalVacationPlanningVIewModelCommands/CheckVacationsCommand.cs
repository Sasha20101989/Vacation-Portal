
using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class CheckVacationsCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private readonly CheckVacationView _checkVacationView = new CheckVacationView();
        private Vacation CheckedVacation { get; set; }
        public CheckVacationsCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public void ChangeStatus14Days()
        {

        }
        public override void Execute(object parameter)
        {
            _checkVacationView.DataContext = _viewModel;
            _checkVacationView.ClearVisibility();

            Task<object> openCheck = DialogHost.Show(_checkVacationView, "RootDialog", _viewModel.ExtendedClosingEventHandler);
            _viewModel.IsEnabled = false;
            bool isFirstCheckDaysPlaned = false;
            bool isSecondCheckDaysPlaned = false;
            _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderByDescending(i => i.Count));
            for(int i = 0; i < _viewModel.VacationsToAproval.Count; i++)
            {
                int countFirstPeriod = 0;
                if(_viewModel.VacationsToAproval[i].Name == "Основной")
                {
                    Range<DateTime> range = _viewModel.Calendar.ReturnRange(_viewModel.VacationsToAproval[i]);
                    foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                    {
                        countFirstPeriod++;
                        if(countFirstPeriod >= 14)
                        {
                            isFirstCheckDaysPlaned = true;
                            CheckedVacation = _viewModel.VacationsToAproval[i];
                        }
                        if(countFirstPeriod >= 21 && countFirstPeriod % 7 >= 0)
                        {
                            isSecondCheckDaysPlaned = true;
                        }
                    }
                }
                if(isFirstCheckDaysPlaned)
                {
                    break;
                }
            }
            if(isFirstCheckDaysPlaned)
            {
                _checkVacationView.VisibilityButtonFirstCheck();
                if(isSecondCheckDaysPlaned)
                {
                    _checkVacationView.VisibilityButtonSecondCheck();
                } else
                {
                    for(int i = 0; i < _viewModel.VacationsToAproval.Count; i++)
                    {
                        int countSecondPeriod = 0;
                        if(_viewModel.VacationsToAproval[i].Name == "Основной" && _viewModel.VacationsToAproval[i] != CheckedVacation)
                        {
                            Range<DateTime> range = _viewModel.Calendar.ReturnRange(_viewModel.VacationsToAproval[i]);
                            foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                            {
                                countSecondPeriod++;
                                if(countSecondPeriod >= 7)
                                {
                                    isSecondCheckDaysPlaned = true;
                                }
                            }
                            if(isSecondCheckDaysPlaned)
                            {
                                _checkVacationView.VisibilityButtonSecondCheck();
                                break;
                            } else
                            {
                                _checkVacationView.NotVisibilityButtonSecondCheck();
                            }
                        }
                    }
                }
            } else
            {
                _checkVacationView.NotVisibilityButtonFirstCheck();
            }
            _viewModel.IsEnabled = true;
        }
    }
}
