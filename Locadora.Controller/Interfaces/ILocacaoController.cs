using Locadora.Models;

namespace Locadora.Controller.Interfaces
{
    internal interface ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao, int funcionarioID);

        public List<Locacao> ListarTodasLocacoes();

        public Locacao BuscarLocacaoPorID(int id);

        public void FinalizarLocacao(int locacaoID, decimal multa);
    }
}
