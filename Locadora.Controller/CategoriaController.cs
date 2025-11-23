using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Utils.Databases;

namespace Locadora.Controller
{
    public class CategoriaController
    {
        public void AdicionarCategoria(Categoria categoria)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.INSERTCATEGORIA, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", categoria.Nome);
                    command.Parameters.AddWithValue("@Descricao", categoria.Descricao ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Diaria", categoria.Diaria);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    categoria.setCategoriaID(id);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao adicionar categoria " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro inesperado ao adicionar categoria " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Categoria> ListarTodasCategorias()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();
            try
            {
                var command = new SqlCommand(Categoria.SELECTALLCATEGORIA, connection);

                SqlDataReader reader = command.ExecuteReader();

                List<Categoria> listaCategoria = new List<Categoria>();

                while (reader.Read())
                {
                    var categoria = new Categoria(
                                                    reader["Nome"].ToString(),
                                                    reader["Descricao"] != DBNull.Value ?
                                                    reader["Descricao"].ToString() : null,
                                                    Convert.ToDecimal(reader["Diaria"])
                                                 );
                    categoria.setCategoriaID(Convert.ToInt32(reader["CategoriaID"]));

                    listaCategoria.Add(categoria);
                }
                return listaCategoria;
            }
            catch (SqlException ex)
            {
                throw new Exception("Não foi possível listar todas categorias " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar todas categorias " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Categoria BuscarCategoriaPorNome(string nome)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                var command = new SqlCommand(Categoria.BUSCARCATEGORIAPORNOME, connection);

                command.Parameters.AddWithValue("@Nome", nome);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var categoria = new Categoria(
                                                    reader["Nome"].ToString(),
                                                    reader["Descricao"] != DBNull.Value ?
                                                    reader["Descricao"].ToString() : null,
                                                    Convert.ToDecimal(reader["Diaria"])
                                                 );
                    categoria.setCategoriaID(Convert.ToInt32(reader["CategoriaID"]));
                    return categoria;
                }
                else
                    return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar categoria " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void AtualizarDiariaCategoria(string nome, decimal diaria)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            var categoriaEncontrada = this.BuscarCategoriaPorNome(nome);
            categoriaEncontrada.setDiaria(diaria);

            if (categoriaEncontrada is null)
                throw new Exception("Não foi encontrado uma categoria com esse nome!");

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    var command = new SqlCommand(Categoria.UPDATECATEGORIA, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", categoriaEncontrada.Nome);
                    command.Parameters.AddWithValue("@Diaria", categoriaEncontrada.Diaria);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao atualizar categoria " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro inesperado ao atualizar categoria " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void DeletarCategoria(string nome)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            var categoriaEncontrada = this.BuscarCategoriaPorNome(nome);

            if (categoriaEncontrada is null)
                throw new Exception("Não foi possível encontrar essa categoria!");

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    var command = new SqlCommand(Categoria.DELETECATEGORIA, connection, transaction);

                    command.Parameters.AddWithValue("@IdCategoria", categoriaEncontrada.CategoriaID);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao deletar categoria " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro inesperado ao deletar categoria " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public string BuscarNomeCategoriaPorId(int id)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Categoria.SELECTNOMECATEGORIAPORID, connection);
                command.Parameters.AddWithValue("@Id", id);

                string nomecategoria = String.Empty;

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    nomecategoria = reader["Nome"].ToString() ?? string.Empty;
                }
                return nomecategoria;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar categoria." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria." + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
