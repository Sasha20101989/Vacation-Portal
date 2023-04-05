using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class StateRepository : IStateRepository
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        public ObservableCollection<SvApprovalStateViewModel> PersonStates { get; set; } = new ObservableCollection<SvApprovalStateViewModel>();
        public Action<ObservableCollection<SvApprovalStateViewModel>> PersonStatesChanged { get; set; }

        public StateRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public async Task<ObservableCollection<SvApprovalStateViewModel>> GetStateVacationsOnApproval(int UserIdSAP)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP
            };
            try
            {
                ObservableCollection<SvApprovalStateViewModel> personStateViewModels = new ObservableCollection<SvApprovalStateViewModel>();

                IEnumerable<SvApprovalStateDTO> svApprovalStateViewModelsDTOs = await database.QueryAsync<SvApprovalStateDTO>("usp_Get_Sv_Approval_State_By_Sv", parameters, commandType: CommandType.StoredProcedure);

                foreach(SvApprovalStateDTO state in svApprovalStateViewModelsDTOs)
                {
                    var vacations = App.API.Person.Subordinates.Select(s => s.Subordinate_Vacations);

                    foreach(Subordinate subordinate in App.API.Person.Subordinates)
                    {
                        foreach(Vacation vacation in subordinate.Subordinate_Vacations)
                        {
                            if(vacation.Id == state.Vacation_Record_Id)
                            {
                                personStateViewModels.Add(
                                    new SvApprovalStateViewModel(
                                        state.Id,
                                        state.Vacation_Record_Id,
                                        state.Supervisor_Id,
                                        state.Status_Id,
                                        vacation)
                                    );
                            }
                        }
                    }
                }
                return personStateViewModels;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task UpdateStateStatusAsync(SvApprovalStateViewModel state, int statusId)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", state.Id);
            parameters.Add("@Status_Id", statusId);
            parameters.Add("@UpdatedStatusId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                await database.QueryAsync("usp_Update_Sv_Approval_State_By_Id", parameters, commandType: CommandType.StoredProcedure);
                state.StatusId = parameters.Get<int>("@UpdatedStatusId");
                state.Vacation.VacationStatusId = parameters.Get<int>("@UpdatedStatusId");
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteStateAsync(int id)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Id = id
            };
            try
            {
                _ = await database.QueryAsync("usp_Delete_State", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
