using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class CheckVacationsCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;
        private readonly SampleError _sampleError = new SampleError();
        private Vacation CheckedVacation { get; set; }
        public CheckVacationsCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.IsEnabled = false;
            bool is14DaysPlaned = false;
            bool is7DaysPlaned = false;
            _viewModel.VacationsToAproval = new ObservableCollection<Vacation>(_viewModel.VacationsToAproval.OrderByDescending(i => i.Count));
            for(int i = 0; i < _viewModel.VacationsToAproval.Count; i++)
            {
                int countFirstPeriod = 0;
                if(_viewModel.VacationsToAproval[i].Name == "Основной")
                {
                    Range<DateTime> range = _viewModel.ReturnRange(_viewModel.VacationsToAproval[i]);
                    foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                    {
                        countFirstPeriod++;
                        if(countFirstPeriod >= 14)
                        {
                            is14DaysPlaned = true;
                            CheckedVacation = _viewModel.VacationsToAproval[i];
                        }
                    }
                }
                if(is14DaysPlaned)
                {
                    break;
                }
            }
            if(is14DaysPlaned)
            {
                for(int i = 0; i < _viewModel.VacationsToAproval.Count; i++)
                {
                    int countSecondPeriod = 0;
                    if(_viewModel.VacationsToAproval[i].Name == "Основной" && _viewModel.VacationsToAproval[i] != CheckedVacation)
                    {
                        Range<DateTime> range = _viewModel.ReturnRange(_viewModel.VacationsToAproval[i]);
                        foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                        {
                            countSecondPeriod++;
                            if(countSecondPeriod >= 7)
                            {
                                is7DaysPlaned = true;
                            }
                        }
                        if(is7DaysPlaned)
                        {
                            _viewModel.ShowAlert("Успех");
                            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", _viewModel.ExtendedClosingEventHandler);
                            break;
                        } else
                        {
                            _viewModel.ShowAlert("Среди периодов планового основного отпуска, должен быть один длительностью не менее 7");
                            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", _viewModel.ExtendedClosingEventHandler);
                        }
                    }
                }
            } else
            {
                _viewModel.ShowAlert("Среди периодов планового основного отпуска, должен быть один длительностью не менее 14 дней");
                Task<object> result = DialogHost.Show(_sampleError, "RootDialog", _viewModel.ExtendedClosingEventHandler);
            }
            _viewModel.IsEnabled = true;
        }
    }
}
