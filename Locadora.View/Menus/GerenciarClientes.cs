using Locadora.Controller;
using Locadora.Models;
using System.ComponentModel.Design;

namespace Locadora.View.Menus
{
    public class GerenciarClientes
    {
        private ClienteController clienteController = new ClienteController();

        public void Run()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("---| GERENCIAR CLIENTES |----");
                Console.WriteLine("1. Cadastrar Cliente");
                Console.WriteLine("2. Listar Clientes");
                Console.WriteLine("3. Buscar Cliente Por Email");
                Console.WriteLine("4. Atualizar Cliente");
                Console.WriteLine("5. Deletar Cliente");
                Console.WriteLine("6. Voltar ao Menu Principal\n");
                Console.Write("Digite a opção desejada: ");

                if (int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("------------------| CADASTRAR CLIENTE |------------------");
                            Console.Write("Digite o nome do cliente: ");
                            string nome = Console.ReadLine()!;
                            Console.Write("Digite o email do cliente: ");
                            string email = Console.ReadLine()!;
                            Console.Write("Digite o telefone do cliente: ");
                            string telefone = Console.ReadLine()!;
                            Console.Write("Digite o tipo de documento do cliente: ");
                            string tipoDocumento = Console.ReadLine()!;
                            Console.Write("Digite o número do documento do cliente: ");
                            string numeroDocumento = Console.ReadLine()!;
                            Console.Write("Digite a data de emissão do documento (dd/MM/yyyy): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataEmissao))
                            {
                                ErrorMessage("DIGITE A DATA DE EMISSÃO CORRETAMENTE.");
                                break;
                            }
                            if (dataEmissao > DateTime.Now)
                            {
                                ErrorMessage("A DATA DE EMISSÃO NÃO PODE SER MAIOR DO QUE A ATUAL DO SISTEMA.");
                                break;
                            }
                            Console.Write("Digite a data de validade do documento (dd/MM/yyyy): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataValidade))
                            {
                                ErrorMessage("DIGITE A DATA DE VALIDADE CORRETAMENTE.");
                                break;
                            }
                            if (dataValidade <= DateTime.Now)
                            {
                                ErrorMessage("O DOCUMENTO ESTÁ VENCIDO (DATA DE VALIDADE DEVE SER FUTURA).");
                                break;
                            }
                            if (dataValidade <= dataEmissao)
                            {
                                ErrorMessage("A DATA DE VALIDADE NÃO PODE SER MENOR OU IGUAL À DATA DE EMISSÃO.");
                                break;
                            }
                            try
                            {
                                clienteController.AdicionarCliente(
                                    new Cliente(nome, email, telefone),
                                    new Documento(tipoDocumento, numeroDocumento, dataEmissao, dataValidade)
                                );
                                PositiveMessage("CLIENTE CADASTRADO COM SUCESSO!");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CLIENTES.");
                            Console.Read();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("----------------------| LISTA DE CLIENTES |----------------------");
                            try
                            {
                                List<Cliente> clientes = clienteController.ListarTodosClientes();

                                foreach(Cliente c in clientes)
                                {
                                    Console.WriteLine(c);
                                    Console.WriteLine("-----------------------------------------------------------------");
                                }
                                if(clientes.Count == 0)
                                {
                                    Console.WriteLine("Nenhum cliente cadastrado.");
                                }
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CLIENTES.");
                            Console.Read();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("----------------------| BUSCAR CLIENTE PELO EMAIL |----------------------");
                            Console.Write("Digite o email do cliente: ");
                            try
                            {
                                Cliente cliente = clienteController.BuscarClientePorEmail(Console.ReadLine()!);

                                Console.WriteLine();
                                Console.WriteLine(cliente);

                                if(cliente is null)
                                {
                                    Console.WriteLine("Cliente não foi encontrado.");
                                }
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CLIENTES.");
                            Console.Read();
                            break;
                        case 4:
                            int opcaoAtualizar;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("---| ATUALIZAR CLIENTE |----");
                                Console.WriteLine("1. Atualizar Telefone");
                                Console.WriteLine("2. Atualizar Documento");
                                Console.WriteLine("3. Voltar à GERENCIAR CLIENTES");
                                Console.Write("Digite a opção desejada: ");

                                if(int.TryParse(Console.ReadLine(), out opcaoAtualizar)){

                                    switch (opcaoAtualizar)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("----------------| ATUALIZAR TELEFONE |----------------");
                                            Console.Write("Digite o email do cliente:");
                                            string emailAtualizarTelefone = Console.ReadLine()!;
                                            Console.Write("Digite o novo telefone do cliente:");
                                            string telefoneAtualizar = Console.ReadLine()!;

                                            try
                                            {
                                                clienteController.AtualizarTelefoneCliente(telefoneAtualizar, emailAtualizarTelefone);
                                                PositiveMessage("TELEFONE DO CLIENTE ATUALIZADO COM SUCESSO!");
                                            }
                                            catch(Exception e)
                                            {
                                                Console.WriteLine(e.ToString());
                                            }
                                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CLIENTES.");
                                            Console.Read();
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("----------------| ATUALIZAR DOCUMENTO | ----------------");
                                            Console.Write("Digite o email do cliente:");
                                            string emailAtualizarDocumento = Console.ReadLine()!;
                                            Console.Write("Digite o tipo de documento do cliente: ");
                                            string tipoDocumentoAtualizar = Console.ReadLine()!;
                                            Console.Write("Digite o número do documento do cliente: ");
                                            string numeroDocumentoAtualizar = Console.ReadLine()!;
                                            Console.Write("Digite a data de emissão do documento (dd/MM/yyyy): ");
                                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataEmissaoAtualizar))
                                            {
                                                ErrorMessage("DIGITE A DATA DE EMISSÃO CORRETAMENTE.");
                                                break;
                                            }
                                            if (dataEmissaoAtualizar > DateTime.Now)
                                            {
                                                ErrorMessage("A DATA DE EMISSÃO NÃO PODE SER MAIOR DO QUE A ATUAL DO SISTEMA.");
                                                break;
                                            }
                                            Console.Write("Digite a data de validade do documento (dd/MM/yyyy): ");
                                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataValidadeAtualizar))
                                            {
                                                ErrorMessage("DIGITE A DATA DE VALIDADE CORRETAMENTE.");
                                                break;
                                            }
                                            if (dataValidadeAtualizar <= DateTime.Now)
                                            {
                                                ErrorMessage("O DOCUMENTO ESTÁ VENCIDO (DATA DE VALIDADE DEVE SER FUTURA).");
                                                break;
                                            }
                                            if (dataValidadeAtualizar <= dataEmissaoAtualizar)
                                            {
                                                ErrorMessage("A DATA DE VALIDADE NÃO PODE SER MENOR OU IGUAL À DATA DE EMISSÃO.");
                                                break;
                                            }
                                            try
                                            {
                                                clienteController.AtualizarDocumentoCliente(new Documento(tipoDocumentoAtualizar, numeroDocumentoAtualizar, dataEmissaoAtualizar, dataValidadeAtualizar), emailAtualizarDocumento);
                                                PositiveMessage("DOCUMENTO ATUALIZADO COM SUCESSO!");
                                            }
                                            catch (Exception e)
                                            {
                                                ErrorMessage(e.ToString());
                                            }
                                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CLIENTES.");
                                            Console.Read();
                                            break;
                                        case 3:
                                            Console.Clear();
                                            break;
                                        default:
                                            Console.WriteLine("Digite uma opção válida (1 a 3).\n");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Digite um número inteiro entre 1 e 3!\n");
                                }
                            } while (opcaoAtualizar != 3);
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("----------------------| DELETAR CLIENTE |----------------------");

                            Console.Write("Digite o email do cliente: ");
                            try
                            {
                                clienteController.DeletarCliente(Console.ReadLine()!);
                                PositiveMessage("CLIENTE DELETADO COM SUCESSO!");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CLIENTES.");
                            Console.Read();
                            break;
                        case 6:
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Digite uma opção válida (1 a 6).\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Digite um número inteiro entre 1 e 6!\n");
                }

            } while (opcao != 6);
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