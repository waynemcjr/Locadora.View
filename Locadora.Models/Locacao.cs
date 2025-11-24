using Locadora.Models.Enums;

namespace Locadora.Models
{
    public class Locacao
    {
        public static readonly string UPDATELOCACAOFINALIZACAO =
            @"UPDATE tblLocacoes
            SET DataDevolucaoReal = @data,
            Multa = @multa,
            Status = 'Finalizada'
            WHERE LocacaoID = @id";

        public static readonly string UPDATEVEICULOSFINALIZACAO =
            @"UPDATE tblVeiculos
            SET StatusVeiculo = 'Disponivel'
            WHERE VeiculoID = (SELECT VeiculoID FROM tblLocacoes WHERE LocacaoID = @locacaoID)";

        public static readonly string SETVEICULOINDISPONIVEL =
            @"UPDATE tblVeiculos 
            SET StatusVeiculo = 'Alugado' 
            WHERE VeiculoID = @id";

        public static readonly string INSERTLOCACAO =
            @"INSERT INTO tblLocacoes 
            (ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, DataDevolucaoReal, 
            ValorDiaria, ValorTotal, Multa, Status)
            VALUES (@ClienteID, @VeiculoID, @DataLocacao, @DataDevolucaoPrevista, @DataDevolucaoReal,
            @ValorDiaria, @ValorTotal, @Multa, @Status);";

        public static readonly string INSERTFUNCIONARIOLOCACAO =
            @"INSERT INTO tblLocacaoFuncionarios (LocacaoID, FuncionarioID)
            VALUES (@LocacaoID, @FuncionarioID)";

        public static readonly string SELECTALLLOCACOES =
            @"SELECT LocacaoID, ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista, 
            DataDevolucaoReal, ValorDiaria, ValorTotal, Multa, Status
            FROM tblLocacoes";

        public static readonly string SELECTLOCACAOPORID =
            @"SELECT LocacaoID, ClienteID, VeiculoID, DataLocacao, 
            DataDevolucaoPrevista, DataDevolucaoReal, ValorDiaria, 
            ValorTotal, Multa, Status
            FROM tblLocacoes
            WHERE LocacaoID = @ID";

        public int LocacaoID { get; private set; }
        public int ClienteID { get; private set; }
        public int VeiculoID { get; private set; }
        public Cliente Cliente { get; private set; }
        public Veiculo Veiculo { get; private set; }
        public DateTime DataLocacao { get; private set; }
        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }
        public decimal ValorDiaria { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal? Multa { get; private set; }
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

        public Locacao(
            int locacaoID,
            int clienteID,
            int veiculoID,
            DateTime dataLocacao,
            DateTime dataPrevista,
            DateTime? dataReal,
            decimal valorDiaria,
            decimal valorTotal,
            decimal? multa,
            EStatusLocacao status)
        {
            LocacaoID = locacaoID;
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = dataLocacao;
            DataDevolucaoPrevista = dataPrevista;
            DataDevolucaoReal = dataReal;
            ValorDiaria = valorDiaria;
            ValorTotal = valorTotal;
            Multa = multa;
            Status = status;
        }

        public void SetLocacaoID(int locacaoID)
        {
            LocacaoID = locacaoID;
        }

        public void SetValorDiaria(decimal valorDiaria)
        {
            ValorDiaria = valorDiaria;
        }
        public void SetCliente(Cliente cliente)
        {
            Cliente = cliente;
        }

        public void SetVeiculo(Veiculo veiculo)
        {
            Veiculo = veiculo;
        }

        //TODO: Definir os valores de cliente e veículo como nome e modelo, respectivamente.
        public override string ToString()
        {

            string nomeCliente = Cliente != null ? Cliente.Nome : $"ID {ClienteID}";
            string nomeVeiculo = Veiculo != null ? $"{Veiculo.Marca} {Veiculo.Modelo}" : $"ID {VeiculoID}";

            return
               $"Locação ID: {LocacaoID}\n" +
               $"Cliente: {nomeCliente}\n" +
               $"Veículo: {nomeVeiculo}\n" +
               $"Data de Locação: {DataLocacao}\n" +
               $"Data de Devolução Prevista: {DataDevolucaoPrevista}\n" +
               $"Data de Devolução Real: {DataDevolucaoReal}\n" +
               $"Valor da Diária: {ValorDiaria:C}\n" +
               $"Valor Total: {ValorTotal:C}\n" +
               $"Multa: {Multa:C}\n" +
               $"Status: {Status}\n";
        }
    }
}