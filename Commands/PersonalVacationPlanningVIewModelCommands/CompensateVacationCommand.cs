using MaterialDesignThemes.Wpf;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands {
    public class CompensateVacationCommand : CommandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private PrintPreView _viewer = new PrintPreView();

        public CompensateVacationCommand(PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter) {
            PrintPreViewModel printPreViewModel = null;

            if(parameter is Vacation vacationToCompensate) {
                if(App.SelectedMode == WindowMode.Personal) {
                    printPreViewModel = new PrintPreViewModel(App.API.Person.FullName,
                                                              App.API.Person.Position,
                                                              App.API.Person.Department_Name,
                                                              App.API.Person.Id_SAP,
                                                              vacationToCompensate);
                } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                    
                    if(_viewModel.SelectedSubordinate == null)
                    {
                        Subordinate subordinate = App.API.Person.Subordinates.FirstOrDefault(subordinate => subordinate.Id_SAP == vacationToCompensate.UserId);
                        _viewModel.SelectedSubordinate = subordinate;
                    }
                    printPreViewModel = new PrintPreViewModel(_viewModel.SelectedSubordinate.FullName,
                                                              _viewModel.SelectedSubordinate.Position,
                                                              _viewModel.SelectedSubordinate.Department_Name,
                                                              _viewModel.SelectedSubordinate.Id_SAP,
                                                              vacationToCompensate);
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
