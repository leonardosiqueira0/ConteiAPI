using Microsoft.Data.SqlClient;
using ConteiAPI.Interfaces.Repositories;

namespace ConteiAPI.Repositories
{
    public class BasicoRepository : IBasicoRepository
    {
        private readonly IConfiguration _configuration;

        public BasicoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        public SqlConnection GetConnection(string db = "ConteiSQL")
        {
            return new SqlConnection(_configuration.GetConnectionString(db));
        }

        public void OpenConnection(SqlConnection connection)
        {
            connection.Open();
        }
    }
}
