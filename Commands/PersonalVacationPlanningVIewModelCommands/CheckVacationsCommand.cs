
using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Extensions;
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
            bool isSupervisorView = false;
            bool isPersonalView = false;
            ObservableCollection<Vacation> VacationsToAproval = new ObservableCollection<Vacation>();
            if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate))
            {
                isSupervisorView = true;
                VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderByDescending(i => i.Count));
            } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
            {
                isPersonalView = true;
                VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAprovalForPerson.OrderByDescending(i => i.Count));
            }

            Task<object> openCheck = DialogHost.Show(_checkVacationView, "RootDialog", _viewModel.ExtendedClosingEventHandler);
            _viewModel.IsEnabled = false;
            bool isFirstCheckDaysPlaned = false;
            bool isSecondCheckDaysPlaned = false;
            
            for(int i = 0; i < VacationsToAproval.Count; i++)
            {
                int countFirstPeriod = 0;
                if(VacationsToAproval[i].Name == "Основной")
                {
                    Range<DateTime> range = _viewModel.Calendar.ReturnRange(VacationsToAproval[i]);
                    foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                    {
                        countFirstPeriod++;
                        if(countFirstPeriod >= 14)
                        {
                            isFirstCheckDaysPlaned = true;
                            CheckedVacation = VacationsToAproval[i];
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
                _checkVacationView.VisibilityButtonFirstCheck(isSupervisorView);
                if(isSecondCheckDaysPlaned)
                {
                    _checkVacationView.VisibilityButtonSecondCheck(isSupervisorView);
                } else
                {
                    for(int i = 0; i < VacationsToAproval.Count; i++)
                    {
                        int countSecondPeriod = 0;
                        if(VacationsToAproval[i].Name == "Основной" && VacationsToAproval[i] != CheckedVacation)
                        {
                            Range<DateTime> range = _viewModel.Calendar.ReturnRange(VacationsToAproval[i]);
                            foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                            {
                                countSecondPeriod++;
                                if(countSecondPeriod >= 7)
                                {
                                    isSecondCheckDaysPlaned = true;
                                }
                            }
                            if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate))
                            {
                                _checkVacationView.VisibilityExclamationButtonSecondCheck(isSupervisorView);
                                break;
                            }
                            if(isSecondCheckDaysPlaned)
                            {
                                _checkVacationView.VisibilityButtonSecondCheck(isSupervisorView);
                                break;
                            }
                            
                        }
                    }
                    if(isSupervisorView)
                    {
                        _checkVacationView.VisibilityExclamationButtonSecondCheck(isSupervisorView);
                    }
                }
            } else
            {
                _checkVacationView.NotVisibilityButtonFirstCheck(isSupervisorView);
            }
            _viewModel.IsEnabled = true;
        }
    }
}
