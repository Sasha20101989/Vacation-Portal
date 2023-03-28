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
using System.Windows.Media;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;
namespace Vacation_Portal.Services.Providers
{
    public class VacationRepository : IVacationRepository
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

        #region Props
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        private ObservableCollection<Vacation> _processedVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> ProcessedVacations
        {
            get => _processedVacations;
            set
            {
                _processedVacations = value;
                OnPropertyChanged(nameof(ProcessedVacations));
            }
        }

        private ObservableCollection<Vacation> _vacationsOnApproval = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsOnApproval
        {
            get => _vacationsOnApproval;
            set
            {
                _vacationsOnApproval = value;
                OnPropertyChanged(nameof(VacationsOnApproval));
            }
        }

        public ObservableCollection<Vacation> Vacations { get; set; } = new ObservableCollection<Vacation>();

        #endregion

        public Action<List<VacationViewModel>> OnVacationsChanged { get; set; }

        public VacationRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        #region Get Vacation
        public async Task<IEnumerable<Vacation>> LoadVacationsAsync(int UserIdSAP)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP
            };
            try
            {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Vacation_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationDTOs.Select(ToVacation);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private Vacation ToVacation(VacationDTO dto)
        {
            BrushConverter converter = new BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Color);
            return new Vacation(dto.Id, dto.Vacation_Name, dto.User_Id, ReturnUserName(dto.User_Id), ReturnUserSurname(dto.User_Id), dto.Type_Id, dto.Count, brushColor, dto.Start_Date, dto.End_Date, dto.Status_Id, dto.Creator_Id);
        }
        private string ReturnUserName(int sapId)
        {
            foreach(Person person in App.API.FullPersons)
            {
                if(person.Id_SAP == sapId)
                {
                    return person.Name;
                }
            }
            return null;
        }
        private string ReturnUserSurname(int sapId)
        {
            foreach(Person person in App.API.FullPersons)
            {
                if(person.Id_SAP == sapId)
                {
                    return person.Surname;
                }
            }
            return null;
        }
        #endregion

        public async Task<Vacation> AddVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            var parameters = new DynamicParameters();
            parameters.Add("@User_Id_SAP", vacation.User_Id_SAP);
            parameters.Add("@Vacation_Id", vacation.Vacation_Id);
            parameters.Add("@Vacation_Year", vacation.Date_Start.Year);
            parameters.Add("@Vacation_Start_Date", vacation.Date_Start);
            parameters.Add("@Vacation_End_Date", vacation.Date_end);
            parameters.Add("@Vacation_Status_Id", vacation.Vacation_Status_Id);
            parameters.Add("@Creator_Id", App.API.Person.Id_Account);
            parameters.Add("@InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                await database.ExecuteAsync(
                    "usp_Add_Vacation", parameters, commandType: CommandType.StoredProcedure);

                vacation.Id = parameters.Get<int>("@InsertedId");

                return vacation;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task DeleteVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Id = vacation.Id
            };
            try
            {
                _ = await database.QueryAsync<Vacation>("usp_Delete_Vacation", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Date_Start = vacation.Date_Start,
                Vacation_Date_End = vacation.Date_end,
                Vacation_Status_Id = vacation.Vacation_Status_Id
            };
            try
            {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Conflicting_Vacation_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationDTOs;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task UpdateVacationStatusAsync(int vacationId, int statusId)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            object parameters = new
            {
                Id = vacationId,
                Vacation_Status_Id = statusId
            };
            try
            {
                await database.QueryAsync("usp_Update_Vacation_Status", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
