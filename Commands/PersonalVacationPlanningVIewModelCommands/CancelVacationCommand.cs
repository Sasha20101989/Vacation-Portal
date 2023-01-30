using System.Collections.ObjectModel;
using System.Linq;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class CancelVacationCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public CancelVacationCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            Vacation deletedVacation = (Vacation) parameter;
            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            if(_viewModel.SelectedSubordinate == null)
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            }
            if(deletedVacation != null)
            {
                int index = 0;
                for(int i = 0; i < VacationAllowances.Count; i++)
                {
                    if(VacationAllowances[i].Vacation_Name == deletedVacation.Name)
                    {
                        index = i;
                        break;
                    }
                }

                await _viewModel.DeleteVacation(deletedVacation);

                if(_viewModel.SelectedSubordinate == null)
                {
                    App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity += deletedVacation.Count;
                    await _viewModel.UpdateVacationAllowance(deletedVacation.User_Id_SAP, deletedVacation.Vacation_Id, deletedVacation.Date_Start.Year, App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity);
                    App.API.Person.User_Vacations.Remove(deletedVacation);
                } else
                {
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity += deletedVacation.Count;
                    await _viewModel.UpdateVacationAllowance(deletedVacation.User_Id_SAP, deletedVacation.Vacation_Id, deletedVacation.Date_Start.Year, _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity);
                    _viewModel.SelectedSubordinate.Subordinate_Vacations.Remove(deletedVacation);
                    foreach(Subordinate subordinate in App.API.Person.Subordinates)
                    {
                        if(subordinate.Id_SAP == deletedVacation.User_Id_SAP)
                        {
                            subordinate.Subordinate_Vacations.Remove(deletedVacation);
                        }
                    }
                }

                _viewModel.PlannedIndex = 0;
                if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate))
                {
                    _viewModel.Calendar.UpdateColor(_viewModel.SelectedSubordinate.Subordinate_Vacations);
                } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
                {
                    _viewModel.Calendar.UpdateColor(App.API.Person.User_Vacations);
                }
            }
        }
    }
}
