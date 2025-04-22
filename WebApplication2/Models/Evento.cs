using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Evento
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O local é obrigatório.")]
        public string Local { get; set; }
        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data do evento")]
        public DateTime Data { get; set; }
        
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null && ((List<Evento>)session["ListaEvento"]).Count > 0)
            {
                return;
            }
            var lista = new List<Evento>();
            lista.Add(new Evento { Id = 0, Local = "Sorocaba", Data = new DateTime(2025, 2, 26) });
            lista.Add(new Evento { Id = 1, Local = "Araçoiaba da Serra", Data = new DateTime(2025, 2, 28) });
            lista.Add(new Evento { Id = 2, Local = "Infinity", Data = new DateTime(2025, 10, 25) });
            lista.Add(new Evento { Id = 3, Local = "Campinas", Data = new DateTime(2025, 3, 15) });
            lista.Add(new Evento { Id = 4, Local = "São Paulo", Data = new DateTime(2025, 4, 10) });
            lista.Add(new Evento { Id = 5, Local = "Votorantim", Data = new DateTime(2025, 5, 5) });
            lista.Add(new Evento { Id = 6, Local = "Itu", Data = new DateTime(2025, 6, 12) });
            lista.Add(new Evento { Id = 7, Local = "Salto", Data = new DateTime(2025, 7, 20) });
            lista.Add(new Evento { Id = 8, Local = "Tatuí", Data = new DateTime(2025, 8, 18) });
            lista.Add(new Evento { Id = 9, Local = "Boituva", Data = new DateTime(2025, 9, 22) });
            lista.Add(new Evento { Id = 10, Local = "Itapetininga", Data = new DateTime(2025, 10, 30) });
            lista.Add(new Evento { Id = 11, Local = "Iperó", Data = new DateTime(2025, 11, 11) });
            lista.Add(new Evento { Id = 12, Local = "São Roque", Data = new DateTime(2025, 12, 5) });


            session["ListaEvento"] = lista;
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            if (lista == null)
            {
                lista = new List<Evento>();
                session["ListaEvento"] = lista;
            }

            this.Id = lista.Count > 0 ? lista.Max(c => c.Id) + 1 : 0;
            lista.Add(this);
        }

        public static Evento Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            return lista?.FirstOrDefault(c => c.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            var original = lista?.FirstOrDefault(c => c.Id == id);

            if (original != null)
            {
                original.Local = this.Local;
                original.Data = this.Data;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            lista?.RemoveAll(c => c.Id == this.Id);
        }
    }
}