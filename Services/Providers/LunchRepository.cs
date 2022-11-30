using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class LunchRepository : ILunchRepository
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        #region PropChange
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(member, value))
            {
                return false;
            }
            member = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public LunchRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
            _holidays = new List<HolidayViewModel>();
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 1)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 2)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 3)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 4)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 5)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 6)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 7)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 1, 8)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 2, 23)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 3, 8)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 5, 1)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 5, 9)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 6, 12)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 11, 4)));

            //Holidays.Add(new HolidayViewModel("Выходной", new DateTime(2022, 5, 8)));
            //Holidays.Add(new HolidayViewModel("Праздник", new DateTime(2022, 11, 6)));
        }

        public Person Person { get; set; } = new Person("я", "я", "я", "ru13779", 1, false, false);
        private List<HolidayViewModel> _holidays;
        public List<HolidayViewModel> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();
        public Action<List<HolidayViewModel>> OnHolidaysChanged { get; set; }

        public async Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(DateTime start, DateTime end)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                DateStart = start,
                DateEnd = end
            };
            IEnumerable<HolidayViewModel> dates = await database.QueryAsync<HolidayViewModel>("usp_Load_Holidays", parameters, commandType: CommandType.StoredProcedure);
            return dates.Select(ToDates);
        }

        private HolidayViewModel ToDates(HolidayViewModel dto)
        {
            return new HolidayViewModel(dto.TypeOfHoliday, dto.Date);
        }

        public async Task<IEnumerable<Settings>> GetSettingsAsync(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Account = account
            };
            IEnumerable<SettingsDTO> settingsDTOs = await database.QueryAsync<SettingsDTO>("usp_Load_Settings_For_User", parameters, commandType: CommandType.StoredProcedure);
            return settingsDTOs.Select(ToSettings);
        }

        private Settings ToSettings(SettingsDTO dto)
        {
            return new Settings(dto.Account, dto.Color);
        }

        public Task<IEnumerable<Person>> GetUser(string account)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PersonDTO>> LoginAsync(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Account = account
            };
            try
            {
                IEnumerable<PersonDTO> userDTOs = await database.QueryAsync<PersonDTO>("usp_Load_Description_For_User", parameters, commandType: CommandType.StoredProcedure);
                Persons.AddRange(userDTOs);
                Person = new Person(Persons[0].Name, Persons[0].Surname, Persons[0].Patronymic, Persons[0].Account, Persons[0].Department_Id, Persons[0].Is_Supervisor, Persons[0].Is_HR);
                return userDTOs;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
