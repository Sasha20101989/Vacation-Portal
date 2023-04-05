using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class OpenTransferWindowCommand : AsyncComandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public OpenTransferWindowCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public async override Task ExecuteAsync(object parameter)
        {
            
            Vacation transferedVacation = (Vacation) parameter;
            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            ObservableCollection<Vacation> TransferedVacations = new ObservableCollection<Vacation>();

            if(transferedVacation == null)
            {
                return;
            }
            _viewModel.IsFlipped = true;
            transferedVacation.VacationStatusId = (int) Statuses.ForTranfser;
            //TODO: свитчить во время сохранения первым делом
           // await _viewModel.TransferVacation(transferedVacation);

            if(App.SelectedMode == WindowMode.Personal)
            {
                _viewModel.VacationsTransferedForPerson = new ObservableCollection<Vacation>(_viewModel.VacationsToAprovalForPerson.Where(v => v.Name == transferedVacation.Name));
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                _viewModel.VacationsTransferedForSubordinate = new ObservableCollection<Vacation>(_viewModel.VacationsToAprovalForSubordinate.Where(v => v.Name == transferedVacation.Name));
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            }

            int index = 0;
            for(int i = 0; i < VacationAllowances.Count; i++)
            {
                if(VacationAllowances[i].Vacation_Name == transferedVacation.Name)
                {
                    index = i;
                    break;
                }
            }

            if(App.SelectedMode == WindowMode.Personal)
            {
                App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity += transferedVacation.Count;
                //TODO: обновлять во время сохранения вторым делом
                //await _viewModel.UpdateVacationAllowance(transferedVacation.UserId, transferedVacation.VacationId, transferedVacation.DateStart.Year, App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity);

            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity += transferedVacation.Count;
                //TODO: обновлять во время сохранения вторым делом
                //await _viewModel.UpdateVacationAllowance(transferedVacation.UserId, transferedVacation.VacationId, transferedVacation.DateStart.Year, _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity);
            }
            _viewModel.SelectedItemAllowance = VacationAllowances[index];
            _viewModel.PlannedIndex = 0;
        }
    }
}
