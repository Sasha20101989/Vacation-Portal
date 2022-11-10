using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using Microsoft.Data.SqlClient;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;
        public UserProvider(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }
        public IEnumerable<Person> GetUser(string account)
        {
            using (IDbConnection database = _sqlDbConnectionFactory.Connect())
            {
                object parameters = new
                {
                    Account = account
                };
                try
                {
                    IEnumerable<PersonDTO> userDTOs = database.Query<PersonDTO>("usp_Load_Description_For_User", parameters, commandType: CommandType.StoredProcedure);
                    return userDTOs.Select(ToUser);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                
            }
        }
        public Person ToUser(PersonDTO dto)
        {
            return new Person(dto.Name,dto.Surname,dto.Patronymic, dto.Account, dto.Department_Id, dto.Is_Supervisor, dto.Is_HR);
        }
    }
}
