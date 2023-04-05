using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands {
    public class CancelVacationCommand : CommandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public CancelVacationCommand(PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter) {
            Vacation deletedVacation = (Vacation) parameter;
            if(deletedVacation == null)
            {
                return;
            }
            await RemoveVacationFromPersonAsync(deletedVacation);
            //App.API.PersonUpdated?.Invoke(null);
        }

        public async Task RemoveVacationFromPersonAsync(Vacation deletedVacation)
        {
            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            if(App.SelectedMode == WindowMode.Personal)
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                if(_viewModel.SelectedSubordinate == null)
                {
                    Subordinate subordinate = App.API.Person.Subordinates.FirstOrDefault(subordinate => subordinate.Id_SAP == deletedVacation.UserId);
                    _viewModel.SelectedSubordinate = subordinate;
                }
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

                if(App.SelectedMode == WindowMode.Personal)
                {
                    App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity += deletedVacation.Count;
                    await _viewModel.UpdateVacationAllowance(deletedVacation.UserId, deletedVacation.VacationId, deletedVacation.DateStart.Year, App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity);
                    App.API.Person.User_Vacations.Remove(deletedVacation);
                } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
                {
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity += deletedVacation.Count;
                    await _viewModel.UpdateVacationAllowance(deletedVacation.UserId, deletedVacation.VacationId, deletedVacation.DateStart.Year, _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity);
                    _viewModel.SelectedSubordinate.Subordinate_Vacations.Remove(deletedVacation);
                    foreach(Subordinate subordinate in App.API.Person.Subordinates)
                    {
                        if(subordinate.Id_SAP == deletedVacation.UserId)
                        {
                            subordinate.Subordinate_Vacations.Remove(deletedVacation);
                        }
                    }
                }

                _viewModel.PlannedIndex = 0;

                if(App.SelectedMode == WindowMode.Personal)
                {
                    _viewModel.UpdateDataForPerson();
                    await _viewModel.Calendar.UpdateColorAsync(App.API.Person.User_Vacations);
                } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
                {
                    await _viewModel.UpdateDataForSubordinateAsync();
                    await _viewModel.Calendar.UpdateColorAsync(_viewModel.SelectedSubordinate.Subordinate_Vacations);
                }
            }
        }
    }
}
