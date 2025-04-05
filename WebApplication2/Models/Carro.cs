using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WebApplication1.Models;

namespace WebApplication2.Models
{
    public class Carro
    {
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataFabricacao { get; set; }
        
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                if (((List<Carro>)session["ListaCarro"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Carro>();
            lista.Add(new Carro { Placa = "ABCDE", Ano = 2020, Cor = "Cinza", DataFabricacao = new DateTime(2020, 03, 12) });
            lista.Add(new Carro { Placa = "ABCDG", Ano = 2025, Cor = "Vermelho", DataFabricacao = new DateTime(2025, 03, 12) });
            lista.Add(new Carro { Placa = "ABCDN", Ano = 2007, Cor = "Preto", DataFabricacao = new DateTime(2007, 03, 12) });

            session.Remove("ListaCarro");
            session.Add("ListaCarro", lista);
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Add(this);
            }
        }
        public static Carro Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                return (session["ListaCarro"] as List<Carro>).ElementAt(id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Remove(this);
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                var carro = Carro.Procurar(session, id);
                carro.Placa = this.Placa;
                carro.Ano = this.Ano;
                carro.Cor = this.Cor;
                carro.DataFabricacao = this.DataFabricacao;
            }
        }
    }
}