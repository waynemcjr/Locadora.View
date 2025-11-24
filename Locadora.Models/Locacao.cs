using Locadora.Models.Enums;

namespace Locadora.Models
{
    public class Locacao
    {
        public readonly static string INSERTLOCACAO = @"INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao,DataDevolucaoReal,DataDevolucaoPrevista,ValorDiaria,ValorTotal,Multa,Status)
                                                       VALUES (@idCliente,@idVeiculo,@DataLocacao,@DataDevolucaoReal,@DataDevolucaoPrevista,@ValorDiaria,@ValorTotal,@Multa,@Status);";

        public readonly static string SELECTLOCACAOPORID = @"SELECT l.LocacaoID,l.ClienteID, l.VeiculoID, l.DataLocacao,l.DataDevolucaoReal,l.DataDevolucaoPrevista,l.ValorDiaria,l.ValorTotal,l.Multa,l.Status,
                                                                    f.CPF
                                                             FROM tblLocacoes l
                                                             LEFT JOIN tblLocacaoFuncionarios lf ON l.LocacaoID = lf.LocacaoID
                                                             LEFT JOIN tblFuncionarios f ON f.FuncionarioID = lf.FuncionarioID
                                                             WHERE l.LocacaoID = @idLocacao;";

        public readonly static string SELECTLOCACAOPORCLIENTE = @"SELECT l.LocacaoID,l.ClienteID, l.VeiculoID, l.DataLocacao,l.DataDevolucaoPrevista,l.DataDevolucaoReal,l.ValorDiaria,l.ValorTotal,l.Multa,l.Status,
                                                                       f.CPF
                                                                  FROM tblLocacoes l
                                                                  JOIN tblClientes c ON l.ClienteID = c.ClienteID
                                                                  JOIN tblVeiculos v ON l.VeiculoID = v.VeiculoID
                                                                  LEFT JOIN tblLocacaoFuncionarios lf ON lf.LocacaoID = l.LocacaoID
                                                                  LEFT JOIN tblFuncionarios f ON f.FuncionarioID = lf.FuncionarioID
                                                                  WHERE c.ClienteID = @idCliente;";

        public readonly static string SELECTLOCACAOPORFUNCIONARIO = @"SELECT l.LocacaoID,l.ClienteID, l.VeiculoID, l.DataLocacao,l.DataDevolucaoPrevista,l.DataDevolucaoReal,l.ValorDiaria,l.ValorTotal,l.Multa,l.Status,
                                                                       f.CPF
                                                                  FROM tblLocacoes l
                                                                  JOIN tblClientes c ON l.ClienteID = c.ClienteID
                                                                  JOIN tblVeiculos v ON l.VeiculoID = v.VeiculoID
                                                                  LEFT JOIN tblLocacaoFuncionarios lf ON lf.LocacaoID = l.LocacaoID
                                                                  LEFT JOIN tblFuncionarios f ON f.FuncionarioID = lf.FuncionarioID
                                                                  WHERE f.FuncionarioID = @idFuncionario;";

        public readonly static string SELECTFUNCIONARIOSDEUMALOCACAO = @"SELECT f.FuncionarioID,f.Nome,f.CPF,f.Email,f.Salario
                                                                         FROM tblFuncionarios f
                                                                         JOIN tblLocacaoFuncionarios lf ON lf.FuncionarioID = f.FuncionarioID
                                                                         WHERE lf.LocacaoID = @idLocacao;";

        public readonly static string SELECTLOCAOESATIVAS = @"SELECT l.LocacaoID,l.ClienteID, l.VeiculoID, l.DataLocacao,l.DataDevolucaoPrevista,l.DataDevolucaoReal,l.ValorDiaria,l.ValorTotal,l.Multa,l.Status,
                                                                       f.CPF
                                                                  FROM tblLocacoes l
                                                                  JOIN tblClientes c ON l.ClienteID = c.ClienteID
                                                                  JOIN tblVeiculos v ON l.VeiculoID = v.VeiculoID
                                                                  LEFT JOIN tblLocacaoFuncionarios lf ON lf.LocacaoID = l.LocacaoID
                                                                  LEFT JOIN tblFuncionarios f ON f.FuncionarioID = lf.FuncionarioID
                                                                  WHERE l.Status = 'Ativa';";


        public readonly static string SELECTTODASLOCACOES = @"SELECT l.LocacaoID,l.ClienteID, l.VeiculoID, l.DataLocacao,l.DataDevolucaoPrevista,l.DataDevolucaoReal,l.ValorDiaria,l.ValorTotal,l.Multa,l.Status,
                                                                       f.CPF
                                                                  FROM tblLocacoes l
                                                                  JOIN tblClientes c ON l.ClienteID = c.ClienteID
                                                                  JOIN tblVeiculos v ON l.VeiculoID = v.VeiculoID
                                                                  LEFT JOIN tblLocacaoFuncionarios lf ON lf.LocacaoID = l.LocacaoID
                                                                  LEFT JOIN tblFuncionarios f ON f.FuncionarioID = lf.FuncionarioID
                                                                  ORDER BY l.DataLocacao DESC;";

