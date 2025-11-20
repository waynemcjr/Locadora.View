using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;

namespace Locadora.Controller
{
    public class DocumentoController
    {
        public void AdicionarDocumento(Documento documento, SqlConnection connection, SqlTransaction _transaction)
        {
            using (_transaction)
            {
                try
                {
                    SqlCommand command = new SqlCommand(Documento.INSERTDOCUMENTO, connection, _transaction);

                    command.Parameters.AddWithValue("@ClienteID", documento.ClienteID);
                    command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
                    command.Parameters.AddWithValue("@Numero", documento.Numero);
                    command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                    command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                    command.ExecuteNonQuery();

                    _transaction.Commit();
                }
                catch (SqlException ex)
                {
                    _transaction.Rollback();
                    throw new Exception("Erro ao adicionar documento " + ex.Message);
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar documento " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}