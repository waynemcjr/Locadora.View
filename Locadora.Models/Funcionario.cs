using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Funcionario
    {
        public readonly static string INSERTFUNCIONARIO = @"INSERT INTO tblFuncionarios(Nome,CPF,Email,Salario)
                                                            VALUES (@Nome,@CPF,@Email,@Salario);";

        public readonly static string SELECTFUNCIONARIOPORCPF = @"SELECT FuncionarioID,Nome,CPF,Email,Salario
                                                                  FROM tblFuncionarios
                                                                  WHERE CPF = @CPF;";

        public readonly static string SELECTTODOSFUNCIONARIOS = @"SELECT Nome, CPF,Email,Salario
                                                                  FROM tblFuncionarios;";

        public readonly static string UPDATEFUNCIONARIOPORCPF = @"UPDATE tblFuncionarios
                                                                  SET Salario = @Salario
                                                                  WHERE FuncionarioID = @idFuncionario;";

        public readonly static string DELETEFUNCIONARIOPORCPF = @"DELETE FROM tblFuncionarios
                                                                  WHERE FuncionarioID = @idFuncionario; ";

        public int FuncionarioID {  get; private set; }

        public string Nome { get; private set; }    

        public string CPF { get; private set; }

        public string Email { get; private set; }

        public decimal? Salario { get; private set; }

        public List<Locacao> LocacoesGerenciadas { get; private set; }

        public Funcionario(string nome, string cPF, string email)
        {
            this.Nome = nome;
            this.CPF = cPF;
            this.Email = email;
        }

        public Funcionario(string nome,string cPF,string email,decimal? salario) : this(nome, cPF, email)
        {
            this.Salario = salario;
        }

        public void SetLocacoes(Locacao locacao)
        {
            this.LocacoesGerenciadas.Add(locacao);
        }

        public void SetFuncionarioID(int id)
        {
            this.FuncionarioID = id;
        }


        public override string ToString()
        {
            return $"Nome: {this.Nome}\n" +
                $"CPF: {this.CPF}\n" +
                $"Email: {this.Email}\n" +
                $"Salário: {this.Salario}";
        }
    }
}
