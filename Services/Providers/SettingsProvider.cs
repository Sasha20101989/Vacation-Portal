using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class SettingsProvider: ISettingsProvider
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;
        public SettingsProvider(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }
        public IEnumerable<Settings> GetSettingsUI(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Account = account
            };
            IEnumerable<SettingsDTO> settingsDTOs = database.Query<SettingsDTO>("usp_Load_Settings_For_User", parameters, commandType: CommandType.StoredProcedure);
            return settingsDTOs.Select(ToSettings);
        }

        private Settings ToSettings(SettingsDTO dto)
        {
            return new Settings(dto.Account, dto.Font_Size, dto.Color);
        }
    }
}
