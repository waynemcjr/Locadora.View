using Locadora.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using Utils.Databases;

namespace Locadora.Controller
{
    public class ClienteController
    {
        private DocumentoController documentoController = new DocumentoController();
        public void AdicionarCliente(Cliente cliente, Documento documento)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    var command = new SqlCommand(Cliente.INSERTCLIENTE, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? (object)DBNull.Value);

                    int clienteId = Convert.ToInt32(command.ExecuteScalar());
                    cliente.SetClienteID(clienteId);

                    
                    var documentoController = new DocumentoController();
                    documento.SetClienteID(clienteId);

                    documentoController.AdicionarDocumento(documento,connection,transaction);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar cliente: " + ex.Message);

                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar cliente: " + e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public Cliente BuscarClientePorEmail(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {

                SqlCommand command = new SqlCommand(Cliente.SELECTCLIENTEPOREMAIL, connection);
                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    var cliente = new Cliente(reader["Nome"].ToString(),
                                              reader["Email"].ToString(),
                                              reader["Telefone"] != DBNull.Value ?
                                              reader["Telefone"].ToString() : null);
                    cliente.SetClienteID((int)reader["ClienteID"]);

                    var documento = new Documento(reader["TipoDocumento"].ToString(),
                                                  reader["Numero"].ToString(),
                                                  (DateTime)reader["DataEmissao"],
                                                  (DateTime)reader["DataValidade"]);

                    cliente.SetDocumento(documento);
                    return cliente;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar cliente:" + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao buscar cliente:" + e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Cliente BuscarClientePorID(int id)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {

                SqlCommand command = new SqlCommand(Cliente.SELECTCLIENTEPORID, connection);
                command.Parameters.AddWithValue("@idCliente", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    var cliente = new Cliente(reader["Nome"].ToString(),
                                              reader["Email"].ToString(),
                                              reader["Telefone"] != DBNull.Value ?
                                              reader["Telefone"].ToString() : null);
                    cliente.SetClienteID((int)reader["ClienteID"]);

                    var documento = new Documento(reader["TipoDocumento"].ToString(),
                                                  reader["Numero"].ToString(),
                                                  (DateTime)reader["DataEmissao"],
                                                  (DateTime)reader["DataValidade"]);

                    cliente.SetDocumento(documento);
                    return cliente;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar cliente:" + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao buscar cliente:" + e.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        public List<Cliente> ListarTodosClientes()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            try
            {

                connection.Open();

                var command = new SqlCommand(Cliente.SELECTTODOSCLIENTES, connection);

                SqlDataReader reader = command.ExecuteReader();

                List<Cliente> clientes = [];

                while (reader.Read())
                {
                    Cliente cliente = new Cliente(
                        reader["Nome"].ToString(),
                        reader["Email"].ToString(),
                        reader["Telefone"] != DBNull.Value ?
                        reader["Telefone"].ToString() : null);


                    var documento = new Documento(reader["TipoDocumento"].ToString(),
                                                  reader["Numero"].ToString(),
                                                  (DateTime)reader["DataEmissao"],
                                                  (DateTime)reader["DataValidade"]);

                    cliente.SetDocumento(documento);

                    clientes.Add(cliente);
                }

                return clientes;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar clientes: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao listar clientes:" + e.Message);
            }
            finally
            {
                connection.Close();
            }

        }



        public void AtualizarTelefoneCliente(string telefone, string email)
        {
            var clienteEncontrado = this.BuscarClientePorEmail(email) ?? throw new Exception("Cliente não encontrado.");

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Cliente.UPDATETELEFONECLIENTE, connection);
                command.Parameters.AddWithValue("@Telefone", telefone);
                command.Parameters.AddWithValue("@IdCliente", clienteEncontrado.ClienteID);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar telefone do cliente: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao atualizar telefone do cliente: " + e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void AtualizarDocumentoCliente(Documento documento,string email)
        {
            var clienteEncontrado = BuscarClientePorEmail(email) ??
                throw new Exception("Cliente não encontrado.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using(SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        documento.SetClienteID(clienteEncontrado.ClienteID);
                        DocumentoController documentoController = new DocumentoController();

                        documentoController.AtualizarDocumento(documento,connection,transaction);
                        transaction.Commit();

                    }catch(SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao atualizar documento do cliente: " + ex.Message);
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao atualizar documento do cliente: " + e.Message);
                    }
                }
            }
        }

        public void DeletarCliente(string email)
        {
            var clienteEncontrado = this.BuscarClientePorEmail(email);

            if (clienteEncontrado is null)
            {
                throw new Exception("Cliente não encontrado.");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();
                try
                {
                    SqlCommand command = new SqlCommand(Cliente.DELETECLIENTEPOREMAIL,connection);
                    command.Parameters.AddWithValue("@idCliente", clienteEncontrado.ClienteID);
                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao deletar cliente: " + ex.Message);

                }
                catch (Exception e)
                {
                    throw new Exception("Erro inesperado ao deletar o cliente: " + e.Message);

                }
            }
                
        }
    }
}
