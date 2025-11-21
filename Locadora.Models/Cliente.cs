using System.Runtime.CompilerServices;

namespace Locadora.Models
{
    public class Cliente
    {
        public readonly static string INSERTCLIENTE = "INSERT INTO tblClientes VALUES(@Nome, @Email, @Telefone); SELECT SCOPE_IDENTITY()";

        public readonly static string SELECTALLCLIENTES = "SELECT c.Nome,c.Email, c.Telefone, d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade FROM tblClientes c JOIN tblDocumentos d ON d.ClienteID = c.ClienteID";

        public readonly static string UPDATEFONECLIENTE = "UPDATE tblClientes SET Telefone = @Telefone WHERE ClienteID = @idCliente";

        public readonly static string SELECTCLIENTEPOREMAIL = "SELECT c.ClienteID, c.Nome,c.Email, c.Telefone, d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade FROM tblClientes c JOIN tblDocumentos d ON d.ClienteID = c.ClienteID WHERE Email = @Email";

        public readonly static string DELETECLIENTE = "DELETE FROM tblClientes WHERE ClienteID = @ClienteId";


        public Documento Documento { get; private set; }
        public int ClienteID { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string? Telefone { get; private set; } = String.Empty;

        public Cliente(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public Cliente(string nome, string email, string? telefone) : this(nome, email)
        {
            Telefone = telefone;
        }

        public override string? ToString()
        {
            return $"Nome: {Nome}\nEmail: {Email}\nTelefone: {Telefone}\n" + this.Documento;
        }

        public void setClienteID(int clienteID)
        {
            ClienteID = clienteID;
        }

        public void setTelefone(string telefone)
        {
            Telefone = telefone;
        }
        public void setDocumento(Documento documento)
        {
            Documento = documento;
        }
    }
}