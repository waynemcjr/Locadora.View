using Locadora.Controller.Interfaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller
{
    public class FuncionarioController : IFuncionarioController
    {
        public void AdicionarFuncionario(Funcionario funcionario)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(Funcionario.INSERTFUNCIONARIO, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Nome", funcionario.Nome);
                            command.Parameters.AddWithValue("@CPF", funcionario.CPF);
                            command.Parameters.AddWithValue("@Email", funcionario.Email);
                            command.Parameters.AddWithValue("@Salario", funcionario.Salario ?? (object)DBNull.Value);

                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao inserir funcionário: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao inserir funcionário: " + e.Message);
                    }
                }
            }
        }

        public Funcionario BuscarFuncionarioPorCPF(string cpf)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(Funcionario.SELECTFUNCIONARIOPORCPF, connection))
                    {
                        command.Parameters.AddWithValue("@CPF", cpf);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Funcionario funcionario = new Funcionario(
                                                                    reader["Nome"].ToString(),
                                                                    reader["CPF"].ToString(),
                                                                    reader["Email"].ToString(),
                                                                    reader["Salario"] != DBNull.Value ?
                                                                    (Decimal)reader["Salario"] : null);
                                funcionario.SetFuncionarioID((int)reader["FuncionarioID"]);
                                return funcionario;
                            }
                            return null;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar funcionário: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao buscar funcionário: " + e.Message);
                }

            }
        }

        public List<Funcionario> ListarTodosFuncionarios()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(Funcionario.SELECTTODOSFUNCIONARIOS, connection))
                    {
                        List<Funcionario> funcionarios = new List<Funcionario>();

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                funcionarios.Add(new Funcionario(reader["Nome"].ToString(),
                                                                 reader["CPF"].ToString(),
                                                                 reader["Email"].ToString(),
                                                                 reader["Salario"] != DBNull.Value ?
                                                                 (Decimal)reader["Salario"] : null));

                            }
                            return funcionarios;
                        }
                    }
                    
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar funcionários: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao listar funcionários: " + e.Message);
                }

            }
        }

        public void AtualizarFuncionarioPorCPF(decimal salario, string cpf)
        {
            Funcionario funcionario = BuscarFuncionarioPorCPF(cpf) ?? throw new Exception("Funcionário não encontrado.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(Funcionario.UPDATEFUNCIONARIOPORCPF, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Salario", salario);
                            command.Parameters.AddWithValue("@idFuncionario", funcionario.FuncionarioID);

                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao atualziar funcionário: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao atualziar funcionário: " + e.Message);
                    }
                }
            }
        }

        public void DeletarFuncionarioPorCPF(string cpf)
        {

            Funcionario funcionario = BuscarFuncionarioPorCPF(cpf) ?? throw new Exception("Funcionário não encontrado.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(Funcionario.DELETEFUNCIONARIOPORCPF, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idFuncionario", funcionario.FuncionarioID);

                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao deletar funcionário: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao deletar funcionário: " + e.Message);
                    }
                }
            }
        }


    }
}
