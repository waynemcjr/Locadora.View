using Locadora.Controller;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;

#region Testes Clientes
//Cliente cliente = new Cliente("Novo cliente com doc", "docdocdoc10@emailemail.com.br");
//Documento documento = new Documento("CIN", "1122334455", new DateOnly(2025, 1, 1), new DateOnly(2035, 1, 1));

//Console.WriteLine(cliente);

var clienteContoller = new ClienteController();

//try
//{
//    clienteContoller.AdicionarCliente(cliente, documento);
//}
//catch (Exception ex)
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
//catch (Exception ex)
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

//try
//{
//    clienteContoller.AtualizarDocumentoCliente("docdocdoc10@emailemail.com.br", documento);
//    Console.WriteLine(clienteContoller.BuscaClientePorEmail("docdocdoc10@emailemail.com.br"));
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
#endregion

#region Testes Categoria

//var categoria = new Categoria("Luxo premium", "categoria exclusiva de luxo", 500);

var categoriaController = new CategoriaController();

//categoriaController.AdicionarCategoria(categoria);

//var listaCategoria = categoriaController.ListarTodasCategorias();

//foreach( var categoria in listaCategoria)
//{
//    Console.WriteLine(categoria);
//    Console.WriteLine("-----------------");
//}

//var categoria = categoriaController.BuscarCategoriaPorNome("SUV");
//Console.WriteLine(categoria);

//categoriaController.AtualizarDiariaCategoria("SUV", 99);
//var categoria = categoriaController.BuscarCategoriaPorNome("SUV");
//Console.WriteLine(categoria);

//categoriaController.DeletarCategoria("Luxo premium");

#endregion

#region Teste Veiculos

var veiculoController = new VeiculoController();
//try
//{
//    var veiculo = new Veiculo(1, "ABX-1234", "Civic", "Honda", 2025, EStatusVeiculo.Disponivel.ToString());
//    veiculoController.AdicionarVeiculo(veiculo);
//    Console.WriteLine(veiculo);
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro ao criar veículo :" + ex.Message);
//}

//try
//{
//    var veiculos = veiculoController.ListarTodosVeiculos();
//    foreach (var veiculo in veiculos)
//    {
//        Console.WriteLine(veiculo);
//    }
//}
//catch(Exception ex)
//{
//    Console.WriteLine("Erro ao exibir a lista " + ex.Message);
//}

//try
//{
//    var v = veiculoController.BuscarVeiculoPlaca("GHI9012");
//    Console.WriteLine(v);
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro ao procurar " + ex.Message);
//}

//try
//{
//    var veiculo = veiculoController.BuscarVeiculoPlaca("GHI9012");
//    veiculoController.DeletarVeiculo(veiculo.VeiculoID);
//}
//catch(Exception ex)
//{
//    Console.WriteLine("Erro " + ex.Message);
//}

//try
//{
//    Console.WriteLine(veiculoController.BuscarVeiculoPlaca("MNO7890"));
//    veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Reservado.ToString(), "MNO7890");
//    Console.WriteLine(veiculoController.BuscarVeiculoPlaca("MNO7890"));
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro " + ex.Message);
//}
#endregion