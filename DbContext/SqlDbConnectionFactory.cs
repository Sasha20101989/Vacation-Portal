using Microsoft.Data.SqlClient;
using System.Data;

namespace Vacation_Portal.DbContext {
    public class SqlDbConnectionFactory {
        private readonly string _connectionString;
        public SqlDbConnectionFactory(string connectionString) {
            _connectionString = connectionString;
        }
        public IDbConnection Connect() {
            return new SqlConnection(_connectionString);
        }
    }
}
