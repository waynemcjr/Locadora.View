using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.View.Menus
{
    public class MenuPrincipal
    {
        private GerenciarClientes gerenciarClientes = new GerenciarClientes();
        private GerenciarCategoriasVeiculos gerenciarCategoriasVeiculos = new GerenciarCategoriasVeiculos();
        private GerenciarFuncionarios gerenciarFuncionarios = new GerenciarFuncionarios();
        private GerenciarLocacoes gerenciarLocacoes = new GerenciarLocacoes();

        public void Run()
        {
            Console.Clear();
            int opcao;
            do
            {
                Console.WriteLine("-----| SISTEMA DE LOCADORA DE VEÍCULOS |-----");
                Console.WriteLine("1. GERENCIAR CLIENTES");
                Console.WriteLine("2. GERENCIAR CATEGORIAS E VEÍCULOS");
                Console.WriteLine("3. GERENCIAR FUNCIONÁRIOS");
                Console.WriteLine("4. GERENCIAR LOCAÇÕES");
                Console.WriteLine("5. SAIR\n");
                Console.Write("Digite a opção desejada: ");

                if(int.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            gerenciarClientes.Run();
                            break;
                        case 2:
                            gerenciarCategoriasVeiculos.Run();
                            break;
                        case 3:
                            gerenciarFuncionarios.Run();
                            break;
                        case 4:
                            gerenciarLocacoes.Run();
                            break;
                        case 5:
                            Console.WriteLine("Saindo do sistema...");
                            break;
                        default:
                            Console.WriteLine("Digite uma opção válida (1 a 5).\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Digite um número inteiro entre 1 e 5!\n");
                }

            } while( opcao != 5 );
        }
    }

    
}
