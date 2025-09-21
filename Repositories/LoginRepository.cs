using Microsoft.Data.SqlClient;
using ConteiAPI.Interfaces;
using ConteiAPI.Helpers;
using ConteiAPI.Models;

namespace ConteiAPI.Repositories
{
    public class LoginRepository(IConfiguration configuration) : BasicoRepository(configuration), ILoginRepository
    {
        public UsuarioModel? Login(DTOs.LoginRequestDto loginRequest)
        {
            string query = @"
                SELECT TOP 1
                    CONVERT(Uniqueidentifier, A1.UsuarioID) UsuarioID,
                    Nome,
                    Usuario,
                    Senha,
                    Convert(int, departamentoID) DepartamentoID,
                    SapID,
                    Label
                FROM
                    Global.Usuarios A1
                LEFT JOIN
                    Global.UsuariosAplicacoes A2
                        ON A1.UsuarioID = A2.UsuarioID
                LEFT JOIN
                    GLOBAL.Aplicacoes A3
                        ON A2.AplicacaoID = A3.AplicacaoID
                WHERE
                    A1.Usuario = @User
                    and A1.senha = @Password
                    AND A3.Descricao = 'ContagemPoty'
            ";

            using (var connection = GetConnection(db: "ConteiDB")) // CASO A AUTENTICAÇÃO SEJA EM BANCO DIFERENTE, ALTERAR AQUI
            {
                using (var command = new SqlCommand(query, connection))
                {
                    var passwordCryptography = Sha256Hasher.HashPassword(loginRequest.Password ?? "");
                    command.Parameters.AddWithValue("@User", loginRequest.User);
                    command.Parameters.AddWithValue("@Password", passwordCryptography);
                    OpenConnection(connection);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new UsuarioModel
                            {
                                Id = reader.GetGuid(0),
                                Nome = reader.GetString(1),
                                Usuario = reader.GetString(2),
                                Senha = reader.GetString(3),
                                DepartamentoID = (!reader.IsDBNull(4)) ? reader.GetInt32(4) : null,
                                SapID = (!reader.IsDBNull(5)) ? reader.GetString(5) : null,
                                Label = (!reader.IsDBNull(6)) ? reader.GetString(6) : null
                            };

                            CloseConnection(connection);
                            return user;
                        }
                    }
                    CloseConnection(connection);
                }
                return null;
            }
        }
    }
}
