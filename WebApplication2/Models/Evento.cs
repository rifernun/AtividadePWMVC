using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Evento
    {
        public string Local { get; set; }
        public DateTime Data { get; set; }
        public static List<Evento> GerarLista()
        {
            var lista = new List<Evento>();
            lista.Add(new Evento { Local = "Sorocaba", Data = new DateTime(2025, 2, 26) });
            lista.Add(new Evento { Local = "Araçoiaba da Serra", Data = new DateTime(2025, 2, 28) });
            lista.Add(new Evento { Local = "Infinity", Data = new DateTime(2025, 10, 25) });
            return lista;
        }
    }
}