using MaterialDesignThemes.Wpf;
using System;
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
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private Color? _primaryColor;
        private readonly MainWindowViewModel _viewModel;
        private readonly VacationSummary _vacationSummary;
        private Department _department;
        private readonly List<Department> _departmentsForPerson;
        private readonly List<Person> _personDescriptions = new List<Person>();
        private ICollectionView _menuItemsView;
        public string IsAdmin => _adminString;
        #endregion

        #region Commands implementation
        public ICommand ThemeChangeCommand { get; set; }
        public AnotherCommandImplementation HomeCommand { get; set; }
        public AnotherCommandImplementation MoveToSettingsCommand { get; set; }
        public AnotherCommandImplementation MovePrevPageCommand { get; set; }
        public AnotherCommandImplementation MoveNextPageCommand { get; set; }

        public AnotherCommandImplementation DismissAllNotificationsCommand { get; set; }
        public AnotherCommandImplementation AddNewNotificationCommand { get; set; }

        #endregion
        
        #region Actions
        public event Action<MenuItem> SelectedItemChanged;
        #endregion

        #region Properties

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

        private string _adminString;
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

        private void ChangeCustomColor(object obj)
        {
            var color = (Color)obj;

            if (ActiveScheme == ColorScheme.Primary)
            {

                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
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

        private VacationStore _vacationStore;
        public VacationStore VacationStore
        {
            get { return _vacationStore; }
            set
            {
                _vacationStore = value;
                OnPropertyChanged(nameof(VacationStore));
            }
        }
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

        private double _fontSize;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems {
            get => _menuItems;
            set
            {
                SetProperty(ref _menuItems, value);
                OnPropertyChanged(nameof(MenuItems));
            }
        }
        public ObservableCollection<MenuItem> MainMenuItems { get; set; }

        #endregion

        public MainWindowViewModel(VacationSummary vacationSummary)
        {
            ITheme theme = _paletteHelper.GetTheme();
            _primaryColor = theme.PrimaryMid.Color;
            SelectedColor = _primaryColor;

            _adminString = string.Empty;
            _fontSize = 16;
            _departmentsForPerson = new List<Department>();
            _vacationSummary = vacationSummary;
            _viewModel = this;
            MenuItems = new ObservableCollection<MenuItem>();
            MainMenuItems = new ObservableCollection<MenuItem>();

            CreateHomePage().Await();

            ThemeChangeCommand = new ThemeChangeCommand(_viewModel);
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

        private void OnSettingsLoad(Settings settings)
        {
            _isLoading = true;
            Color color = (Color)ColorConverter.ConvertFromString(settings.Color);
            SelectedColor = color;
            FontSize = settings.FontSize;
            _isLoading = false;
        }

        private void OnSelectedItemChanged(MenuItem selectedItem)
        {
            SelectedItemChanged?.Invoke(selectedItem);
        }

        private async Task CreateHomePage()
        {
            await Task.Delay(300);
            MenuItems = new ObservableCollection<MenuItem>(new[]
                {
                new MenuItem(
            HomePage,
            typeof(HomeView),
            selectedIcon: PackIconKind.Home,
            unselectedIcon: PackIconKind.HomeOutline)
            });
            CreateCommands().Await();
        }
        private async Task CreateOtherPages()
        {
            await Task.Delay(300);
            var persons = _vacationSummary.GetUser(Environment.UserName);
            if (persons != null)
            {
                _personDescriptions.AddRange(persons);

                if (_personDescriptions.Count != 0)
                {
                    bool isSupervisor = _personDescriptions[0].Is_Supervisor;
                    bool isHR = _personDescriptions[0].Is_HR;
                    var department = _vacationSummary.GetDepartmentForUser(_personDescriptions[0].Account);
                    if (department != null)
                    {//временное использование
                        _departmentsForPerson.AddRange(department);

                        _department = new Department(_departmentsForPerson[0].Name, _vacationSummary);
                        _vacationStore = new VacationStore(_department);

                        foreach (var item in GenerateMenuItems(_viewModel, _department, _vacationStore, _personDescriptions[0]).OrderBy(i => i.Name))
                        {
                            if (!MenuItems.Contains(item))
                            {
                                MenuItems.Add(item);
                            }
                        }

                        MenuItem personalItem = MenuItems.FirstOrDefault(x => x.Name == PersonalVacationPlanning);
                        CreateMainMenuItems(personalItem);

                        if (isSupervisor)
                        {
                            _adminString = "Аккаунт руководителя";
                            MenuItem supervisorItem = MenuItems.FirstOrDefault(x => x.Name == SupervisorPage);
                            CreateMainMenuItems(supervisorItem);
                        }
                        else if (isHR)
                        {
                            _adminString = "Аккаунт HR сотрудника";
                            MenuItem hRItem = MenuItems.FirstOrDefault(x => x.Name == SupervisorPage);
                            CreateMainMenuItems(hRItem);
                        }

                        _vacationStore.SettingsUILoad += OnSettingsLoad;
                        _vacationStore.LoadSettings();
                    }
                }
            }
            _menuItemsView = CollectionViewSource.GetDefaultView(MenuItems);
            _menuItemsView.Filter = MenuItemsFilter;
        }
        private async Task CreateCommands()
        {
            await Task.Delay(300);
            HomeCommand = new AnotherCommandImplementation(
            _ =>
            {
                SearchKeyword = string.Empty;
                SelectedIndex = 0;
            });

            MoveToSettingsCommand = new AnotherCommandImplementation(
                _ =>
                {
                    SearchKeyword = string.Empty;
                    SelectedItem = new MenuItem(
                    SettingsPage,
                    typeof(SettingsView),
                    selectedIcon: PackIconKind.Cog,
                    unselectedIcon: PackIconKind.CogOutline,
                    new SettingsViewModel(_viewModel));
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

            DismissAllNotificationsCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (SelectedIndex >= 0)
                    {
                        MenuItems.ElementAt(SelectedIndex).DismissAllNotifications();
                    }
                });//, _ => MenuItems[SelectedIndex].Notifications != null
            AddNewNotificationCommand = new AnotherCommandImplementation(
                _ =>
                {
                    MenuItems.ElementAt(SelectedIndex).AddNewNotification();
                });

            

            AddNewNotificationCommand.Execute(new object());

            CreateOtherPages().Await();
            // _menuItemsView.Refresh();
        }

        private ObservableCollection<MenuItem> CreateMainMenuItems(MenuItem menuItem)
        {
            MainMenuItems.Add(menuItem);
            return MainMenuItems;
        }

        private static IEnumerable<MenuItem> GenerateMenuItems(MainWindowViewModel viewModel,
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
            yield return new MenuItem(
            PersonalVacationPlanning,
            typeof(PersonalVacationPlanningView),
            selectedIcon: PackIconKind.BagPersonalTag,
            unselectedIcon: PackIconKind.BagPersonalTagOutline,
            new PersonalVacationPlanningViewModel(person));
        }
    }
}
