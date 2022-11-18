using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vacation_Portal.DbContext;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.Services.Providers.Interfaces;
using System.Data;
using System.Linq;
using System.Windows;
using Dapper;
using Vacation_Portal.DTOs;

namespace Vacation_Portal.Services.Providers
{
    public class LunchRepository : ILunchRepository
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        public LunchRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public Person Person { get; set; }

        public Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Settings>> GetSettingsAsync(string account)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetUser(string account)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Person> LoginAsync(string account)
        {
            using (IDbConnection database = _sqlDbConnectionFactory.Connect())
            {
                object parameters = new
                {
                    Account = account
                };
                try
                {
                    PersonDTO userDTOs = (PersonDTO)await database.QueryAsync<PersonDTO>("usp_Load_Description_For_User", parameters, commandType: CommandType.StoredProcedure);
                    Person person = new Person(userDTOs.Name, userDTOs.Surname, userDTOs.Patronymic, userDTOs.Account, userDTOs.Department_Id, userDTOs.Is_Supervisor, userDTOs.Is_HR);
                    Person = person;
                    return person;
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
            return new Person(dto.Name, dto.Surname, dto.Patronymic, dto.Account, dto.Department_Id, dto.Is_Supervisor, dto.Is_HR);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
