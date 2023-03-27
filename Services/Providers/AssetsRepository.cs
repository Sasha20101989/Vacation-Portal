using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using Vacation_Portal.DbContext;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;

        public List<Status> AllStatuses { get; set; }
        public AssetsRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
            AllStatuses = GetStatuses();
        }

        public List<Status> GetStatuses()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                IEnumerable<Status> statusDTOs = database.Query<Status>("usp_Get_Statuses", commandType: CommandType.StoredProcedure);
                List<Status> allStatuses = statusDTOs.ToList();
                return allStatuses;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
