using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller
{
    public class LocacaoController : ILocacaoController
    {
        private readonly ClienteController ClienteController = new ClienteController();
        private readonly VeiculoController VeiculoController = new VeiculoController();

        public void AdicionarLocacao(Locacao locacao, int funcionarioID)
        {
            using SqlConnection connection = new(ConnectionDB.GetConnectionString());
            connection.Open();

            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using SqlCommand command = new(Locacao.INSERTLOCACAO + " SELECT SCOPE_IDENTITY();", connection, transaction);

                command.Parameters.AddWithValue("@ClienteID", locacao.ClienteID);
                command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                command.Parameters.AddWithValue("@DataDevolucaoReal", (object?)locacao.DataDevolucaoReal ?? DBNull.Value);
                command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                command.Parameters.AddWithValue("@Multa", (object?)locacao.Multa ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", locacao.Status.ToString());

                int locacaoID = Convert.ToInt32(command.ExecuteScalar());
                locacao.SetLocacaoID(locacaoID);

                using SqlCommand commandFuncionario = new(Locacao.INSERTFUNCIONARIOLOCACAO, connection, transaction);

                commandFuncionario.Parameters.AddWithValue("@LocacaoID", locacaoID);
                commandFuncionario.Parameters.AddWithValue("@FuncionarioID", funcionarioID);
                commandFuncionario.ExecuteNonQuery();

                using SqlCommand cmdVeiculo = new(Locacao.SETVEICULOINDISPONIVEL, connection, transaction);
                cmdVeiculo.Parameters.AddWithValue("@id", locacao.VeiculoID);
                cmdVeiculo.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqlException e)
            {
                transaction.Rollback();
                throw new Exception("Erro ao adicionar locacao no BD: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception("Erro desconhecido ao adicionar locacao: " + e.Message);
            }
        }

        public List<Locacao> ListarTodasLocacoes()
        {
            List<Locacao> locacoes = [];

            using SqlConnection connection = new(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {

            using SqlCommand command = new(Locacao.SELECTALLLOCACOES, connection);

            using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Locacao locacao = new(
                        reader.GetInt32(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetDateTime(3),
                        reader.GetDateTime(4),
                        reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                        reader.GetDecimal(6),
                        reader.GetDecimal(7),
                        reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                        Enum.Parse<EStatusLocacao>(reader.GetString(9)));

                    locacao.SetLocacaoID(reader.GetInt32(0));

                    var cliente = ClienteController.BuscarClientePorID(locacao.ClienteID);
                    var veiculo = VeiculoController.BuscarVeiculoPorID(locacao.VeiculoID);

                    locacao.SetCliente(cliente);
                    locacao.SetVeiculo(veiculo);

                    locacoes.Add(locacao);
                }
                return locacoes;
            }
            catch (SqlException e)
            {
                throw new Exception("Erro ao listar locacoes no BD: " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro desconhecido ao listar locacoes: " + e.Message);
            }
        }

        public Locacao BuscarLocacaoPorID(int id)
        {

            using SqlConnection connection = new(ConnectionDB.GetConnectionString());
            connection.Open();

            using SqlCommand command = new(Locacao.SELECTLOCACAOPORID, connection);
            command.Parameters.AddWithValue("@ID", id);

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null!;

            Locacao locacao = new(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.GetDateTime(3),
                reader.GetDateTime(4),
                reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                reader.GetDecimal(6),
                reader.GetDecimal(7),
                reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                Enum.Parse<EStatusLocacao>(reader.GetString(9)));

            locacao.SetLocacaoID(reader.GetInt32(0));

            var cliente = ClienteController.BuscarClientePorID(locacao.ClienteID);
            var veiculo = VeiculoController.BuscarVeiculoPorID(locacao.VeiculoID);

            locacao.SetCliente(cliente);
            locacao.SetVeiculo(veiculo);

            return locacao;
        }

        public void FinalizarLocacao(int locacaoID, decimal multa)
        {
            using SqlConnection connection = new(ConnectionDB.GetConnectionString());
            connection.Open();

            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using SqlCommand commandLocacao = new(Locacao.UPDATELOCACAOFINALIZACAO, connection, transaction);
                commandLocacao.Parameters.AddWithValue("@data", DateTime.Now);
                commandLocacao.Parameters.AddWithValue("@multa", multa);
                commandLocacao.Parameters.AddWithValue("@id", locacaoID);
                commandLocacao.ExecuteNonQuery();

                using SqlCommand commandVeiculos = new(Locacao.UPDATEVEICULOSFINALIZACAO, connection, transaction);
                commandVeiculos.Parameters.AddWithValue("@locacaoID", locacaoID);
                commandVeiculos.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqlException e)
            {
                transaction.Rollback();
                throw new Exception("Erro ao finalizar locacao no BD: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception("Erro desconhecido ao finalizar locacao: " + e.Message);
            }
        }
    }
}
