using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class DepartmentProvider: IDepartmentProvider
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        public DepartmentProvider(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }


        public async Task<IEnumerable<Department>> GetDepartmentForUser(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                @Account = account
            };
            try
            {
                IEnumerable<DepartmentDTO> departmentDTOs = await database.QueryAsync<DepartmentDTO>("usp_Load_Departments_For_User", parameters, commandType: CommandType.StoredProcedure);
                return departmentDTOs.Select(ToDepartment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public Department ToDepartment(DepartmentDTO dto)
        {
            return new Department(dto.Department_Name);
        }
    }
}
