using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Celular
    {
        public int Numero { get; set; }
        public string Marca { get; set; }
        public bool Novo { get; set; }

        public static List<Celular> GerarLista()
        {
            var lista = new List<Celular>();
            lista.Add(new Celular { Numero = 123, Marca = "Xiaomi", Novo = true });
            lista.Add(new Celular { Numero = 1234, Marca = "Philco", Novo = false });
            lista.Add(new Celular { Numero = 1235, Marca = "Motorola", Novo = false });
            return lista;
        }
    }
}