using Locadora.Controller;
using Locadora.Models;

namespace Locadora.View.Menus
{
    public class GerenciarCategoriasVeiculos
    {
        private CategoriaController categoriaController = new CategoriaController();
        private VeiculoController veiculoController = new VeiculoController();

        public void Run()
        {
            Console.Clear();
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("-----| GERENCIAR CATEGORIAS E VEÍCULOS |----");
                Console.WriteLine("1. Cadastrar Categoria");
                Console.WriteLine("2. Listar Categorias com Veículos");
                Console.WriteLine("3. Cadastrar Veículo");
                Console.WriteLine("4. Consultar Veículos por Categoria");
                Console.WriteLine("5. Atualizar Status do Veículo");
                Console.WriteLine("6. Voltar ao Menu Principal\n");
                Console.Write("Digite a opção desejada: ");

                if (int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("------------| CADASTRAR CATEGORIA |------------");
                            Console.Write("Digite o nome da categoria: ");
                            string nomeCategoria = Console.ReadLine()!;
                            Console.Write("Digite a descrição da categoria: ");
                            string descricaoCategoria = Console.ReadLine()!;
                            Console.Write("Digite o valor da diária da categoria: ");
                            if (!Decimal.TryParse(Console.ReadLine(), out decimal diariaCategoria))
                            {

                                ErrorMessage("DIGITE O VALOR DA DIÁRIA CORRETAMENTE!");
                            }
                            try
                            {
                                categoriaController.AdicionarCategoria(new Categoria(nomeCategoria,descricaoCategoria,diariaCategoria));
                                PositiveMessage("CATEGORIA ADICIONADA COM SUCESSO!");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CATEGORIAS E VEÍCULOS.");
                            Console.Read();
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("------------| ATUALIZAR STATUS DO VEÍCULO |------------");
                            Console.Write("Digite a placa do veículo: ");
                            string placa = Console.ReadLine()!;
                            Console.Write("Digite o status do veículo: ");
                            string status = Console.ReadLine()!;
                            try
                            {
                                veiculoController.AtualizarVeiculo(status, placa);
                                PositiveMessage("STATUS DO VEÍCULO ATUALIZADO COM SUCESSO!");
                            }
                            catch (Exception e)
                            {
                                ErrorMessage(e.ToString());
                            }
                            AlertMessage("Pressione qualquer tecla para voltar à GERENCIAR CATEGORIAS E VEÍCULOS.");
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