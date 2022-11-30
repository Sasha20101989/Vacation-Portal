using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        //private static readonly string HRPage = "Страница HR сотрудника";
        //private static readonly string Табельщик = "Страница HR сотрудника";
        private static readonly string _personalVacationPlanning = "Страница персонального планирования отпуска";
        #endregion

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Account { get; set; }
        public int Department_Id { get; set; }
        public bool Is_Supervisor { get; set; }
        public bool Is_HR { get; set; }
        public Settings Settings { get; set; }

        //private readonly List<Settings> ListSettings = new List<Settings>();

        public event Action<Settings> SettingsLoad;
        public event Action<ObservableCollection<MenuItem>> MenuItemsChanged;

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }

        public Person(string name, string surname, string patronymic, string account, int departmentId, bool isSupervisor, bool isHR)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Account = account;
            Department_Id = departmentId;
            Is_Supervisor = isSupervisor;
            Is_HR = isHR;
        }

        public async void GetSettings()
        {
            //IEnumerable<Settings> settings = await App.API.GetSettingsAsync(Environment.UserName);
            //OnSettingsLoad(settings);
        }
        //private void OnSettingsLoad(IEnumerable<Settings> settings)
        //{
        //    ListSettings.AddRange(settings);
        //    Settings = ListSettings[0];
        //    SettingsLoad?.Invoke(Settings);
        //}

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

                 _viewModel.MenuItems.Add(new MenuItem(_holidaysPage, typeof(HolidaysView), selectedIcon: PackIconKind.BoxCog, unselectedIcon: PackIconKind.BoxCogOutline, new HolidaysViewModel())); _viewModel.MenuItems.Add(new MenuItem(_settingsPage, typeof(SettingsView), selectedIcon: PackIconKind.Cog, unselectedIcon: PackIconKind.CogOutline, new SettingsViewModel(_viewModel)));
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
                     MenuItem hRItem = _viewModel.MenuItems.FirstOrDefault(x => x.Name == _supervisorPage);
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
