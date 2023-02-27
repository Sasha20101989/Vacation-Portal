
using MaterialDesignThemes.Wpf;
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
            ObservableCollection<Vacation> VacationsToAproval = new ObservableCollection<Vacation>();
            if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD))
            {
                isSupervisorView = true;
                VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.SelectedSubordinate.Subordinate_Vacations.OrderByDescending(i => i.Count));
            } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
            {
                VacationsToAproval = new ObservableCollection<Vacation>(App.API.Person.User_Vacations.OrderByDescending(i => i.Count));
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
                    foreach(DateTime planedDate in VacationsToAproval[i].DateRange)
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
                            foreach(DateTime planedDate in VacationsToAproval[i].DateRange)
                            {
                                countSecondPeriod++;
                                if(countSecondPeriod >= 7)
                                {
                                    isSecondCheckDaysPlaned = true;
                                    _checkVacationView.VisibilityButtonSecondCheck(isSupervisorView);
                                    _viewModel.IsEnabled = true;
                                    return;
                                }
                            }
                        }
                    }
                    if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD))
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