        public static readonly string INSERTLOCACAOFUNCIONARIO = @"INSERT INTO tblLocacaoFuncionarios(FuncionarioID,LocacaoID)
                                                                   VALUES (@idFuncionario,@idLocacao);";

        public readonly static string UPDATELOCACAOPORID = @"UPDATE tblLocacoes
                                                           SET DataDevolucaoReal = GETDATE(),
                                                               Multa = CASE
                                                                           WHEN DATEDIFF(day, DataDevolucaoPrevista, GETDATE()) > 0 THEN
                                                                               DATEDIFF(day, DataDevolucaoPrevista, GETDATE()) * 2 * ValorDiaria
                                                                           ELSE
                                                                               0.00
                                                                       END,
                                                               ValorTotal = ValorDiaria * DATEDIFF(day, DataLocacao,DataDevolucaoPrevista),
                                                               Status = 'Finalizada'
                                                           WHERE LocacaoID = @idLocacao;";


        public int LocacaoID { get; private set; }

        public int ClienteID { get; private set; }

        public string ClienteNome { get; private set; }

        public string ClienteEmail { get; private set; }

        public int VeiculoID { get; private set; }

        public string VeiculoModelo { get; private set; }

        public string VeiculoPlaca { get; private set; }

        public DateTime DataLocacao { get; private set; }

        public DateTime DataDevolucaoPrevista { get; private set; }

        public DateTime? DataDevolucaoReal { get; private set; }

        public decimal ValorDiaria { get; private set; }

        public int DiasParaRetornar { get; private set; }

        public decimal ValorTotal { get; private set; }

        public decimal Multa { get; private set; }

        public EStatusLocacao Status { get; private set; }

        public List<Funcionario> Funcionarios { get; private set; }

        public Locacao(int clienteID, int veiculoID, int diasParaRetornar)
        {
            this.ClienteID = clienteID;
            this.VeiculoID = veiculoID;
            this.DataLocacao = DateTime.Now;
            this.DiasParaRetornar = diasParaRetornar;
            this.Multa = 0;
            this.DataDevolucaoPrevista = DateTime.Now.AddDays(diasParaRetornar);
            this.DataDevolucaoReal = null;
            this.Status = EStatusLocacao.Ativa;
            this.Funcionarios = [];
        }

        public Locacao(int locacaoID, int clienteID, int veiculoID, DateTime dataLocacao,
            DateTime? dataDevolucaoReal, DateTime dataDevolucaoPrevista, decimal valorDiaria,
            decimal valorTotal, decimal multa, EStatusLocacao status)
        {
            LocacaoID = locacaoID;
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = dataLocacao;
            DataDevolucaoReal = dataDevolucaoReal;
            DataDevolucaoPrevista = dataDevolucaoPrevista;
            ValorDiaria = valorDiaria;
            ValorTotal = valorTotal;
            Multa = multa;
            Status = status;
        }

        public void SetClienteNome(string clienteNome)
        {
            this.ClienteNome = clienteNome;
        }

        public void SetClienteEmail(string clienteEmail)
        {
            this.ClienteEmail = clienteEmail;
        }

        public void SetVeiculoModelo(string veiculoModelo)
        {
            this.VeiculoModelo = veiculoModelo;
        }

        public void SetVeiculoPlaca(string veiculoPlaca)
        {
            this.VeiculoPlaca = veiculoPlaca;
        }

        public void SetFuncionarios(List<Funcionario> funcionarios)
        {
            this.Funcionarios = funcionarios;
        }

        public override string? ToString()
        {
            var result = $"Cliente: {this.ClienteNome} => '{this.ClienteEmail}'\n" +
                          $"Veículo: {this.VeiculoModelo} => '{this.VeiculoPlaca}'\n\n" +
                          $"Data de Locação: {DateOnly.FromDateTime(this.DataLocacao)}\n" +
                          $"Data de Devolução Prevista: {DateOnly.FromDateTime(this.DataDevolucaoPrevista)}\n" +
                          $"Data de Devolução Real: {this.DataDevolucaoReal}\n\n" +
                          $"Valor da Diária: {this.ValorDiaria:C}\n" +
                          $"Valor Locação: {this.ValorTotal:C}\n" +
                          $"Multa: {this.Multa:C}\n" +
                          $"Valor Total: {(this.ValorTotal + this.Multa):C}\n" +
                          $"Status: {this.Status}\n\n" +
                          $"Funcionários associados:\n";
            foreach (Funcionario f in this.Funcionarios)
            {
                result += $"    {f.Nome} => ";
                result += $"'{f.Email}'\n";
            }
            if (this.Funcionarios.Count == 0)
                result += "   Nenhum funcionário associado.\n";
            return result;

        }
    }
}