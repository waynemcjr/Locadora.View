using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Controller.Interfaces
{
    public interface IFuncionarioController
    {
        public void AdicionarFuncionario(Funcionario funcionario);

        public Funcionario BuscarFuncionarioPorCPF(string cpf);

        public List<Funcionario> ListarTodosFuncionarios();

        public void AtualizarFuncionarioPorCPF(decimal salario, string cpf);

        public void DeletarFuncionarioPorCPF(string cpf);

    }
}
