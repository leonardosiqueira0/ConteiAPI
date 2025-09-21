using Microsoft.Data.SqlClient;

namespace ConteiAPI.Interfaces.Repositories
{
    public interface IBasicoRepository
    {
        SqlConnection GetConnection(string banco = "ContagemPoty");
        void OpenConnection(SqlConnection connection);
        void CloseConnection(SqlConnection connection);
    }
}
