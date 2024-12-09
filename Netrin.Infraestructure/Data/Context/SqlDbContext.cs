using Microsoft.Data.SqlClient;
using System.Data;

namespace Netrin.Infraestructure.Data.Context
{
    public class SqlDbContext
    {
        private readonly string _connectionString;

        public SqlDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CriarConexao()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
