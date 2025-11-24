using Locadora.Controller;
using Locadora.Models;

namespace Locadora.View.Menus
{
    public class GerenciarFuncionarios
    {
        private FuncionarioController funcionarioController = new FuncionarioController();

        public void Run()
        {
            Console.Clear();
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("-----| GERENCIAR FUNCIONÁRIOS |----");
                Console.WriteLine("1. Cadastrar Funcionário");
                Console.WriteLine("2. Listar Funcionários");
                Console.WriteLine("3. Atualizar Funcionário");
                Console.WriteLine("4. Deletar Funcionário");
                Console.WriteLine("5. Voltar ao Menu Principal\n");
                Console.Write("Digite a opção desejada: ");

                if (int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("-------------------| CADASTRAR FUNCIONÁRIO |------------------");
                            Console.Write("Digite o nome do funcionário: ");
                            string nome = Console.ReadLine()!;

                            Console.Write("Digite o salário do funcionário: ");
                            if (Double.TryParse(Console.ReadLine(), out double salario))
                            {
                                Console.Write("Digite o CPF do funcionário: ");
                                string cpf = Console.ReadLine()!;

                                Console.Write("Digite o email do funcionário: ");
                                string email = Console.ReadLine()!;
                                try
                                {
                                    funcionarioController.AdicionarFuncionario(new Funcionario(nome,cpf,email,(Decimal)salario));
                                    PositiveMessage("FUNCIONÁRIO CADASTRADO COM SUCESSO!");
                                }
                                catch (Exception e)
                                {
                                    ErrorMessage(e.ToString());
                                }
                            }
                            else
                            {
                                ErrorMessage("DIGITE O SALÁRIO CORRETAMENTE!");
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR FUNCIONÁRIOS.");
                            Console.Read();
                            break;
                        case 2:
                            Console.Clear();
                            try
                            {
                                List<Funcionario> funcionarios = funcionarioController.ListarTodosFuncionarios();
                                Console.WriteLine("-------------------| LISTA DE FUNCIONÁRIOS |------------------");
                                foreach (Funcionario f in funcionarios)
                                {
                                    Console.WriteLine(f);
                                    Console.WriteLine("--------------------------------------------------------------");
                                }
                            }
                            catch(Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR FUNCIONÁRIOS.");
                            Console.Read();

                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("-------------------| ATUALIZAR FUNCIONÁRIO |------------------");
                            Console.Write("Digite o novo salário do funcionário: ");
                            if (Double.TryParse(Console.ReadLine(), out double salarioAtualizado))
                            {
                                Console.Write("Digite o CPF do funcionário: ");
                                string cpf = Console.ReadLine()!;

                                try
                                {
                                    funcionarioController.AtualizarFuncionarioPorCPF((Decimal)salarioAtualizado, cpf);
                                    PositiveMessage("FUNCIONÁRIO ATUALIZADO COM SUCESSO!");
                                }
                                catch (Exception e)
                                {
                                    ErrorMessage(e.ToString());
                                }
                            }
                            else
                            {
                                ErrorMessage("DIGITE O SALÁRIO CORRETAMENTE!");
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR FUNCIONÁRIOS.");
                            Console.Read();

                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("-------------------| DELETAR FUNCIONÁRIO |-------------------");
                            Console.Write("Digite o CPF do funcionário: ");
                            try
                            {
                                funcionarioController.DeletarFuncionarioPorCPF(Console.ReadLine()!);
                                PositiveMessage("FUNCIONÁRIO DELETADO COM SUCESSO!");
                            }
                            catch(Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR FUNCIONÁRIOS.");
                            Console.Read();
                            break;
                        case 5:
                            Console.Clear();
                            break;
                        default:
                            AlertMessage("Digite uma opção válida (1 a 5).\n");
                            break;
                    }
                    
                }
                else
                {
                    AlertMessage("Digite um número inteiro entre 1 e 5!\n");
                }

            } while (opcao != 5);

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