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
    public class DocumentoController
    {

        public void AdicionarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                var command = new SqlCommand(Documento.INSERTDOCUMENTO, connection,transaction);

                command.Parameters.AddWithValue("@ClienteID", documento.ClienteID);
                command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
                command.Parameters.AddWithValue("@Numero", documento.Numero);
                command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao adicionar documento: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao adicionar documento: " + e.Message);
            }
        }

        public void AtualizarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                var command = new SqlCommand(Documento.UPDATEDOCUMENTO, connection, transaction);

                command.Parameters.AddWithValue("@idCliente", documento.ClienteID);
                command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
                command.Parameters.AddWithValue("@Numero", documento.Numero);
                command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar documento: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Erro inesperado ao atualizar documento: " + e.Message);
            }
        }





    }
}
