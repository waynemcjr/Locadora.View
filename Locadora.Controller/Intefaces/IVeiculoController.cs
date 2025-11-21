using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locadora.Models;

namespace Locadora.Controller.Intefaces
{
    public interface IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo);

        public List<Veiculo> ListarTodosVeiculos();

        public Veiculo BuscarVeiculoPlaca(string placa);

        public void AtualizarStatusVeiculo(string statusVeiculo);

        public void DeletarVeiculo(int idVeiculo);
    }
}
