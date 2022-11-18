using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.NotificationCommands;
using Vacation_Portal.Commands.ThemeInteractionCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.ForPages;
using Vacation_Portal.MVVM.Views;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Pages
        private static readonly string HomePage = "Главная страница";
        private static readonly string SettingsPage = "Настройки";
        private static readonly string SupervisorPage = "Страница руководителя";
        private static readonly string HRPage = "Страница HR сотрудника";
        private static readonly string Табельщик = "Страница HR сотрудника";
        private static readonly string PersonalVacationPlanning = "Страница персонального планирования отпуска";
        #endregion

        #region Addons
        private readonly PaletteHelper PaletteHelper = new PaletteHelper();
        private Color? PrimaryColor { get; set; }
        private readonly MainWindowViewModel _viewModel;
        private readonly VacationSummary _vacationSummary;
        private Department Department { get; set; }
        private List<Department> DepartmentsForPerson { get; set; } = new List<Department>();
        private List<Person> PersonDescriptions { get; set; } = new List<Person>();
        private ICollectionView _menuItemsView;
        #endregion

        #region Commands implementation
        public ICommand ThemeChangeCommand { get; set; }
        public AnotherCommandImplementation HomeCommand { get; set; }
        public AnotherCommandImplementation MoveToSettingsCommand { get; set; }
        public AnotherCommandImplementation MovePrevPageCommand { get; set; }
        public AnotherCommandImplementation MoveNextPageCommand { get; set; }

        public ICommand DismissAllNotificationsCommand { get; set; }
        public ICommand AddNewNotificationCommand { get; set; }

        #endregion

        #region Actions
        public event Action<MenuItem> SelectedItemChanged;
        #endregion

        #region Props

        #region Theme props
        private ColorScheme _activeScheme;
        public ColorScheme ActiveScheme
        {
            get => _activeScheme;
            set
            {
                if (_activeScheme != value)
                {
                    _activeScheme = value;
                    OnPropertyChanged();
                }
            }
        }

        private Color? _selectedColor;
        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged();

                    if (value is Color color)
                    {
                        ChangeCustomColor(color);
                    }
                }
            }
        }

        private bool _isCheckedTheme;
        public bool IsCheckedTheme
        {
            get
            {
                return _isCheckedTheme;
            }
            set
            {
                _isCheckedTheme = value;
                OnPropertyChanged(nameof(IsCheckedTheme));
            }
        }
        #endregion Theme props

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));

            }
        }

        private string _adminString = string.Empty;
        public string AdminString
        {
            get
            {
                return _adminString;
            }
            set
            {
                _adminString = value;
                OnPropertyChanged(nameof(AdminString));
            }
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                if (SetProperty(ref _searchKeyword, value))
                {
                    _menuItemsView.Refresh();
                }
                OnPropertyChanged(nameof(SearchKeyword));
            }
        }

        public VacationStore VacationStore { get; set; }
        private MenuItem _selectedItem;
        public MenuItem SelectedItem
        {
            get => _selectedItem;
            set
            {

                SetProperty(ref _selectedItem, value);
                if (_selectedItem != null)
                {
                    Title = _selectedItem.Name;
                }
                OnPropertyChanged(nameof(SelectedItem));
                OnSelectedItemChanged(SelectedItem);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();
        public ObservableCollection<MenuItem> MainMenuItems { get; set; } = new ObservableCollection<MenuItem>();

        #endregion

        public MainWindowViewModel(VacationSummary vacationSummary)
        {
            ITheme theme = PaletteHelper.GetTheme();
            PrimaryColor = theme.PrimaryMid.Color;
            SelectedColor = PrimaryColor;

            _vacationSummary = vacationSummary;
            _viewModel = this;

            MenuItems.Add(new MenuItem(HomePage, typeof(HomeView), selectedIcon: PackIconKind.Home, unselectedIcon: PackIconKind.HomeOutline));

            CreateOtherPages().Await();

            HomeCommand = new AnotherCommandImplementation(
            _ =>
            {
                SearchKeyword = string.Empty;
                SelectedIndex = 0;
            });

            MovePrevPageCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (!string.IsNullOrWhiteSpace(SearchKeyword))
                        SearchKeyword = string.Empty;

                    SelectedIndex--;
                },
                _ => SelectedIndex > 0);

            MoveNextPageCommand = new AnotherCommandImplementation(
               _ =>
               {
                   if (!string.IsNullOrWhiteSpace(SearchKeyword))
                       SearchKeyword = string.Empty;

                   SelectedIndex++;
               },
               _ => SelectedIndex < MenuItems.Count - 1);

            MoveToSettingsCommand = new AnotherCommandImplementation(
                _ =>
                {
                    SearchKeyword = string.Empty;
                    int index = 0;
                    for (int i = 0; i < MenuItems.Count; i++)
                    {
                        if (MenuItems[i].Name == SettingsPage)
                        {
                            index = i;
                            break;
                        }
                    }

                    SelectedIndex = index;
                });

            AddNewNotificationCommand = new AddNewNotificationCommand(_viewModel);
            DismissAllNotificationsCommand = new DismissAllNotificationsCommand(_viewModel);
            ThemeChangeCommand = new ThemeChangeCommand(_viewModel);

            AddNewNotificationCommand.Execute(new object());

            _menuItemsView = CollectionViewSource.GetDefaultView(MenuItems);
            _menuItemsView.Filter = MenuItemsFilter;

            
        }

        private void OnSettingsLoad(Settings settings)
        {
            _isLoading = true;
            Color color = (Color)ColorConverter.ConvertFromString(settings.Color);
            SelectedColor = color;
            _isLoading = false;
        }

        private void OnSelectedItemChanged(MenuItem selectedItem)
        {
            SelectedItemChanged?.Invoke(selectedItem);
        }

        private async Task CreateOtherPages()
        {
            IEnumerable<Person> persons = await _vacationSummary.GetUser(Environment.UserName);
            IEnumerable<Department> department = await _vacationSummary.GetDepartmentForUser(Environment.UserName);
            if (persons != null && department != null)
            {
                PersonDescriptions.AddRange(persons);
                DepartmentsForPerson.AddRange(department);

                Department = new Department(DepartmentsForPerson[0].Name, _vacationSummary);
                VacationStore = new VacationStore(Department);
                VacationStore.SettingsUILoad += OnSettingsLoad;
                VacationStore.LoadSettings();

                foreach (Task <MenuItem> task in GenerateMenuItems(_viewModel, Department, VacationStore, PersonDescriptions[0]))
                {
                    MenuItem menuItem = await task;
                    if (!MenuItems.Contains(menuItem))
                    {
                        MenuItems.Add(menuItem);
                    }
                }

                MenuItems.Add(new MenuItem(SettingsPage, typeof(SettingsView), selectedIcon: PackIconKind.Cog, unselectedIcon: PackIconKind.CogOutline, new SettingsViewModel(_viewModel)));

                MenuItem personalItem = MenuItems.FirstOrDefault(x => x.Name == PersonalVacationPlanning);
                MainMenuItems = CreateMainMenuItems(personalItem);

                if (PersonDescriptions[0].Is_Supervisor)
                {
                    AdminString = "Аккаунт руководителя";
                    MenuItem supervisorItem = MenuItems.FirstOrDefault(x => x.Name == SupervisorPage);
                    MainMenuItems = CreateMainMenuItems(supervisorItem);
                }
                else if (PersonDescriptions[0].Is_HR)
                {
                    AdminString = "Аккаунт HR сотрудника";
                    MenuItem hRItem = MenuItems.FirstOrDefault(x => x.Name == SupervisorPage);
                    MainMenuItems = CreateMainMenuItems(hRItem);
                }
            }

        }

        private IEnumerable<Task<MenuItem>> GenerateMenuItems(MainWindowViewModel viewModel, Department department, VacationStore vacationStore, Person person)
        {
            yield return CreateMainItems(person);
        }

        private async Task<MenuItem> CreateMainItems(Person person)
        {
            await Task.Delay(100);

            MenuItem menuItem = new MenuItem(
            PersonalVacationPlanning,
            typeof(PersonalVacationPlanningView),
            selectedIcon: PackIconKind.BagPersonalTag,
            unselectedIcon: PackIconKind.BagPersonalTagOutline,
            new PersonalVacationPlanningViewModel(person));

            return menuItem;
        }

        private IEnumerable<Task<MenuItem>> GenerateMenuItemsAsync(MainWindowViewModel viewModel,
                                                               Department department,
                                                               VacationStore vacationStore,
                                                               Person person)
        {
            //это табельщик, может ставить отпуска всему подразделению
            //if (isHRTabelAdmin)
            //{
            //    yield return new MenuItem(
            //    HRPage,
            //    typeof(HRView),
            //    selectedIcon: PackIconKind.AccountsGroup,
            //    unselectedIcon: PackIconKind.AccountsGroupOutline);
            //}

            //это окно администратора, с раздачей прав
            //if (isHRAdmin)
            //{
            //    yield return new MenuItem(
            //    HRPage,
            //    typeof(HRView),
            //    selectedIcon: PackIconKind.AccountsGroup,
            //    unselectedIcon: PackIconKind.AccountsGroupOutline);
            //}


            /*
             async Task<Foo> DoSomethingAsync(string url)
             {
                 ...
             }       
             // producing IAsyncEnumerable<T>
             async IAsyncEnumerable<Foo> DownLoadAllURL(string[] strs)
             {
                 foreach (string url in strs)
                 {
                     yield return await DoSomethingAsync(url);
                 }
             }
             ...
             // using
             await foreach (Foo foo in DownLoadAllURL(new string[] { "url1", "url2" }))
             {
                 Use(foo);
             }
             We can achieve the same behavior at C# 5 but with a different semantics:
             
             async Task<Foo> DoSomethingAsync(string url)
             {
                 ...
             }
             IEnumerable<Task<Foo>> DownLoadAllURL(string[] strs)
             {
                 foreach (string url in strs)
                 {
                     yield return DoSomethingAsync(url);
                 }
             }
             
             // using
             foreach (Task<Foo> task in DownLoadAllURL(new string[] { "url1", "url2" }))
             {
                 Foo foo = await task;
                 Use(foo);
             }
            */
            //await new MenuItem(
            //PersonalVacationPlanning,
            //typeof(PersonalVacationPlanningView),
            //selectedIcon: PackIconKind.BagPersonalTag,
            //unselectedIcon: PackIconKind.BagPersonalTagOutline,
            //new PersonalVacationPlanningViewModel(person));


            yield return CreateMainMenuItems(person);
        }

        private async Task<MenuItem> CreateMainMenuItems(Person person)
        {
            await Task.Delay(100);

            MenuItem menuItem = new MenuItem(
            PersonalVacationPlanning,
            typeof(PersonalVacationPlanningView),
            selectedIcon: PackIconKind.BagPersonalTag,
            unselectedIcon: PackIconKind.BagPersonalTagOutline,
            new PersonalVacationPlanningViewModel(person));

            return menuItem;
        }

        #region Utils
        private ObservableCollection<MenuItem> CreateMainMenuItems(MenuItem menuItem)
        {
            MainMenuItems.Add(menuItem);
            return MainMenuItems;
        }

        private void ChangeCustomColor(object obj)
        {
            var color = (Color)obj;

            if (ActiveScheme == ColorScheme.Primary)
            {

                PaletteHelper.ChangePrimaryColor(color);
                PrimaryColor = color;
            }
        }

        private bool MenuItemsFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(_searchKeyword))
            {
                return true;
            }

            return obj is MenuItem item
                   && item.Name.ToLower().Contains(_searchKeyword.ToLower());
        }
        #endregion
    }
}
