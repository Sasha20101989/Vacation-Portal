using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.ViewModels.ForPages;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.MVVM.Models
{
    public class Person
    {
        #region Pages
        private static readonly string _settingsPage = "Настройки приложения";
        private static readonly string _holidaysPage = "Настройки выходных и праздников";
        private static readonly string _supervisorPage = "Страница руководителя";
        private static readonly string _hrPage = "Страница HR сотрудника";
        //private static readonly string Табельщик = "Страница HR сотрудника";
        private static readonly string _personalVacationPlanning = "Страница персонального планирования отпуска";
        #endregion
        public int Id_SAP { get; set; }
        public string Id_Account { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Department_Id { get; set; }
        public int Vitrual_Department_Id { get; set; }
        public string Position { get; set; }
        public static bool Is_HR { get; set; }
        public static bool Is_Accounting { get; set; }
        public static bool Is_Supervisor { get; set; }
        public Settings Settings { get; set; }

        private readonly List<Settings> _listSettings = new List<Settings>();
        public ObservableCollection<VacationViewModel> Vacations { get; set; } = new ObservableCollection<VacationViewModel>();
        public event Action<Settings> SettingsLoad;
        //public event Action<List<VacationViewModel>> VacationsLoad;
        public event Action<ObservableCollection<MenuItem>> MenuItemsChanged;

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }

        public Person(int id_SAP, string id_Account, string name, string surname, string patronymic, int departmentId, int virtualDepartmentId, string position)
        {
            Id_SAP = id_SAP;
            Id_Account = id_Account;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Department_Id = departmentId;
            Vitrual_Department_Id = virtualDepartmentId;
            Position = position;
            //App.API.OnVacationsChanged += OnVacationsLoad;
        }
        //public async void GetVacations()
        //{
        //    IEnumerable<VacationViewModel> vacations = await App.API.LoadVacationAsync(Id_SAP);
        //    App.Current.Dispatcher.Invoke((Action) delegate
        //    {
        //        App.SplashScreen.status.Text = "Ищу ваши отпуска...";
        //        App.SplashScreen.status.Foreground = Brushes.Black;
        //    });
        //    OnVacationsLoad(vacations);
        //}
        //public void OnVacationsLoad(IEnumerable<VacationViewModel> vacations)
        //{
        //    foreach(VacationViewModel vacation in vacations)
        //    {
        //        Vacations.Add(vacation);
        //        App.API.Vacations.Add(vacation);
        //    }
        //    VacationsLoad?.Invoke(Vacations);
        //}
        public async IAsyncEnumerable<VacationViewModel> FetchVacationsAsync(int year)
        {
            IEnumerable<VacationViewModel> vacations = await App.API.LoadVacationAsync(App.API.Person.Id_SAP, year);

            foreach(var item in vacations)
            {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationAllowanceViewModel> FetchVacationAllowancesAsync(int year)
        {
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = await App.API.GetVacationAllowanceAsync(App.API.Person.Id_SAP, year);
            foreach(var item in vacationAllowances)
            {
                yield return item;
            }
        }
        public async void GetAccess()
        {
            IEnumerable<Access> access = await App.API.GetAccessAsync(Environment.UserName);
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Ищу настройки вашего аккаунта...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            OnAccessLoad(access);
        }
        private static void OnAccessLoad(IEnumerable<Access> access)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Приложение с доступом сотрудника";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            foreach(Access item in access)
            {
                if(item.Is_Accounting)
                {
                    Is_Accounting = true;
                    App.Current.Dispatcher.Invoke((Action) delegate
                    {
                        App.SplashScreen.status.Text = "Приложение с доступом табельщика";
                        App.SplashScreen.status.Foreground = Brushes.Black;
                    });
                }
                if(item.Is_Supervisor)
                {
                    Is_Supervisor = true;
                    App.Current.Dispatcher.Invoke((Action) delegate
                    {
                        App.SplashScreen.status.Text = "Приложение с доступом руководителя";
                        App.SplashScreen.status.Foreground = Brushes.Black;
                    });
                }
                if(item.Is_HR)
                {
                    Is_HR = true;
                    App.SplashScreen.status.Text = "Приложение с доступом HR сотрудника";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                }
            }
        }

        public async void GetSettings()
        {

            IEnumerable<Settings> settings = await App.API.GetSettingsAsync(Environment.UserName);
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Ищу настройки вашего аккаунта...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            OnSettingsLoad(settings);
        }
        private void OnSettingsLoad(IEnumerable<Settings> settings)
        {

            _listSettings.AddRange(settings);

            if(_listSettings.Count < 1)
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Настройки приложения не найдены";
                    App.SplashScreen.status.Foreground = Brushes.Orange;
                });
            } else
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Применяю настройки вашего аккаунта...";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
                Settings = _listSettings[0];
                SettingsLoad?.Invoke(Settings);
            }
        }

        public void AddPages(MainWindowViewModel _viewModel)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
             {

                 foreach(MenuItem menuItem in GenerateMenuItems())
                 {
                     if(!_viewModel.MenuItems.Contains(menuItem))
                     {

                         _viewModel.MenuItems.Add(menuItem);

                     }
                 }

                 _viewModel.MenuItems.Add(new MenuItem(_holidaysPage, typeof(HolidaysView), selectedIcon: PackIconKind.BoxCog, unselectedIcon: PackIconKind.BoxCogOutline, new HolidaysViewModel()));
                 _viewModel.MenuItems.Add(new MenuItem(_settingsPage, typeof(SettingsView), selectedIcon: PackIconKind.Cog, unselectedIcon: PackIconKind.CogOutline, new SettingsViewModel(_viewModel)));

                 MenuItem personalItem = _viewModel.MenuItems.FirstOrDefault(x => x.Name == _personalVacationPlanning);
                 _viewModel.MainMenuItems = CreateMainMenuItems(personalItem, _viewModel);

                 if(Is_Supervisor)
                 {
                     _viewModel.AdminString = "Аккаунт руководителя";
                     MenuItem supervisorItem = _viewModel.MenuItems.FirstOrDefault(x => x.Name == _supervisorPage);
                     _viewModel.MainMenuItems = CreateMainMenuItems(supervisorItem, _viewModel);
                 } else if(Is_HR)
                 {
                     _viewModel.AdminString = "Аккаунт HR сотрудника";
                     MenuItem hRItem = _viewModel.MenuItems.FirstOrDefault(x => x.Name == _hrPage);
                     _viewModel.MainMenuItems = CreateMainMenuItems(hRItem, _viewModel);
                 }
                 OnMenuItemsChanged(_viewModel.MenuItems);
             });
        }
        private void OnMenuItemsChanged(ObservableCollection<MenuItem> menuItems)
        {
            MenuItemsChanged?.Invoke(menuItems);
        }

        private ObservableCollection<MenuItem> CreateMainMenuItems(MenuItem menuItem, MainWindowViewModel _viewModel)
        {
            _viewModel.MainMenuItems.Add(menuItem);
            return _viewModel.MainMenuItems;
        }
        private IEnumerable<MenuItem> GenerateMenuItems()
        {
            yield return new MenuItem(
            _personalVacationPlanning,
            typeof(PersonalVacationPlanningView),
            selectedIcon: PackIconKind.BagPersonalTag,
            unselectedIcon: PackIconKind.BagPersonalTagOutline,
            new PersonalVacationPlanningViewModel());
        }
    }
}
