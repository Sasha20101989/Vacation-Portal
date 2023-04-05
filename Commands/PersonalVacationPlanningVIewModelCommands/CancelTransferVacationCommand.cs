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
    public class CancelTransferVacationCommand : AsyncComandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public CancelTransferVacationCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public async override Task ExecuteAsync(object parameter)
        {
            Vacation cancelableTransfer = (Vacation) parameter;
            if(_viewModel.IsFlipped)
            {

            }
            _viewModel.IsFlipped = false;
            //if(cancelableTransfer == null)
            //{
            //    return;
            //}

            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            if(App.SelectedMode == WindowMode.Personal)
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                if(_viewModel.SelectedSubordinate == null)
                {
                    Subordinate subordinate = App.API.Person.Subordinates.FirstOrDefault(subordinate => subordinate.Id_SAP == cancelableTransfer.UserId);
                    _viewModel.SelectedSubordinate = subordinate;
                }

                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            }

            int index = VacationAllowances.IndexOf(VacationAllowances.FirstOrDefault(f => f.Vacation_Name == cancelableTransfer.Name));
            if(index == -1)
            {
                return;
            }
            if(App.SelectedMode == WindowMode.Personal)
            {
                if(App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity >= cancelableTransfer.Count)
                {
                    App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity -= cancelableTransfer.Count;
                    await _viewModel.UpdateVacationAllowance(cancelableTransfer.UserId, cancelableTransfer.VacationId, cancelableTransfer.DateStart.Year, App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity);
                    await App.VacationAPI.TransferVacationAsync(cancelableTransfer);
                } else
                {
                    _viewModel.ShowAlert("Для того, чтобы отменить перенос отпуска данного типа, вам необходимо иметь доступное количество дней отпуска, равное количеству дней, которые вы хотите отменить.");
                }
               
            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                if(_viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity >= cancelableTransfer.Count)
                {
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity -= cancelableTransfer.Count;
                    await _viewModel.UpdateVacationAllowance(cancelableTransfer.UserId, cancelableTransfer.VacationId, cancelableTransfer.DateStart.Year, _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity);
                    await App.VacationAPI.TransferVacationAsync(cancelableTransfer);
                } else
                {
                    _viewModel.ShowAlert("Для того, чтобы отменить перенос отпуска данного типа, сотруднику необходимо иметь доступное количество дней отпуска, равное количеству дней, которые вы хотите отменить.");
                }
            }
            
        }
    }
}
