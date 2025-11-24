using System.Data;

namespace Locadora.Models
{
    public class Cliente
    {
        public readonly static string INSERTCLIENTE = "INSERT INTO tblClientes VALUES (@Nome,@Email,@Telefone); " +
                                                       "SELECT SCOPE_IDENTITY();";

        public readonly static string SELECTTODOSCLIENTES = @"SELECT c.Nome, c.Email, c.Telefone,
                                                                     d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade
                                                              FROM tblClientes c
                                                              JOIN tblDocumentos d ON c.ClienteID = d.ClienteID;";

        public readonly static string UPDATETELEFONECLIENTE = "UPDATE tblClientes SET Telefone = @Telefone WHERE ClienteID = @idCliente;";

        public readonly static string SELECTCLIENTEPOREMAIL = @"SELECT c.ClienteID,c.Nome, c.Email, c.Telefone,
                                                                       d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade
                                                              FROM tblClientes c
                                                              JOIN tblDocumentos d ON c.ClienteID = d.ClienteID
                                                              WHERE c.Email = @Email;";

        public readonly static string SELECTCLIENTEPORID = @"SELECT c.ClienteID,c.Nome, c.Email, c.Telefone,
                                                                    d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade
                                                              FROM tblClientes c
                                                              JOIN tblDocumentos d ON c.ClienteID = d.ClienteID
                                                              WHERE c.ClienteID = @idCliente;";

        public readonly static string DELETECLIENTEPOREMAIL = @"DELETE FROM tblDocumentos WHERE ClienteID = @idCliente;
                                                                DELETE FROM tblClientes WHERE ClienteID = @idCliente;";
    
        public int ClienteID { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string? Telefone { get; private set; } = String.Empty;

        public Documento Documento { get; private set; }

        public Cliente(string nome, string email)
        {
            this.Nome = nome;
            this.Email = email;
        }

        public Cliente(string nome, string email, string? telefone) : this(nome, email)
        {
            this.Telefone = telefone;
        }

        public void SetClienteID(int id)
        {
            this.ClienteID = id;
        }

        public void SetDocumento(Documento documento)
        {
            Documento = documento;
        }

        public void SetTelefone(string telefone)
        {
            this.Telefone = telefone;
        }

        public override string ToString()
        {
            return $"Nome: {this.Nome}\n" +
                $"Email: {this.Email}\n" +
                $"Telefone: {this.Telefone}\n" +
                $"{this.Documento.TipoDocumento}: {this.Documento.Numero} => Emissão: {DateOnly.FromDateTime(this.Documento.DataEmissao)} - Validade: {DateOnly.FromDateTime(this.Documento.DataValidade)}";
        }

    }
}
