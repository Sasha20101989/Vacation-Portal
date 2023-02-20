using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class CompensateVacationCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private PrintPreView Viewer = null;

        public CompensateVacationCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            PrintPreViewModel printPreViewModel = null;

            if (parameter is Vacation vacationToCompensate)
            {
                if (App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
                {
                    printPreViewModel = new PrintPreViewModel(App.API.Person.FullName,
                                                              App.API.Person.Position,
                                                              App.API.Person.Virtual_Department_Name,
                                                              App.API.Person.Id_SAP,
                                                              vacationToCompensate);
                }
                else if (App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD))
                {
                    printPreViewModel = new PrintPreViewModel(_viewModel.SelectedSubordinate.FullName,
                                                              _viewModel.SelectedSubordinate.Position,
                                                              _viewModel.SelectedSubordinate.Virtual_Department_Name,
                                                              _viewModel.SelectedSubordinate.Id_SAP,
                                                              vacationToCompensate);
                }
            }

            if (printPreViewModel == null) return;

            Viewer?.Close();
            Viewer = new PrintPreView { DataContext = printPreViewModel };
            Viewer.Show();

        }
    }
}
