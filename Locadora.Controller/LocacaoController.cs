using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Databases;

namespace Locadora.Controller
{
    public class LocacaoController : ILocacaoController
    {

        public ClienteController clienteController = new();

        public VeiculoController veiculoController = new();

        public FuncionarioController funcionarioController = new();

        public CategoriaController categoriaController = new();

        public void AdicionarLocacao(Locacao locacao)
        {
            var clienteEncontrado = clienteController.BuscarClientePorID(locacao.ClienteID) ?? throw new Exception("Cliente não encontrado.");
            var veiculoEncontrado = veiculoController.BuscarVeiculoPorID(locacao.VeiculoID) ?? throw new Exception("Veículo não encontrado.");

            Decimal diaria = categoriaController.BuscarCategoriaPorID(veiculoController.BuscarVeiculoPorID(locacao.VeiculoID).CategoriaID).Diaria;

            if (veiculoEncontrado.StatusVeiculo != "Disponível")
                throw new Exception("Veículo está indisponível.");

            if (locacao.Status.ToString() != "Ativa")
                throw new Exception("Locação já está finalizada.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(Locacao.INSERTLOCACAO, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idCliente", locacao.ClienteID);
                            command.Parameters.AddWithValue("@idVeiculo", locacao.VeiculoID);
                            command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                            command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                            command.Parameters.AddWithValue("@DataDevolucaoReal", (object?)locacao.DataDevolucaoReal ?? DBNull.Value);
                            command.Parameters.AddWithValue("@ValorDiaria", diaria);
                            command.Parameters.AddWithValue("@ValorTotal", locacao.DiasParaRetornar * diaria);
                            command.Parameters.AddWithValue("@Multa", locacao.Multa);
                            command.Parameters.AddWithValue("@Status", locacao.Status.ToString());

                            command.ExecuteNonQuery();
                            veiculoController.AtualizarVeiculo("Alugado", veiculoEncontrado.Placa);
                            transaction.Commit();
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao alocar veículo: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao alocar veículo: " + e.Message);
                    }
                }
            }
        }

        public Locacao BuscarLocacaoPorId(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Locacao.SELECTLOCACAOPORID, connection))
                    {

                        command.Parameters.AddWithValue("@idLocacao", id);
                        Locacao locacao = null;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Funcionario> funcionarios = [];
                            while (reader.Read())
                            {
                                if (locacao == null)
                                {
                                    locacao = new Locacao((int)reader["LocacaoID"],
                                                              (int)reader["ClienteID"],
                                                              (int)reader["VeiculoID"],
                                                              Convert.ToDateTime(reader["DataLocacao"]),
                                                              reader["DataDevolucaoReal"] != (object)DBNull.Value ?
                                                              Convert.ToDateTime(reader["DataDevolucaoReal"]) : null,
                                                              Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                                              Convert.ToDecimal(reader["ValorDiaria"]),
                                                              Convert.ToDecimal(reader["ValorTotal"]),
                                                              Convert.ToDecimal(reader["Multa"]),
                                                              (EStatusLocacao)Enum.Parse(typeof(EStatusLocacao), reader["Status"].ToString())
                                                             );

                                    if (locacao == null)
                                        return null;

                                    Cliente cliente = clienteController.BuscarClientePorID(locacao.ClienteID);
                                    locacao.SetClienteNome(cliente.Nome);
                                    locacao.SetClienteEmail(cliente.Email);

                                    Veiculo veiculo = veiculoController.BuscarVeiculoPorID(locacao.VeiculoID);
                                    locacao.SetVeiculoModelo(veiculo.Modelo);
                                    locacao.SetVeiculoPlaca(veiculo.Placa);

                                }
                                if (reader["CPF"] != DBNull.Value)
                                {
                                    funcionarios.Add(funcionarioController.BuscarFuncionarioPorCPF(reader["CPF"].ToString()));
                                }

                                locacao.SetFuncionarios(funcionarios);
                            }
                            return locacao;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar locação: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao buscar locação: " + e.Message);
                }
            }
        }

        public List<Funcionario> ListarFuncionariosDeUmaLocacao(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Locacao.SELECTFUNCIONARIOSDEUMALOCACAO, connection))
                    {

                        command.Parameters.AddWithValue("@idLocacao", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Funcionario> funcionarios = [];
                            while (reader.Read())
                            {
                                var funcionario = new Funcionario(reader["Nome"].ToString(),
                                                                  reader["CPF"].ToString(),
                                                                  reader["Email"].ToString(),
                                                                  reader["Salario"] != (object)DBNull.Value ?
                                                                  (Decimal)reader["Salario"] : null
                                                                 );
                                funcionarios.Add(funcionario);
                            }
                            return funcionarios;

                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar funcionários da locação: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao listar funcionários da locação: " + e.Message);
                }
            }
        }

        public void AssociarFuncionario(string cpf, int idLocacao)
        {
            var funcionarioEncontrado = funcionarioController.BuscarFuncionarioPorCPF(cpf) ?? throw new Exception("Funcionário não encontrado.");
            var locacaoEncontrada = BuscarLocacaoPorId(idLocacao) ?? throw new Exception("Locação não encontrada.");


            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                        using (SqlCommand command = new SqlCommand(Locacao.INSERTLOCACAOFUNCIONARIO, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idLocacao", idLocacao);
                            command.Parameters.AddWithValue("@idFuncionario", funcionarioEncontrado.FuncionarioID);
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao associar funcionário à locação: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao associar funcionário à locação: " + e.Message);
                    }
                }

            }
        }

        public void FinalizarLocacao(int idLocacao)
        {
            var locacaoEncontrada = BuscarLocacaoPorId(idLocacao) ?? throw new Exception("Locação não encontrada.");
            var veiculoEncontrado = veiculoController.BuscarVeiculoPorID(locacaoEncontrada.VeiculoID);

            if (locacaoEncontrada.Status.ToString() != "Ativa")
                throw new Exception("Locação já está finalizada.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(Locacao.UPDATELOCACAOPORID, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@idLocacao", idLocacao);
                            command.ExecuteNonQuery();
                            veiculoController.AtualizarVeiculo("Disponível", veiculoEncontrado.Placa);
                            transaction.Commit();
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao finalizar locação: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao finalizar locação: " + e.Message);
                    }
                }
            }
        }

        public List<Locacao> ListarLocacaoPorCliente(string email)
        {
            var clienteEncontrado = clienteController.BuscarClientePorEmail(email) ?? throw new Exception("Cliente não encontrado.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Locacao.SELECTLOCACAOPORCLIENTE, connection))
                    {

                        command.Parameters.AddWithValue("@idCliente", clienteEncontrado.ClienteID);
                        Dictionary<int, Locacao> locacoesDict = new();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int locacaoID = (int)reader["LocacaoID"];

                                if (!locacoesDict.TryGetValue(locacaoID, out Locacao locacao))
                                {
                                    locacao = new Locacao(
                                        (int)reader["LocacaoID"],
                                        (int)reader["ClienteID"],
                                        (int)reader["VeiculoID"],
                                        Convert.ToDateTime(reader["DataLocacao"]),
                                        reader["DataDevolucaoReal"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["DataDevolucaoReal"]) : null,
                                        Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                        Convert.ToDecimal(reader["ValorDiaria"]),
                                        Convert.ToDecimal(reader["ValorTotal"]),
                                        Convert.ToDecimal(reader["Multa"]),
                                        (EStatusLocacao)Enum.Parse(typeof(EStatusLocacao), reader["Status"].ToString())
                                    );

                                    locacao.SetClienteNome(clienteEncontrado.Nome);
                                    locacao.SetClienteEmail(clienteEncontrado.Email);

                                    Veiculo veiculo = veiculoController.BuscarVeiculoPorID(locacao.VeiculoID);
                                    locacao.SetVeiculoModelo(veiculo.Modelo);
                                    locacao.SetVeiculoPlaca(veiculo.Placa);

                                    locacao.SetFuncionarios(new List<Funcionario>());

                                    locacoesDict.Add(locacaoID, locacao);
                                }

                                if (reader["CPF"] != DBNull.Value)
                                {
                                    var funcionario = funcionarioController.BuscarFuncionarioPorCPF(reader["CPF"].ToString());
                                    locacao.Funcionarios.Add(funcionario);
                                }
                            }
                        }

                        return locacoesDict.Values.ToList();

                    }


                }


                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar locação por cliente: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao listar locação por cliente: " + e.Message);
                }
            }
        }

        public List<Locacao> ListarLocacaoPorFuncionario(string cpf)
        {
            var funcionarioEncontrado = funcionarioController.BuscarFuncionarioPorCPF(cpf) ?? throw new Exception("Funcionário não encontrado.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Locacao.SELECTLOCACAOPORFUNCIONARIO, connection))
                    {

                        command.Parameters.AddWithValue("@idFuncionario", funcionarioEncontrado.FuncionarioID);
                        Dictionary<int, Locacao> locacoesDict = new();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int locacaoID = (int)reader["LocacaoID"];

                                if (!locacoesDict.TryGetValue(locacaoID, out Locacao locacao))
                                {
                                    locacao = new Locacao(
                                        (int)reader["LocacaoID"],
                                        (int)reader["ClienteID"],
                                        (int)reader["VeiculoID"],
                                        Convert.ToDateTime(reader["DataLocacao"]),
                                        reader["DataDevolucaoReal"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["DataDevolucaoReal"]) : null,
                                        Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                        Convert.ToDecimal(reader["ValorDiaria"]),
                                        Convert.ToDecimal(reader["ValorTotal"]),
                                        Convert.ToDecimal(reader["Multa"]),
                                        (EStatusLocacao)Enum.Parse(typeof(EStatusLocacao), reader["Status"].ToString())
                                    );

                                    Cliente cliente = clienteController.BuscarClientePorID(locacao.ClienteID);
                                    locacao.SetClienteNome(cliente.Nome);
                                    locacao.SetClienteEmail(cliente.Email);

                                    Veiculo veiculo = veiculoController.BuscarVeiculoPorID(locacao.VeiculoID);
                                    locacao.SetVeiculoModelo(veiculo.Modelo);
                                    locacao.SetVeiculoPlaca(veiculo.Placa);

                                    locacao.SetFuncionarios(new List<Funcionario>());

                                    locacoesDict.Add(locacaoID, locacao);
                                }

                                if (reader["CPF"] != DBNull.Value)
                                {
                                    var funcionario = funcionarioController.BuscarFuncionarioPorCPF(reader["CPF"].ToString());
                                    locacao.Funcionarios.Add(funcionario);
                                }
                            }
                        }

                        return locacoesDict.Values.ToList();

                    }


                }


                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar locação por funcionário: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao listar locação por funcionário: " + e.Message);
                }
            }
        }

        public List<Locacao> ListarLocacoesAivas()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Locacao.SELECTLOCAOESATIVAS, connection))
                    {
                        Dictionary<int, Locacao> locacoesDict = new();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int locacaoID = (int)reader["LocacaoID"];

                                if (!locacoesDict.TryGetValue(locacaoID, out Locacao locacao))
                                {
                                    locacao = new Locacao(
                                        (int)reader["LocacaoID"],
                                        (int)reader["ClienteID"],
                                        (int)reader["VeiculoID"],
                                        Convert.ToDateTime(reader["DataLocacao"]),
                                        reader["DataDevolucaoReal"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["DataDevolucaoReal"]) : null,
                                        Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                        Convert.ToDecimal(reader["ValorDiaria"]),
                                        Convert.ToDecimal(reader["ValorTotal"]),
                                        Convert.ToDecimal(reader["Multa"]),
                                        (EStatusLocacao)Enum.Parse(typeof(EStatusLocacao), reader["Status"].ToString())
                                    );

                                    Cliente cliente = clienteController.BuscarClientePorID(locacao.ClienteID);
                                    locacao.SetClienteNome(cliente.Nome);
                                    locacao.SetClienteEmail(cliente.Email);

                                    Veiculo veiculo = veiculoController.BuscarVeiculoPorID(locacao.VeiculoID);
                                    locacao.SetVeiculoModelo(veiculo.Modelo);
                                    locacao.SetVeiculoPlaca(veiculo.Placa);

                                    locacao.SetFuncionarios(new List<Funcionario>());

                                    locacoesDict.Add(locacaoID, locacao);
                                }

                                if (reader["CPF"] != DBNull.Value)
                                {
                                    var funcionario = funcionarioController.BuscarFuncionarioPorCPF(reader["CPF"].ToString());
                                    locacao.Funcionarios.Add(funcionario);
                                }
                            }
                        }

                        return locacoesDict.Values.ToList();

                    }


                }


                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar locação: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao buscar locação: " + e.Message);
                }
            }
        }

        public List<Locacao> ListarTodasLocacoes()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(Locacao.SELECTTODASLOCACOES, connection))
                    {
                        Dictionary<int, Locacao> locacoesDict = new();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int locacaoID = (int)reader["LocacaoID"];

                                if (!locacoesDict.TryGetValue(locacaoID, out Locacao locacao))
                                {
                                    locacao = new Locacao(
                                        (int)reader["LocacaoID"],
                                        (int)reader["ClienteID"],
                                        (int)reader["VeiculoID"],
                                        Convert.ToDateTime(reader["DataLocacao"]),
                                        reader["DataDevolucaoReal"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["DataDevolucaoReal"]) : null,
                                        Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                                        Convert.ToDecimal(reader["ValorDiaria"]),
                                        Convert.ToDecimal(reader["ValorTotal"]),
                                        Convert.ToDecimal(reader["Multa"]),
                                        (EStatusLocacao)Enum.Parse(typeof(EStatusLocacao), reader["Status"].ToString())
                                    );

                                    Cliente cliente = clienteController.BuscarClientePorID(locacao.ClienteID);
                                    locacao.SetClienteNome(cliente.Nome);
                                    locacao.SetClienteEmail(cliente.Email);

                                    Veiculo veiculo = veiculoController.BuscarVeiculoPorID(locacao.VeiculoID);
                                    locacao.SetVeiculoModelo(veiculo.Modelo);
                                    locacao.SetVeiculoPlaca(veiculo.Placa);

                                    locacao.SetFuncionarios(new List<Funcionario>());

                                    locacoesDict.Add(locacaoID, locacao);
                                }

                                if (reader["CPF"] != DBNull.Value)
                                {
                                    var funcionario = funcionarioController.BuscarFuncionarioPorCPF(reader["CPF"].ToString());
                                    locacao.Funcionarios.Add(funcionario);
                                }
                            }
                        }

                        return locacoesDict.Values.ToList();

                    }


                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar locações: " + ex.Message);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao listar locações: " + e.Message);
                }
            }
        }
    }
}