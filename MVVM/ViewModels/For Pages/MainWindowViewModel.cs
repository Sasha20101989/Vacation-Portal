using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.NotificationCommands;
using Vacation_Portal.Commands.ThemeInteractionCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.ForPages;
using Vacation_Portal.MVVM.Views;
using Vacation_Portal.MVVM.Views.Controls;
using Vacation_Portal.Services.Providers;

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
        private readonly HomeViewModel HomeViewModel = new HomeViewModel();
        private Department Department { get; set; }
        private List<Department> DepartmentsForPerson { get; set; } = new List<Department>();
        private List<Person> PersonDescriptions { get; set; } = new List<Person>();
        public ICollectionView MenuItemsView;
        #endregion

        #region Commands implementation
        public ICommand ThemeChangeCommand { get; set; }
        public AnotherCommandImplementation HomeCommand { get; set; }
        public AnotherCommandImplementation MoveToSettingsCommand { get; set; }
        public AnotherCommandImplementation MovePrevPageCommand { get; set; }
        public AnotherCommandImplementation MoveNextPageCommand { get; set; }
        public ICommand LoginCommand { get; }
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
                    MenuItemsView.Refresh();
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

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }
        private ObservableCollection<MenuItem> _mainMenuItems;
        public ObservableCollection<MenuItem> MainMenuItems
        {
            get
            {
                return _mainMenuItems;
            }
            set
            {
                _mainMenuItems = value;
                OnPropertyChanged(nameof(MainMenuItems));
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            
            ITheme theme = PaletteHelper.GetTheme();
            PrimaryColor = theme.PrimaryMid.Color;
            SelectedColor = PrimaryColor;

            _viewModel = this;
            _menuItems = new ObservableCollection<MenuItem>();
            _mainMenuItems = new ObservableCollection<MenuItem>();

            MenuItems.Add(new MenuItem(HomePage, typeof(HomeView), selectedIcon: PackIconKind.Home, unselectedIcon: PackIconKind.HomeOutline, HomeViewModel));
            HomeViewModel.IsLogginIn = true;

           
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

            LoginCommand = new LoginCommand(this);
            AddNewNotificationCommand = new AddNewNotificationCommand(_viewModel);
            DismissAllNotificationsCommand = new DismissAllNotificationsCommand(_viewModel);
            ThemeChangeCommand = new ThemeChangeCommand(_viewModel);

            AddNewNotificationCommand.Execute(new object()); 
            LoginCommand.Execute(new object());
        }
        public async Task GetUserAsync()
        {
            await App.API.LoginAsync(Environment.UserName);
            await Task.Delay(3000);
            OnLoginSuccesed(App.API.Person);
        }

        private void OnLoginSuccesed(Person person)
        {
            //IsLoginSuccesed = true;
            person.GetSettings();
            person.SettingsLoad += OnPerson_SettingsLoad;
            person.MenuItemsChanged += OnPerson_MenuItemsChanged;
            person.AddPages(_viewModel);
            
        }

        private void OnPerson_MenuItemsChanged(ObservableCollection<MenuItem> obj)
        {
            MenuItemsView = CollectionViewSource.GetDefaultView(MenuItems);
            MenuItemsView.Filter = MenuItemsFilter;
            HomeViewModel.IsLogginIn = false;
            HomeViewModel.IsLoginSuccesed = true;
        }

        private void OnPerson_SettingsLoad(Settings obj)
        {
            Color color = (Color)ColorConverter.ConvertFromString(App.API.Person.Settings.Color);
            SelectedColor = color;
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

        public bool MenuItemsFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(_searchKeyword))
            {
                return true;
            }

            return obj is MenuItem item
                   && item.Name.ToLower().Contains(_searchKeyword.ToLower());
        }

    }
}
