using Microsoft.Data.SqlClient;
using ConteiAPI.Interfaces;
using ConteiAPI.Models;
using ConteiAPI.Repositories;
using ConteiAPI.Interfaces.Repositories;

namespace ConteiAPI.Repositories
{
    public class AutenticacaoRepository(IConfiguration configuration) : BasicoRepository(configuration), Interfaces.Repositories.IAutenticacaoRepository
    {
        public void CreateRefreshToken(RefreshTokenModel refreshToken)
        {
            string query = @"
                INSERT INTO Global.RefreshTokens (Token, ExpirationDate, IsRevoked, UserId)
                VALUES (@Token, @ExpirationDate, @IsRevoked, @UserId);
            ";

            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Token", refreshToken.Token);
                    command.Parameters.AddWithValue("@ExpirationDate", refreshToken.ExpirationDate);
                    command.Parameters.AddWithValue("@IsRevoked", refreshToken.IsRevoked);
                    command.Parameters.AddWithValue("@UserId", refreshToken.UserId);
                    Console.WriteLine(command.ToString());
                    OpenConnection(connection);
                    command.ExecuteNonQuery();
                    CloseConnection(connection);
                }
            }


        }

        public RefreshTokenModel? GetRefreshToken(string Token)
        {
            string query = @"
                SELECT TOP 1 Id, Token, ExpirationDate, IsRevoked, UserId
                FROM Global.RefreshTokens
                WHERE Token = @Token AND IsRevoked = 0
            ";

            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Token", Token);
                    OpenConnection(connection);
                    Console.WriteLine(query);
                    Console.WriteLine(Token);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var refreshToken = new RefreshTokenModel
                            {
                                Id = reader.GetGuid(0),
                                Token = reader.GetString(1),
                                ExpirationDate = reader.GetDateTime(2),
                                IsRevoked = reader.GetBoolean(3),
                                UserId = reader.GetGuid(4)
                            };

                            CloseConnection(connection);
                            return refreshToken;
                        }
                    }
                    CloseConnection(connection);
                }
                return null;
            }
        }

        public void RevokeRefreshToken(string token)
        {
            string query = @"
                UPDATE Global.RefreshTokens
                SET IsRevoked = 1
                WHERE Token = @Token AND IsRevoked = 0
            ";

            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Token", token);
                    OpenConnection(connection);
                    command.ExecuteNonQuery();
                    CloseConnection(connection);
                }
            }
        }
    }
}
