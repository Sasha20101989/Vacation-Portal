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
        private static readonly string _hrAdminPage = "Настройки календаря";
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
        public bool Is_HR { get; set; }
        public bool Is_Accounting { get; set; }
        public bool Is_Supervisor { get; set; }
        public Settings Settings { get; set; }
        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public ObservableCollection<Subordinate> Subordinates { get; set; } = new ObservableCollection<Subordinate>();

        public event Action<ObservableCollection<MenuItem>> MenuItemsChanged;

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
        }
        //public async IAsyncEnumerable<Access> FetchSubordinatesAsync()
        //{
        //    App.Current.Dispatcher.Invoke((Action) delegate
        //    {
        //        App.SplashScreen.status.Text = "Загружаю список подчинённых...";
        //        App.SplashScreen.status.Foreground = Brushes.Black;
        //    });
        //    IEnumerable<Subordinate> subordinates = await App.API.GetSubordinateAsync(Id_SAP);

        //    foreach(var item in subordinates)
        //    {
        //        yield return item;
        //    }
        //}
        public async IAsyncEnumerable<Access> FetchAccessAsync()
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Проверяю разрешения вашего аккаунта...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            IEnumerable<Access> access = await App.API.GetAccessAsync(Environment.UserName);

            foreach(Access item in access)
            {
                yield return item;
            }
        }
        public async IAsyncEnumerable<Settings> FetchSettingsAsync()
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Ищу настройки вашего аккаунта...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });

            IEnumerable<Settings> settings = await App.API.GetSettingsAsync(Environment.UserName);

            foreach(Settings item in settings)
            {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationViewModel> FetchVacationsAsync(int year)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Загружаю ваши отпуска...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            IEnumerable<VacationViewModel> vacations = await App.API.LoadVacationAsync(App.API.Person.Id_SAP, year);

            foreach(VacationViewModel item in vacations)
            {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationAllowanceViewModel> FetchVacationAllowancesAsync(int year)
        {
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = await App.API.GetVacationAllowanceAsync(App.API.Person.Id_SAP, year);
            foreach(VacationAllowanceViewModel item in vacationAllowances)
            {
                yield return item;
            }
        }

        public void AddPages(MainWindowViewModel _viewModel)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
             {
                 App.SplashScreen.status.Text = "Добавляю доступные вам страницы...";
                 App.SplashScreen.status.Foreground = Brushes.Black;
                 foreach(MenuItem menuItem in GenerateMenuItems(_viewModel))
                 {
                     if(!_viewModel.MenuItems.Contains(menuItem))
                     {
                         _viewModel.MenuItems.Add(menuItem);
                     }
                 }

                 MenuItem personalItem = _viewModel.MenuItems.FirstOrDefault(x => x.Name == _personalVacationPlanning);
                 _viewModel.MainMenuItems = CreateMainMenuItems(personalItem, _viewModel);

                 if(Is_Supervisor)
                 {
                     // _viewModel.MenuItems.Add(new MenuItem(_supervisorPage, typeof(VacationPlanningForSubordinatesView), selectedIcon: PackIconKind.AccountTie, unselectedIcon: PackIconKind.AccountTieOutline, new PersonalVacationPlanningViewModel()));
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
        private ObservableCollection<MenuItem> CreateMainMenuItems(MenuItem menuItem, MainWindowViewModel viewModel)
        {
            viewModel.MainMenuItems.Add(menuItem);
            return viewModel.MainMenuItems;
        }
        private IEnumerable<MenuItem> GenerateMenuItems(MainWindowViewModel viewModel)
        {
            yield return new MenuItem(
            _personalVacationPlanning,
            typeof(PersonalVacationPlanningView),
            selectedIcon: PackIconKind.BagPersonalTag,
            unselectedIcon: PackIconKind.BagPersonalTagOutline,
            new PersonalVacationPlanningViewModel());

            if(Is_Supervisor)
            {
                yield return new MenuItem(
                _supervisorPage,
                typeof(VacationPlanningForSubordinatesView),
                selectedIcon: PackIconKind.AccountTie,
                unselectedIcon: PackIconKind.AccountTieOutline,
                new PersonalVacationPlanningViewModel());
            }

            if(Is_HR)
            {
                yield return new MenuItem(
                _holidaysPage,
                typeof(HolidaysView),
                selectedIcon: PackIconKind.BoxCog,
                unselectedIcon: PackIconKind.BoxCogOutline,
                new HolidaysViewModel());

                yield return new MenuItem(
                _hrAdminPage,
                typeof(AdminPageHrView),
                selectedIcon: PackIconKind.BoxCog,
                unselectedIcon: PackIconKind.BoxCogOutline,
                new AdminPageHrViewModel());
            }

            yield return new MenuItem(
            _settingsPage,
            typeof(SettingsView),
            selectedIcon: PackIconKind.Cog,
            unselectedIcon: PackIconKind.CogOutline,
            new SettingsViewModel(viewModel));
        }
    }
}
