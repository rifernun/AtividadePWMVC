using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Carro
    {
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }

        public static List<Carro> GerarLista()
        {
            var lista = new List<Carro>();
            lista.Add(new Carro { Placa = "ABCDE", Ano = 2020, Cor = "Cinza" });
            lista.Add(new Carro { Placa = "ABCDG", Ano = 2025, Cor = "Vermelho" });
            lista.Add(new Carro { Placa = "ABCDN", Ano = 2007, Cor = "Preto" });

            return lista;
        }
    }
}