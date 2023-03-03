using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands {
    public class ShiftVacationsCommand : CommandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private readonly TransferPrintPreView _viewer = new TransferPrintPreView();

        public ShiftVacationsCommand(PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter) {
            PrintPreViewModel printPreViewModel = null;
             
            IEnumerable<Vacation> allAvailableToShiftVacations = null;

            if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal)) {
                allAvailableToShiftVacations = App.API.Person.User_Vacations.Where(vac => vac.Date_end > System.DateTime.Today && vac.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.Approved)).Take(5).ToList();

                if(allAvailableToShiftVacations.Any()) {
                    printPreViewModel = new PrintPreViewModel(App.API.Person.FullName,
                                                                App.API.Person.Position,
                                                                App.API.Person.Department_Name,
                                                                App.API.Person.Id_SAP,
                                                                allAvailableToShiftVacations);
                }

            } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD)) {
                allAvailableToShiftVacations = _viewModel.SelectedSubordinate.Subordinate_Vacations.Where(vac => vac.Date_end > System.DateTime.Today && vac.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.Approved)).Take(5).ToList();

                if(allAvailableToShiftVacations.Any()) {
                    printPreViewModel = new PrintPreViewModel(_viewModel.SelectedSubordinate.FullName,
                                                              _viewModel.SelectedSubordinate.Position,
                                                              _viewModel.SelectedSubordinate.Department_Name,
                                                              _viewModel.SelectedSubordinate.Id_SAP,
                                                              allAvailableToShiftVacations);
                }

            }


            if(printPreViewModel == null) {
                return;
            }
            _viewer.MyDocument.DataContext = printPreViewModel;
            Task<object> openCheck = DialogHost.Show(_viewer, "RootDialog", _viewModel.ExtendedClosingEventHandler);

        }
    }
}
