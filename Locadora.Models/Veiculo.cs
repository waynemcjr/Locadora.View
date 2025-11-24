using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Veiculo
    {
        public readonly static string INSERTVEICULO = @"INSERT INTO tblVeiculos (CategoriaID, Placa, Marca, Modelo, Ano, StatusVeiculo)
                                                        VALUES (@CategoriaID, @Placa, @Marca, @Modelo, @Ano, @StatusVeiculo)";

        public readonly static string SELECTVEICULOPORPLACA = @"SELECT VeiculoID,CategoriaID,Placa, Marca, Modelo, Ano, StatusVeiculo
                                                               FROM tblVeiculos
                                                               WHERE Placa = @Placa";

        public readonly static string SELECTVEICULOPORID = @"SELECT VeiculoID,CategoriaID,Placa, Marca, Modelo, Ano, StatusVeiculo
                                                               FROM tblVeiculos
                                                               WHERE VeiculoID = @idVeiculo";

        public readonly static string SELECTALLVEICULOS = @"SELECT CategoriaID,Placa, Marca, Modelo, Ano, StatusVeiculo
                                                            FROM tblVeiculos";

        public readonly static string UPDATESTATUSVEICULO = @"UPDATE tblVeiculos 
                                                              SET StatusVeiculo = @StatusVeiculo
                                                              WHERE VeiculoID = @idVeiculo";

        public readonly static string DELETEVEICULO = @"DELETE FROM tblVeiculos
                                                        WHERE VeiculoID = @idVeiculo;";

        

        public int VeiculoID { get; private set; }

        public int CategoriaID { get; private set; }

        public string Placa { get; private set; }

        public string Marca { get; private set; }

        public string Modelo { get; private set; }

        public int Ano { get; private set; }

        public string StatusVeiculo { get; private set; }

        public Veiculo(int categoriaID, string placa, string marca, string modelo, int ano, string statusVeiculo)
        {
            this.CategoriaID = categoriaID;
            this.Placa = placa;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Ano = ano;
            this.StatusVeiculo = statusVeiculo;
        }

        public void SetVeiculoID(int veiculoID)
        {
            this.VeiculoID = veiculoID;
        }

        public void SetStatusVeiculo(string status)
        {
            this.StatusVeiculo = status;
        }

        public override string ToString()
        {
            return $"Placa: {this.Placa}\n" +
                   $"Marca: {this.Marca}\n" +
                   $"Modelo: {this.Modelo}\n" +
                   $"Ano: {this.Ano}\n" +
                   $"Status: {this.StatusVeiculo}";
        }

    }
}
