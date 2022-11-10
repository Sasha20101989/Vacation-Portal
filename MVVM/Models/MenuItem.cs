using MaterialDesignThemes.Wpf;
using Vacation_Portal.MVVM.ViewModels.Base;
using System;
using System.Windows;
using System.Collections.Generic;

namespace Vacation_Portal.MVVM.Models
{
    public class MenuItem : ViewModelBase {
        private readonly Type _contentType;
        private readonly object _dataContext;
        private object _content;
        //public object Content => _content = CreateContent();
        public object Content => _content ??= CreateContent();
        public string Name { get; set; }
        private Thickness _marginRequirement = new Thickness (16);

        public MenuItem (string name, Type contentType, PackIconKind selectedIcon, PackIconKind unselectedIcon, object dataContext = null) {
            Name = name;
            _contentType = contentType;
            _dataContext = dataContext;
            SelectedIcon = selectedIcon;
            UnselectedIcon = unselectedIcon;
        }

        private bool _hasNotifications;
        public bool HasNotifications
        {
            get
            {
                return _hasNotifications;
            }
            set
            {
                _hasNotifications = value;
                OnPropertyChanged(nameof(HasNotifications));
            }
        }
        private int _notificationNumber=0;
        public object Notifications
        {
            get
            {
                if (_notificationNumber == 0)
                {
                    _hasNotifications = false;
                    return null;
                }
                else
                {
                    if (_notificationNumber < 100)
                    {
                        _hasNotifications= true;
                        return _notificationNumber;
                    }
                    else
                    {
                        _hasNotifications = true;
                        return "99+";
                    }
                }
            }
        }

        public PackIconKind SelectedIcon { get; set; }
        public PackIconKind UnselectedIcon { get; set; }

        public Thickness MarginRequirement {
            get => _marginRequirement;
            set => SetProperty (ref _marginRequirement, value);
        }

        private object CreateContent () {
            var content = Activator.CreateInstance (_contentType);
            if (_dataContext != null && content is FrameworkElement element) {
                element.DataContext = _dataContext;
            }

            return content;
        }
        public void AddNewNotification () {
            _notificationNumber++;
            OnPropertyChanged (nameof (Notifications));
        }
        public void DismissAllNotifications () {
            _notificationNumber = 0;
            OnPropertyChanged (nameof (Notifications));
        }

        public override bool Equals(object obj)
        {
            return obj is MenuItem item && Name == item.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
