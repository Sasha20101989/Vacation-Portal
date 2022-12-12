using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Pages
        private static readonly string _homePage = "Главная страница";
        private static readonly string _settingsPage = "Настройки";
        //private static readonly string SupervisorPage = "Страница руководителя";
        //private static readonly string HRPage = "Страница HR сотрудника";
        //private static readonly string Табельщик = "Страница HR сотрудника";
        //private static readonly string PersonalVacationPlanning = "Страница персонального планирования отпуска";
        #endregion

        #region Addons
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private Color? PrimaryColor { get; set; }
        private readonly MainWindowViewModel _viewModel;
        private readonly HomeViewModel _homeViewModel = new HomeViewModel();
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

        #region Props

        #region Theme props
        private ColorScheme _activeScheme;
        public ColorScheme ActiveScheme
        {
            get => _activeScheme;
            set
            {
                if(_activeScheme != value)
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
                if(_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged();

                    if(value is Color color)
                    {
                        ChangeCustomColor(color);
                    }
                }
            }
        }

        private bool _isCheckedTheme;
        public bool IsCheckedTheme
        {
            get => _isCheckedTheme;
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
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));

            }
        }

        private string _adminString = string.Empty;
        public string AdminString
        {
            get => _adminString;
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
                if(SetProperty(ref _searchKeyword, value))
                {
                    MenuItemsView.Refresh();
                }
                OnPropertyChanged(nameof(SearchKeyword));
            }
        }

        private MenuItem _selectedItem;
        public MenuItem SelectedItem
        {
            get => _selectedItem;
            set
            {

                SetProperty(ref _selectedItem, value);
                if(_selectedItem != null)
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
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }
        private ObservableCollection<MenuItem> _mainMenuItems;
        public ObservableCollection<MenuItem> MainMenuItems
        {
            get => _mainMenuItems;
            set
            {
                _mainMenuItems = value;
                OnPropertyChanged(nameof(MainMenuItems));
            }
        }

        #endregion

        public MainWindowViewModel()
        {

            ITheme theme = _paletteHelper.GetTheme();
            PrimaryColor = theme.PrimaryMid.Color;
            SelectedColor = PrimaryColor;

            _viewModel = this;
            _menuItems = new ObservableCollection<MenuItem>();
            _mainMenuItems = new ObservableCollection<MenuItem>();

            MenuItems.Add(new MenuItem(_homePage, typeof(HomeView), selectedIcon: PackIconKind.Home, unselectedIcon: PackIconKind.HomeOutline, _homeViewModel));

            _homeViewModel.IsLogginIn = true;

            HomeCommand = new AnotherCommandImplementation(
            _ =>
            {
                SearchKeyword = string.Empty;
                SelectedIndex = 0;
            });

            MovePrevPageCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if(!string.IsNullOrWhiteSpace(SearchKeyword))
                    {
                        SearchKeyword = string.Empty;
                    }

                    SelectedIndex--;
                },
                _ => SelectedIndex > 0);

            MoveNextPageCommand = new AnotherCommandImplementation(
               _ =>
               {
                   if(!string.IsNullOrWhiteSpace(SearchKeyword))
                   {
                       SearchKeyword = string.Empty;
                   }

                   SelectedIndex++;
               },
               _ => SelectedIndex < MenuItems.Count - 1);

            MoveToSettingsCommand = new AnotherCommandImplementation(
                _ =>
                {
                    SearchKeyword = string.Empty;
                    int index = 0;
                    for(int i = 0; i < MenuItems.Count; i++)
                    {
                        if(MenuItems[i].Name == _settingsPage)
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
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Ищу вас в базе данных...";
            });
            _ = await App.API.LoginAsync(Environment.UserName);

            await foreach(Access item in App.API.Person.FetchAccessAsync(DateTime.Now.Year))
            {
                App.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Приложение с доступом сотрудника";
                    App.SplashScreen.status.Foreground = Brushes.Black;
                });
                if(item.Is_Accounting)
                {
                    App.API.Person.Is_Accounting = true;
                    App.Current.Dispatcher.Invoke((Action) delegate
                    {
                        App.SplashScreen.status.Text = "Приложение с доступом табельщика";
                        App.SplashScreen.status.Foreground = Brushes.Black;
                    });
                }
                if(item.Is_Supervisor)
                {
                    App.API.Person.Is_Supervisor = true;
                    App.Current.Dispatcher.Invoke((Action) delegate
                    {
                        App.SplashScreen.status.Text = "Приложение с доступом руководителя";
                        App.SplashScreen.status.Foreground = Brushes.Black;
                    });
                }
                if(item.Is_HR)
                {
                    App.API.Person.Is_HR = true;
                    App.Current.Dispatcher.Invoke((Action) delegate
                    {
                        App.SplashScreen.status.Text = "Приложение с доступом HR сотрудника";
                        App.SplashScreen.status.Foreground = Brushes.Black;
                    });
                }
            }

            await foreach(Settings item in App.API.Person.FetchSettingsAsync())
            {
                Color color = (Color) ColorConverter.ConvertFromString(item.Color);
                SelectedColor = color;
            }
            OnLoginSuccesed(App.API.Person);
        }

        private void OnLoginSuccesed(Person person)
        {

            if(person != null)
            {
                person.MenuItemsChanged += OnPerson_MenuItemsChanged;
                person.AddPages(_viewModel);
            }
        }

        private void OnPerson_MenuItemsChanged(ObservableCollection<MenuItem> obj)
        {
            MenuItemsView = CollectionViewSource.GetDefaultView(MenuItems);
            MenuItemsView.Filter = MenuItemsFilter;
            _homeViewModel.IsLogginIn = false;
            _homeViewModel.IsLoginSuccesed = true;
        }

        private void ChangeCustomColor(object obj)
        {
            Color color = (Color) obj;

            if(ActiveScheme == ColorScheme.Primary)
            {

                _paletteHelper.ChangePrimaryColor(color);
                PrimaryColor = color;
            }
        }

        public bool MenuItemsFilter(object obj)
        {
            return string.IsNullOrWhiteSpace(_searchKeyword)
                   || (obj is MenuItem item
                   && item.Name.ToLower().Contains(_searchKeyword.ToLower()));
        }
    }
}
