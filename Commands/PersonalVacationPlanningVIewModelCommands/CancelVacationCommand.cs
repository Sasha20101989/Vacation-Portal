using System.Collections.ObjectModel;
using System.Linq;
using Vacation_Portal.Commands.BaseCommands;
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
            Vacation deletedItem = (Vacation) parameter;
            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            if(_viewModel.SelectedPerson == null)
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.VacationAllowancesForPerson.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.VacationAllowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            }
            if(deletedItem != null)
            {
                int index = 0;
                for(int i = 0; i < VacationAllowances.Count; i++)
                {
                    if(VacationAllowances[i].Vacation_Name == deletedItem.Name)
                    {
                        index = i;
                        break;
                    }
                }

                await _viewModel.DeleteVacation(deletedItem);

                if(_viewModel.SelectedPerson == null)
                {
                    _viewModel.VacationAllowancesForPerson[index].Vacation_Days_Quantity += deletedItem.Count;
                    await _viewModel.UpdateVacationAllowance(deletedItem.User_Id_SAP, deletedItem.Vacation_Id, deletedItem.Date_Start.Year, _viewModel.VacationAllowancesForPerson[index].Vacation_Days_Quantity);
                    _viewModel.VacationsToAprovalForPerson.Remove(deletedItem);
                } else
                {
                    _viewModel.VacationAllowances[index].Vacation_Days_Quantity += deletedItem.Count;
                    await _viewModel.UpdateVacationAllowance(deletedItem.User_Id_SAP, deletedItem.Vacation_Id, deletedItem.Date_Start.Year, _viewModel.VacationAllowances[index].Vacation_Days_Quantity);
                    _viewModel.VacationsToAproval.Remove(deletedItem);
                }
                
                //_viewModel.VacationsToAprovalFromDataBase.Remove(deletedItem);
                _viewModel.PlannedIndex = 0;
                _viewModel.Calendar.UpdateColor();
            }
        }
    }
}
