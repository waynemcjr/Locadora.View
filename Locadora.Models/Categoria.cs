using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Categoria
    {
        public readonly static string INSERTCATEGORIA = @"INSERT INTO tblCategorias(Nome,Descricao,Diaria) 
                                                                 VALUES(@Nome,@Descricao,@Diaria);";

        public readonly static string SELECTCATEGORIAPORID = @"SELECT Nome,Descricao,Diaria
                                                              FROM tblCategorias 
                                                              WHERE CategoriaID = @idCategoria;";



        public readonly static string SELECTTODASCATEGORIAS = @"SELECT Nome,Descricao,Diaria 
                                                                FROM tblCategorias;";

        public readonly static string UPDATECATEGORIA = @"UPDATE tblCategorias
                                                               SET Nome = @Nome,
                                                                    Descricao = @Descricao,
                                                                    Diaria = @Diaria
                                                               WHERE CategoriaID = @idCategoria;";

        public readonly static string DELETECATEGORIA = @"DELETE FROM tblCategorias
                                                               WHERE CategoriaID = @idCategoria;";


        public int CategoriaID { get; set; }

        public string Nome { get; private set; }

        public string? Descricao { get; private set; }

        public decimal Diaria { get; private set; }

        public List<Veiculo> Veiculos { get; private set; }

        public Categoria(string nome, decimal diaria)
        {
            this.Nome = nome;
            this.Diaria = diaria;
        }

        public Categoria(string nome, string? descricao, decimal diaria) : this(nome, diaria)
        {
            this.Descricao = descricao;
        }

        public void SetCategoriaID(int id)
        {
            this.CategoriaID = id;
        }

        public void SetVeiculos(Veiculo veiculo)
        {
            this.Veiculos.Add(veiculo);
        }

        public override string ToString()
        {
            return $"Nome: {this.Nome}\n" +
                    $"Descrição: {this.Descricao}\n" +
                    $"Diária: {this.Diaria}";
        }

    }
}