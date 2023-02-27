using System;
using System.Windows.Media;
using System.Windows.Threading;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands {
    public class StartLearningCommand : CommandBase {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public StartLearningCommand(PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
        }
        public override bool CanExecute(object parameter) {
            if(_viewModel.IsEnabled) {
                _viewModel.IsEnabled = false;
                return true;
            }
            _viewModel.IsEnabled = true;
            return true;
        }
        public override void Execute(object parameter) {
            LearnVacation("Выберете отпуск который хотите запланировать.");
        }

        private void LearnVacation(string message) {

            DateTime started = DateTime.Now;

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) => {
                long totalDuration = started.AddMilliseconds(5000).Ticks - started.Ticks;
                long currentProgress = DateTime.Now.Ticks - started.Ticks;
                double currentProgressPercent = 100.0 / totalDuration * currentProgress;

                _viewModel.LearningProgress = currentProgressPercent;
                BrushConverter converter = new System.Windows.Media.BrushConverter();
                _viewModel.BorderColorVacation = (Brush) converter.ConvertFromString("#FF0000");
                _viewModel.MessageQueueVacation.Enqueue(message);

                if(_viewModel.LearningProgress >= 100) {
                    _viewModel.MessageQueueVacation.Clear();
                    _viewModel.BorderColorVacation = Brushes.Transparent;
                    _viewModel.LearningProgress = 0;

                    if(o is DispatcherTimer timer) {
                        timer.Stop();
                        if(message == "До момента, как вы подтвердите, вы можете изменить вид отпуска.") {
                            LernCalendar("Тогда выбранные дни сбросятся, здесь");
                        } else {
                            LernCalendar("Выберете кликом в календаре, первый и последний день промежутка, который хотите запланировать.");
                        }
                    }
                }
            }), Dispatcher.CurrentDispatcher);
        }

        private void LernCalendar(string message) {
            DateTime started = DateTime.Now;

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) => {
                long totalDuration = started.AddMilliseconds(5000).Ticks - started.Ticks;
                long currentProgress = DateTime.Now.Ticks - started.Ticks;
                double currentProgressPercent = 100.0 / totalDuration * currentProgress;

                _viewModel.LearningProgress = currentProgressPercent;
                BrushConverter converter = new System.Windows.Media.BrushConverter();
                _viewModel.BorderColorCalendar = (Brush) converter.ConvertFromString("#FF0000");
                _viewModel.MessageQueueCalendar.Enqueue(message);

                if(_viewModel.LearningProgress >= 100) {
                    _viewModel.MessageQueueCalendar.Clear();
                    _viewModel.BorderColorCalendar = Brushes.Transparent;
                    _viewModel.LearningProgress = 0;

                    if(o is DispatcherTimer timer) {
                        timer.Stop();
                        if(message == "Тогда выбранные дни сбросятся, здесь") {
                            LearnViewPlanedDays("И здесь");
                        } else {
                            LearnViewPlanedDays("Убедитесь в том что вид отпуска, даты и колличество совпадет с вашим выбором и подтвердите кнопкой.");
                        }
                    }
                }
            }), Dispatcher.CurrentDispatcher);
        }

        private void LearnViewPlanedDays(string message) {

            DateTime started = DateTime.Now;

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) => {
                long totalDuration = started.AddMilliseconds(5000).Ticks - started.Ticks;
                long currentProgress = DateTime.Now.Ticks - started.Ticks;
                double currentProgressPercent = 100.0 / totalDuration * currentProgress;

                _viewModel.LearningProgress = currentProgressPercent;
                BrushConverter converter = new System.Windows.Media.BrushConverter();
                _viewModel.BorderColorSelectedGap = (Brush) converter.ConvertFromString("#FF0000");
                _viewModel.MessageQueueSelectedGap.Enqueue(message);

                if(_viewModel.LearningProgress >= 100) {
                    _viewModel.MessageQueueSelectedGap.Clear();
                    _viewModel.BorderColorSelectedGap = Brushes.Transparent;
                    _viewModel.LearningProgress = 0;

                    if(o is DispatcherTimer timer) {
                        timer.Stop();
                        if(message != "И здесь") {
                            LearnVacation("До момента, как вы подтвердите, вы можете изменить вид отпуска.");
                        } else {
                            LernVIewPLanedVacations("После вашего подтверждения, выбранные в календаре дни добавляются в этот список. и остаток выбранного вида отпуска уменьшается.");
                        }
                    }
                }
            }), Dispatcher.CurrentDispatcher);
        }

        private void LernVIewPLanedVacations(string message) {

            DateTime started = DateTime.Now;

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) => {
                long totalDuration = started.AddMilliseconds(5000).Ticks - started.Ticks;
                long currentProgress = DateTime.Now.Ticks - started.Ticks;
                double currentProgressPercent = 100.0 / totalDuration * currentProgress;

                _viewModel.LearningProgress = currentProgressPercent;
                BrushConverter converter = new System.Windows.Media.BrushConverter();
                _viewModel.BorderColorPLanedVacations = (Brush) converter.ConvertFromString("#FF0000");
                _viewModel.MessageQueuePLanedVacations.Enqueue(message);

                if(_viewModel.LearningProgress >= 100) {
                    _viewModel.MessageQueuePLanedVacations.Clear();
                    _viewModel.BorderColorPLanedVacations = Brushes.Transparent;
                    _viewModel.LearningProgress = 0;

                    if(o is DispatcherTimer timer) {
                        timer.Stop();
                    }
                    _viewModel.IsEnabled = true;
                }
            }), Dispatcher.CurrentDispatcher);
        }
    }
}
