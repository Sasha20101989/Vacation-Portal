using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class VacationAllowanceRepository: IVacationAllowanceRepository
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        public ObservableCollection<VacationAllowanceViewModel> VacationAllowances { get; set; } = new ObservableCollection<VacationAllowanceViewModel>();

        public VacationAllowanceRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }
        #region Get Vacation Allowance
        public async Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP
            };
            try
            {
                IEnumerable<VacationAllowanceDTO> vacationAllowanceDTOs = await database.QueryAsync<VacationAllowanceDTO>("usp_Load_Vacation_Allowance_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationAllowanceDTOs.Select(ToVacationAllowance);
            } catch(Exception)
            {
                return null;
            }
        }
        private VacationAllowanceViewModel ToVacationAllowance(VacationAllowanceDTO dto)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new VacationAllowanceViewModel(dto.User_Id_SAP, dto.Vacation_Name, dto.Vacation_Id, dto.Vacation_Year, dto.Vacation_Days_Quantity, brushColor);
        }
        #endregion

        public async Task UpdateVacationAllowanceAsync(int userIdSAP, int vacation_Id, int year, int count)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = userIdSAP,
                Vacation_Id = vacation_Id,
                Vacation_Year = year,
                Quantity = count
            };
            try
            {
                await database.QueryAsync("usp_Update_Vacation_Allowance", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
