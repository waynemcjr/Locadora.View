using Locadora.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Locacao
    {
        public Guid LocacaoID { get; private set; }
        public int ClienteID { get; private set; }
        public int VeiculoID { get; private set; }
        public DateTime DataLocacao { get; private set; }
        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }
        public decimal ValorDiaria { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal Multa { get; private set; }
        public EStatusLocacao Status { get; private set; }

        public Locacao(int clienteID, int veiculoID, decimal valorDiaria, int diasLocacao)
        {
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = DateTime.Now;
            ValorDiaria = valorDiaria;
            ValorTotal = valorDiaria * diasLocacao;
            DataDevolucaoPrevista = DateTime.Now.AddDays(diasLocacao);
            Status = EStatusLocacao.Ativa;
        }

        //TODO: Definir os valores de cliente e veiculo como nome e modelo respectivamente
        public override string ToString()
        {
            return $"Cliente: {ClienteID}\nVeiculo: {VeiculoID}\n" +
                $"DataLocacao: {DataLocacao}\n" +
                $"DataDevolucaoPrevista: {DataDevolucaoPrevista}\n" +
                $"DataDevolucaoReal: {DataDevolucaoReal}\n" +
                $"ValorDiaria: {ValorDiaria}\nValorTotal: {ValorTotal}\n" +
                $"Multa: {Multa}\nStatus: {Status}";
        }
    }
}