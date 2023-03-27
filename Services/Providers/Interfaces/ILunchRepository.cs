using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces {
    public interface ILunchRepository {
        ICommand Login { get; }
        Action<Person> OnLoginSuccess { get; set; }
        Person Person { get; set; }
        ObservableCollection<Person> FullPersons { get; set; }
        ObservableCollection<Subordinate> PersonsWithVacationsOnApproval { get; set; }
        
        void GetPersonsWithVacationsOnApproval();
        Task<Person> LoginAsync(string account);
        
    }
}
