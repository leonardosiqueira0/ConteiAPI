using ConteiAPI.DTOs;
using ConteiAPI.Interfaces.Repositories;
using ConteiAPI.Interfaces.Services;
using ConteiAPI.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ConteiAPI.Repositories
{
    public class ContagemRepository(IConfiguration configuration, IEtiquetaService etiquetaService) : BasicoRepository(configuration), IContagemRepository
    {
        public void CriarContagem(ContagemModel contagemModel)
        {
            string query = @"
                INSERT INTO Estoque.Contagem (Id, Data, Deposito, Descricao, Status)
                VALUES (@Id, @Data, @Deposito, @Descricao, @Status)";
            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", contagemModel.Id);
                    command.Parameters.AddWithValue("@Data", contagemModel.Data);
                    command.Parameters.AddWithValue("@Deposito", contagemModel.Deposito);
                    command.Parameters.AddWithValue("@Descricao", contagemModel.Descricao);
                    command.Parameters.AddWithValue("@Status", contagemModel.Status);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CriarContagemEtiqueta(ContagemEtiquetaModel contagemEtiquetaModel)
        {
            string query = @"
                INSERT INTO Estoque.ContagemEtiqueta (Id, ContagemID, EtiquetaID, UsuarioID, Data, Quantidade, Status)
                VALUES (@Id, @ContagemID, @EtiquetaID, @UsuarioID, @Data, @Quantidade, @Status)";
            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", contagemEtiquetaModel.Id);
                    command.Parameters.AddWithValue("@ContagemID", contagemEtiquetaModel.ContagemID);
                    command.Parameters.AddWithValue("@EtiquetaID", contagemEtiquetaModel.EtiquetaID);
                    command.Parameters.AddWithValue("@UsuarioID", contagemEtiquetaModel.UsuarioID);
                    command.Parameters.AddWithValue("@Data", contagemEtiquetaModel.Data);
                    command.Parameters.AddWithValue("@Quantidade", contagemEtiquetaModel.Quantidade);
                    command.Parameters.AddWithValue("@Status", contagemEtiquetaModel.Status);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool VerificarSeContagemJaFoiRealizada(Guid contagemID, int etiquetaID)
        {
            string query = @"
                SELECT COUNT(1)
                FROM Estoque.ContagemEtiqueta
                WHERE ContagemID = @ContagemID AND EtiquetaID = @EtiquetaID";

            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContagemID", contagemID);
                    command.Parameters.AddWithValue("@EtiquetaID", etiquetaID);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public List<ContagemModel> ObterContagens(string? status)
        {
            var contagens = new List<ContagemModel>();
            string query = status is null
                ? @"SELECT Id, Data, Deposito, Descricao, Status FROM Estoque.Contagem"
                : @"SELECT Id, Data, Deposito, Descricao, Status FROM Estoque.Contagem WHERE Status = @status";

            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                using (var command = new SqlCommand(query, connection))
                {
                    if (status is not null)
                        command.Parameters.AddWithValue("@status", status);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contagens.Add(new ContagemModel
                            {
                                Id = reader.GetGuid(0),
                                Data = reader.GetDateTime(1),
                                Deposito = reader.GetString(2),
                                Descricao = reader.GetString(3),
                                Status = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return contagens;
        }

        public List<ContagemEtiquetaDTO> ObterContagemEtiquetas(Guid contagemID)
        {
            var etiquetas = new List<ContagemEtiquetaDTO>();
            string query = @"SELECT Id, ContagemID, EtiquetaID, UsuarioID, Data, Quantidade, Status FROM Estoque.ContagemEtiqueta WHERE ContagemID = @ContagemID";
            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContagemID", contagemID);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var etiquetaID = reader.GetInt32(2);
                            var etiquetaDTO = etiquetaService.GetEtiquetaByID(etiquetaID);
                            if (etiquetaDTO == null)
                                throw new InvalidOperationException($"Etiqueta com ID {etiquetaID} não encontrada.");

                            etiquetas.Add(new ContagemEtiquetaDTO
                            {
                                Id = reader.GetGuid(0),
                                ContagemID = reader.GetGuid(1),
                                Etiqueta = etiquetaDTO,
                                UsuarioID = reader.GetString(3),
                                Data = reader.GetDateTime(4),
                                Quantidade = reader.GetInt32(5),
                                Status = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            return etiquetas;
        }

        public void Finalizar(Guid id)
        {
            string query = @"
                UPDATE
	Estoque.Contagem
SET
	Status = 'Finalizado'
WHERE ID = @ContagemID";
            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContagemID", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
