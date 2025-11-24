using Locadora.Controller;
using Locadora.Models;
using System.Collections.Generic;

namespace Locadora.View.Menus
{
    public class GerenciarLocacoes
    {
        private LocacaoController locacaoController = new LocacaoController();

        public void Run()
        {
            Console.Clear();
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("-----| GERENCIAR LOCAÇÕES |----");
                Console.WriteLine("1. Registrar Nova Locação");
                Console.WriteLine("2. Associar Funcionário à Locação");
                Console.WriteLine("3. Consultar Locações Ativas");
                Console.WriteLine("4. Finalizar Locação");
                Console.WriteLine("5. Listar Locações por Cliente");
                Console.WriteLine("6. Listar Locações por Funcionário");
                Console.WriteLine("7. Listar Funcionários de uma Locação");
                Console.WriteLine("8. Histórico de Locações");
                Console.WriteLine("9. Voltar ao Menu Principal\n");
                Console.Write("Digite a opção desejada: ");

                if (int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("-------------------| REGISTRAR NOVA LOCAÇÃO |------------------");
                            Console.Write("Digite o id do cliente: ");
                            if (int.TryParse(Console.ReadLine(), out int idCliente))
                            {
                                Console.Write("Digite o id do veículo: ");
                                if (int.TryParse(Console.ReadLine(), out int idVeiculo))
                                {

                                    Console.Write("Digite o valor da diária: ");
                                    if (Double.TryParse(Console.ReadLine(), out double valorDiaria))
                                    {

                                        Console.Write("Digite o número de dias para finalizar a locação: ");
                                        if (int.TryParse(Console.ReadLine(), out int diasParaFinalizar))
                                        {
                                            try
                                            {
                                                locacaoController.AdicionarLocacao(new Locacao(idCliente, idVeiculo, (Decimal)valorDiaria, diasParaFinalizar));
                                                PositiveMessage("LOCAÇÃO CADASTRADA COM SUCESSO!");
                                            }
                                            catch (Exception e)
                                            {
                                                ErrorMessage(e.ToString());
                                            }
                                        }
                                        else
                                        {
                                            ErrorMessage("DIGITE O NÚMERO DE DIAS PARA FINALIZAR A LOCAÇÃO CORRETAMENTE!");
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessage("DIGITE O VALOR DA DIÁRIA CORRETAMENTE!");
                                    }
                                }
                                else
                                {
                                    ErrorMessage("DIGITE O ID DO VEÍCULO CORRETAMENTE!");
                                }
                            }
                            else
                            {
                                ErrorMessage("DIGITE O ID DO CLIENTE CORRETAMENTE!");
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("-------------------| ASSOCIAR FUNCIONÁRIO À LOCAÇÃO |------------------");

                            Console.Write("Digite o CPF do funcionário: ");
                            string cpf = Console.ReadLine()!;

                            Console.Write("Digite o ID da locação: ");
                            if (int.TryParse(Console.ReadLine(), out int idLocacaoAssociar))
                            {
                                try
                                {
                                    locacaoController.AssociarFuncionario(cpf, idLocacaoAssociar);
                                    PositiveMessage("FUNCIONÁRIO ADICIONADO À LOCAÇÃO COM SUCESSO!");
                                }
                                catch (Exception e)
                                {
                                    ErrorMessage(e.ToString());
                                }
                            }
                            else
                            {
                                ErrorMessage("DIGITE O ID DA LOCAÇÃO CORRETAMENTE!");
                            }

                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 3:
                            Console.Clear();
                            try
                            {
                                List<Locacao> locacoes = locacaoController.ListarLocacoesAivas();
                                Console.WriteLine("------------------| LISTA DE LOCAÇÕES ATIVAS |----------------");
                                foreach (Locacao l in locacoes)
                                {
                                    Console.WriteLine(l);
                                    Console.WriteLine("--------------------------------------------------------------");
                                }
                                if (locacoes.Count == 0)
                                    Console.WriteLine("Nenhuma locação está ativa.");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("-------------------| FINALIZAR LOCAÇÃO |------------------");
                            Console.Write("Digite o ID da locação: ");
                            if (int.TryParse(Console.ReadLine(), out int idLocacaoFinalizar))
                            {
                                try
                                {
                                    locacaoController.FinalizarLocacao(idLocacaoFinalizar);
                                    PositiveMessage("LOCAÇÃO FINALIZADA COM SUCESSO!");
                                }
                                catch (Exception e)
                                {
                                    ErrorMessage(e.ToString());
                                }
                            }
                            else
                            {
                                ErrorMessage("DIGITE O ID DA LOCAÇÃO CORRETAMENTE!");
                            }

                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("---------------| LISTA DE LOCAÇÕES POR CLIENTE |---------------");

                            Console.Write("Digite o email do cliente: ");
                            string emailBusca = Console.ReadLine()!;

                            try
                            {
                                Console.WriteLine("");
                                List<Locacao> locacoes = locacaoController.ListarLocacaoPorCliente(emailBusca);

                                foreach (Locacao l in locacoes)
                                {
                                    Console.WriteLine(l);
                                    Console.WriteLine("--------------------------------------------------------------");
                                }
                                if (locacoes != null)
                                    Console.WriteLine("Esse cliente não realizou nenhuma locação.");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }

                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine("-------------| LISTA DE LOCAÇÕES POR FUNCIONÁRIO |------------");

                            Console.Write("Digite o CPF do funcionário: ");
                            string cpfBusca = Console.ReadLine()!;

                            try
                            {
                                Console.WriteLine("\n");
                                List<Locacao> locacoes = locacaoController.ListarLocacaoPorFuncionario(cpfBusca);

                                foreach (Locacao l in locacoes)
                                {
                                    Console.WriteLine(l);
                                    Console.WriteLine("--------------------------------------------------------------");
                                }
                                if (locacoes is null)
                                    Console.WriteLine("Esse funcionário não realizou nenhuma locação.");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }

                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                            break;
                        case 7:
                            Console.Clear();
                            Console.WriteLine("-----------| LISTA DE FUNCIONÁRIOS DE UMA LOCAÇÃO |-----------");

                            Console.Write("Digite o ID da locação: ");
                            if (int.TryParse(Console.ReadLine(), out int idLocacao))
                            {
                                try
                                {
                                    Console.WriteLine();
                                    List<Funcionario> funcionarios = locacaoController.ListarFuncionariosDeUmaLocacao(idLocacao);

                                    foreach (Funcionario f in funcionarios)
                                    {
                                        Console.WriteLine(f);
                                        Console.WriteLine("--------------------------------------------------------------");
                                    }
                                    if (funcionarios is null)
                                        Console.WriteLine("Nenhum funcionário está associado a essa locação.");
                                }
                                catch (Exception e)
                                {
                                    ErrorMessage(e.ToString());
                                }
                            }
                            else
                            {
                                ErrorMessage("DIGITE O ID CORRETAMENTE!");
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 8:
                            Console.Clear();
                            try
                            {
                                List<Locacao> locacoes = locacaoController.ListarTodasLocacoes();
                                Console.WriteLine("-------------------| HISTÓRICO DE LOCAÇÕES |------------------");
                                Console.WriteLine();
                                foreach (Locacao l in locacoes)
                                {
                                    Console.WriteLine(l);
                                    Console.WriteLine("--------------------------------------------------------------");
                                }
                                if (locacoes is null)
                                    Console.WriteLine("Nenhuma locação cadastrada ainda.");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR LOCAÇÕES.");
                            Console.Read();
                            break;
                        case 9:
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Digite uma opção válida (1 a 9).\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Digite um número inteiro entre 1 e 9!\n");
                }

            } while (opcao != 9);
        }
        public void PositiveMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void AlertMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}