using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Documento
    {
        public readonly static string INSERTDOCUMENTO = "INSERT INTO tblDocumentos(ClienteID,TipoDocumento,Numero,DataEmissao,DataValidade) " +
                                                                     "VALUES (@ClienteID,@TipoDocumento,@Numero,@DataEmissao,@DataValidade);";

        public static readonly string UPDATEDOCUMENTO = @"UPDATE tblDocumentos
                                                          SET TipoDocumento = @TipoDocumento,
                                                              Numero = @Numero,
                                                              DataEmissao = @DataEmissao,
                                                              DataValidade = @DataValidade
                                                          WHERE ClienteID = @idCliente;";

        public int DoumentoID { get; private set; }

        public int ClienteID { get; private set; }

        public string TipoDocumento { get; private set; }

        public string Numero { get; private set; }

        public DateTime DataEmissao { get; private set; }

        public DateTime DataValidade { get; private set; }

        public Documento(string tipoDocumento, string numero, DateTime dataEmissao, DateTime dataValidade)
        {
            this.TipoDocumento = tipoDocumento;
            this.Numero = numero;
            this.DataEmissao = dataEmissao;
            this.DataValidade = dataValidade;
        }

        public void SetClienteID(int clienteID)
        {
            this.ClienteID = clienteID;
        }

        public override string ToString()
        {
            return $"Tipo: {this.TipoDocumento}\n" +
                $"Numero: {this.Numero}\n" +
                $"Data de Emissão: {this.DataEmissao}\n" +
                $"Data de Validade: {this.DataValidade}";
        }

    }
}
