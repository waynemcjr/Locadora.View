using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;

Cliente cliente = new Cliente("Novo cliente com doc", "docdoc10@emailemail.com.br");
Documento documento = new Documento("RG", "123459999", new DateOnly(2020, 1, 1), new DateOnly(2030, 1, 1));

//Console.WriteLine(cliente);

var clienteContoller = new ClienteController();

try
{
    clienteContoller.AdicionarCliente(cliente, documento);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

//try
//{
//    var listadeClientes = clienteContoller.ListarTodosClientes();

//    foreach (var clientedaLista in listadeClientes)
//    {
//        Console.WriteLine(clientedaLista);
//    }
//}
//catch(Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

//clienteContoller.AtualizarTelefoneCliente("99999-9999", "novoteste@emailemail.com.br");
//Console.WriteLine(clienteContoller.BuscaClientePorEmail("novoteste@emailemail.com.br"));

//try
//{
//    clienteContoller.DeletarCliente("novo@emailemail.com.br");
//    Console.WriteLine("Cliente deletado com sucesso!");
//}
//catch(Exception ex)
//{
//    Console.WriteLine("Erro ao deletar cliente na program " + ex.Message);
//}

