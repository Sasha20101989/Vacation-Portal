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

        public Person Person {get;set; }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();

        public Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            throw new NotImplementedException();
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
            using (IDbConnection database = _sqlDbConnectionFactory.Connect())
            {
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }

            }
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

    }
}
