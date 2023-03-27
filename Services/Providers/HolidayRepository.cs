using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Vacation_Portal.Commands;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class HolidayRepository : IHolidayRepository
    {
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

        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        #region Commands
        public ICommand LoadHolidays { get; } = new LoadHolidaysCommand();
        public ICommand LoadHolidayTypes { get; } = new LoadHolidayTypesCommand();
        #endregion

        #region Props
        public Action<ObservableCollection<HolidayViewModel>> OnHolidaysChanged { get; set; }
        public Action<ObservableCollection<Holiday>> OnHolidayTypesChanged { get; set; }

        private ObservableCollection<HolidayViewModel> _holidays = new ObservableCollection<HolidayViewModel>();
        public ObservableCollection<HolidayViewModel> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }

        private ObservableCollection<Holiday> _holidayTypes = new ObservableCollection<Holiday>();
        public ObservableCollection<Holiday> HolidayTypes
        {
            get => _holidayTypes;
            set
            {
                _holidayTypes = value;
                OnPropertyChanged(nameof(HolidayTypes));
            }
        }
        #endregion

        public HolidayRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }
        public async Task<IEnumerable<Holiday>> GetHolidayTypesAsync()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                IEnumerable<HolidayDTO> holidayTypesDTOs = await database.QueryAsync<HolidayDTO>("usp_Load_Holiday_Types", commandType: CommandType.StoredProcedure);

                return holidayTypesDTOs.Select(ToHolidayTypes);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private Holiday ToHolidayTypes(HolidayDTO dto)
        {
            return new Holiday(dto.Id, dto.Holiday_Name);
        }
        public async Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Date_Year_Current = yearCurrent,
                Date_Year_Next = yearNext
            };
            try
            {

                IEnumerable<HolidayDTO> holidayDTOs = await database.QueryAsync<HolidayDTO>("usp_Load_Holidays", parameters, commandType: CommandType.StoredProcedure);
                return holidayDTOs.Select(ToHolidays);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private HolidayViewModel ToHolidays(HolidayDTO dto)
        {
            return new HolidayViewModel(dto.Id, dto.Holiday_Name, dto.Holiday_Date, dto.Holiday_Year);
        }

        public async Task DeleteHolidayAsync(HolidayViewModel holiday)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Holiday_Id = holiday.Id
            };
            _ = await database.QueryAsync<HolidayDTO>("usp_Delete_Holiday", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task AddHolidayAsync(HolidayViewModel holiday)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Holiday_Id = holiday.Id,
                Holiday_Date = holiday.Date,
                Holiday_Year = holiday.Date.Year
            };
            _ = await database.QueryAsync<HolidayDTO>("usp_Add_Holiday", parameters, commandType: CommandType.StoredProcedure);
        }


    }
}
