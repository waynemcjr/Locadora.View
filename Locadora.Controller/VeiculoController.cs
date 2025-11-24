using Locadora.Controller.Interfaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Utils.Databases;

namespace Locadora.Controller
{
    public class VeiculoController : IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    
                    try
                    {
                        SqlCommand command = new SqlCommand(Veiculo.INSERTVEICULO, connection,transaction);

                        command.Parameters.AddWithValue("@CategoriaID",veiculo.CategoriaID);
                        command.Parameters.AddWithValue("@Placa", veiculo.Placa);
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
                        throw new Exception("Erro ao adicionar veículo" + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao adicionar veículo" + e.Message);
                    }
                }
            }
        }

        public Veiculo BuscarVeiculoPorPlaca(string placa)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOPORPLACA, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Placa", placa);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Veiculo veiculo = new Veiculo(
                                                              reader.GetInt32(1),
                                                              reader.GetString(2),
                                                              reader.GetString(3),
                                                              reader.GetString(4),
                                                              reader.GetInt32(5),
                                                              reader.GetString(6)
                                                             );
                                veiculo.SetVeiculoID(reader.GetInt32(0));

                                return veiculo;
                            }
                            return null;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro ao listar veículos: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro inesperado ao listar veículos: " + e.Message);
                    }

                }
            }
        }

        public Veiculo BuscarVeiculoPorID(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOPORID, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@idVeiculo", id);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Veiculo veiculo = new Veiculo(
                                                              reader.GetInt32(1),
                                                              reader.GetString(2),
                                                              reader.GetString(3),
                                                              reader.GetString(4),
                                                              reader.GetInt32(5),
                                                              reader.GetString(6)
                                                             );
                                veiculo.SetVeiculoID(reader.GetInt32(0));

                                return veiculo;
                            }
                            return null;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro ao listar veículos: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro inesperado ao listar veículos: " + e.Message);
                    }

                }
            }
        }

        public List<Veiculo> ListarTodosVeiculos()
        {
            var veiculos = new List<Veiculo>();

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
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
                        throw new Exception("Erro ao listar veículos: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro inesperado ao listar veículos: " + e.Message);
                    }
                    return veiculos;
                }
            }
        }

        public void AtualizarVeiculo(string statusVeiculo,string placa)
        {
            var veiculoEncontrado = BuscarVeiculoPorPlaca(placa) ?? throw new Exception("Veículo não encontrado.");


            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {


                    using (SqlCommand command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                            command.Parameters.AddWithValue("@idVeiculo", veiculoEncontrado.VeiculoID);

                            command.ExecuteNonQuery();

                            transaction.Commit();
                        }

                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro ao atualizar veículos: " + ex.Message);
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro inesperado ao atualizar veículos: " + e.Message);
                        }
                    }
                }
            }
        }


        public void DeletarVeiculo(string placa)
        {

            var veiculoEncontrado = BuscarVeiculoPorPlaca(placa) ?? throw new Exception("Veículo não encontrado.");


            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {


                    using (SqlCommand command = new SqlCommand(Veiculo.DELETEVEICULO, connection,transaction))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@idVeiculo", veiculoEncontrado.VeiculoID);

                            command.ExecuteNonQuery();

                            transaction.Commit();
                        }

                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro ao deletar veículos: " + ex.Message);
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw new Exception("Erro inesperado ao deletar veículos: " + e.Message);
                        }
                    }
                }
            }
        }

    }
}
