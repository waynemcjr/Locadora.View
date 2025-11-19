using Locadora.Controller;
using Locadora.Models;

//Cliente cliente = new Cliente("Novo cliente de teste", "novoteste@emailemail.com.br");
//Documento documento = new Documento(1, "RG", "123456789", new DateOnly(2020, 1, 1), new DateOnly(2030, 1, 1));

//Console.WriteLine(cliente);

var clienteContoller = new ClienteContoller();

//try
//{
//clienteContoller.AdicionarCliente(cliente);
//}
//catch(Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

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

clienteContoller.AtualizarTelefoneCliente("99999-9999", "novoteste@emailemail.com.br");
Console.WriteLine(clienteContoller.BuscaClientePorEmail("novoteste@emailemail.com.br"));

