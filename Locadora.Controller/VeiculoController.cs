using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locadora.Controller.Intefaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller
{
    public class VeiculoController : IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Veiculo.INSERTVEICULO, connection, transaction);

                    command.Parameters.AddWithValue("@CategoriaID", veiculo.CategoriaID);
                    command.Parameters.AddWithValue("@placa", veiculo.Placa);
                    command.Parameters.AddWithValue("@Marca", veiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                    command.Parameters.AddWithValue("@Ano", veiculo.Ano);
                    command.Parameters.AddWithValue("@StatusVeiculo", veiculo.StatusVeiculo);

                    command.ExecuteNonQuery();

                    transaction.Commit();

                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar veiculo " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar veiculo " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        public void AtualizarStatusVeiculo(string statusVeiculo, string placa)
        {
            Veiculo veiculo = BuscarVeiculoPlaca(placa) ??
            throw new Exception("Veiculo nao encontrado!");

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction);
                try
                {
                    command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                    command.Parameters.AddWithValue("@IdVeiculo", veiculo.VeiculoID);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar o status do veiculo " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar o status do veiculo " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        public Veiculo BuscarVeiculoPlaca(string placa)
        {
            Veiculo veiculo = null;

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOBYPLACA, connection))
            {
                try
                {

                    command.Parameters.AddWithValue("@Placa", placa);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            veiculo = new Veiculo(
                                                          reader.GetInt32(1),
                                                          reader.GetString(2),
                                                          reader.GetString(3),
                                                          reader.GetString(4),
                                                          reader.GetInt32(5),
                                                          reader.GetString(6)
                                                         );
                            veiculo.setVeiculoID(reader.GetInt32(0));
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar veiculos " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao listar veiculos " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return veiculo ?? throw new Exception("Veiculo nao encontrado!");

            }
        }

        public void DeletarVeiculo(int idVeiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();


            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Veiculo.DELETEVEICULO, connection, transaction);
                try
                {
                    command.Parameters.AddWithValue("@IdVeiculo", idVeiculo);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar veiculo " + ex.Message);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao deletar veiculo" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Veiculo> ListarTodosVeiculos()
        {
            var veiculos = new List<Veiculo>();

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTALLVEICULOS, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Veiculo veiculo = new Veiculo(
                                                          reader.GetInt32(0),
                                                          reader.GetString(1),
                                                          reader.GetString(2),
                                                          reader.GetString(3),
                                                          reader.GetInt32(4),
                                                          reader.GetString(5)
                                                         );
                            veiculos.Add(veiculo);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar veiculos " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao listar veiculos " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return veiculos;
            }
        }
    }
}
