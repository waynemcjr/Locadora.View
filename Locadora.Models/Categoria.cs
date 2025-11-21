using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Categoria
    {
        public readonly static string INSERTCATEGORIA = "INSERT INTO tblCategorias VALUES (@Nome, @Descricao, @Diaria); SELECT SCOPE_IDENTITY()";

        public readonly static string SELECTALLCATEGORIA = "SELECT * FROM tblCategorias";

        public readonly static string BUSCARCATEGORIAPORNOME = "SELECT * FROM tblCategorias WHERE Nome = @Nome";

        public readonly static string UPDATECATEGORIA = "UPDATE tblCategorias SET Diaria = @Diaria WHERE Nome = @Nome";

        public readonly static string DELETECATEGORIA = "DELETE tblCategorias WHERE CategoriaID = @IdCategoria";

        public int CategoriaID { get; private set; }
        public string Nome { get; private set; }
        public string? Descricao { get; private set; } = String.Empty;
        public decimal Diaria { get; private set; }

        public Categoria(string nome, string descicao, decimal diaria)
        {
            Nome = nome;
            Descricao = descicao;
            Diaria = diaria;
        }

        public override string? ToString()
        {
            return $"Nome: {Nome}\nDescrição: {Descricao}\nValor da diaria: {Diaria}";
        }

        public void setCategoriaID(int categoriaId)
        {
            CategoriaID = categoriaId;
        }

        public void setDiaria(decimal diaria)
        {
            Diaria = diaria;
        }
    }
}
