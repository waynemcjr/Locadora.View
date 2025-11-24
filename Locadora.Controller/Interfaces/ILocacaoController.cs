using Locadora.Models;

namespace Locadora.Controller.Interfaces
{
    public interface ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao);

        public void AssociarFuncionario(string cpf,int idLocacao);

        public List<Locacao> ListarLocacoesAivas();

        public void FinalizarLocacao(int idLocacao);

        public List<Locacao> ListarLocacaoPorCliente(string email);

        public List<Locacao> ListarLocacaoPorFuncionario(string cpf);

        public List<Funcionario> ListarFuncionariosDeUmaLocacao(int idLocacao);

        public List<Locacao> ListarTodasLocacoes();

    }
}
