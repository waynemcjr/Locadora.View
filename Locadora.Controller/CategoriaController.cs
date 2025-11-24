using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Databases;

namespace Locadora.Controller
{
    public class CategoriaController
    {

        public void AdicionarCategoria(Categoria categoria)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using SqlCommand command = new(Categoria.INSERTCATEGORIA, connection, transaction);
                        command.Parameters.AddWithValue("@Nome", categoria.Nome);
                        command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
                        command.Parameters.AddWithValue("@Diaria", categoria.Diaria);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao adicionar categoria: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao adicionar categoria: " + e.Message);
                    }
                }
            }


        }

        public Categoria BuscarCategoriaPorID(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(Categoria.SELECTCATEGORIAPORID, connection, transaction);
                        command.Parameters.AddWithValue("@idCategoria", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                var categoria = new Categoria(reader["Nome"].ToString(),
                                                          reader["Descricao"] != DBNull.Value ?
                                                          reader["Descricao"].ToString() : null,
                                                          (Decimal)reader["Diaria"]);

                                categoria.SetCategoriaID(id);

                                reader.Close();
                                transaction.Commit();
                                return categoria;
                            }

                        }
                        return null;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao buscar categoria: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao buscar categoria: " + e.Message);
                    }
                }
            }
            return null;
        }

        public List<Categoria> ListarTodasCategorias()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(Categoria.SELECTTODASCATEGORIAS, connection, transaction);
                        SqlDataReader reader = command.ExecuteReader();

                        List<Categoria> categorias = [];

                        while (reader.Read())
                        {

                            var categoria = new Categoria(reader["Nome"].ToString(),
                                                          reader["Descricao"] != DBNull.Value ?
                                                          reader["Descricao"].ToString() : null,
                                                          (Decimal)reader["Diaria"]);

                            categorias.Add(categoria);
                        }
                        reader.Close();
                        transaction.Commit();
                        return categorias;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao listar categorias: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao listar categorias: " + e.Message);
                    }
                }
            }
        }

        public Categoria ListarVeiculosPorCategoria(int idCategoria)
        {
            var categoriaEncontrada = BuscarCategoriaPorID(idCategoria) ?? throw new Exception("Categoria não encontrada.");

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(Categoria.SELECTVEICULOSPORCATEGORIAID, connection, transaction);

                        command.Parameters.AddWithValue("idCategoria", idCategoria);

                        SqlDataReader reader = command.ExecuteReader();
                        Categoria categoria1 = null;

                        while (reader.Read())
                        {
                            if (categoria1 is null)
                            {
                                categoria1 = new Categoria(reader["Nome"].ToString(),
                                                          reader["Descricao"] != DBNull.Value ?
                                                          reader["Descricao"].ToString() : null,
                                                          (Decimal)reader["Diaria"]);
                            }


                            categoria1.SetVeiculos(new Veiculo((int)reader["CategoriaID"],
                                                               reader["Placa"].ToString(),
                                                               reader["Marca"].ToString(),
                                                               reader["Modelo"].ToString(),
                                                               (int)reader["Ano"],
                                                               reader["StatusVeiculo"].ToString()
                                                            ));
                        }
                        reader.Close();
                        transaction.Commit();
                        return categoria1;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao listar categorias: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao listar categorias: " + e.Message);
                    }
                }
            }
            Categoria categoria = null;

        }

        public void AtualizarCategoriaPorID(int id, Categoria categoria)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    var categoriaExistente = BuscarCategoriaPorID(id) ?? throw new Exception("Categoria não encontrada!");

                    try
                    {
                        using SqlCommand command = new(Categoria.UPDATECATEGORIA, connection, transaction);

                        command.Parameters.AddWithValue("@CategoriaID", categoria.CategoriaID);
                        command.Parameters.AddWithValue("@Nome", categoria.Nome);

                        if (categoria.Descricao is null)
                            command.Parameters.AddWithValue("@Descricao", "");
                        else
                            command.Parameters.AddWithValue("@Descricao", categoria.Descricao);
                        command.Parameters.AddWithValue("@Diaria", categoria.Diaria);

                        command.Parameters.AddWithValue("@idCategoria", id);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao atualizar categoria: " + ex.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro inesperado ao atualizar categoria: " + e.Message);
                    }
                }
            }
        }

        public void DeletarCategoriaPorID(int id, SqlConnection connection, SqlTransaction transaction)
        {

            var categoriaExistente = BuscarCategoriaPorID(id) ?? throw new Exception("Categoria não encontrada!");

            try
            {
                using (SqlCommand command = new SqlCommand(Categoria.DELETECATEGORIA, connection, transaction))
                {

                    command.Parameters.AddWithValue("@idCategoria", id);
                    command.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao deletar categoria: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao deletar categoria: " + e.Message);
            }
        }
    }
}