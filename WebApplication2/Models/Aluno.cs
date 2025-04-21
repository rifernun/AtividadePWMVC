using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Datansc { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null && ((List<Aluno>)session["ListaAluno"]).Count > 0)
            {
                return;
            }

            var lista = new List<Aluno>
            {
                new Aluno { Id = 0, Nome = "Richard", Email = "richard@email.com", Datansc = new DateTime(2000, 01, 01) },
                new Aluno { Id = 1, Nome = "Matheus", Email = "matheus@email.com", Datansc = new DateTime(1999, 05, 10) },
                new Aluno { Id = 2, Nome = "Xandão", Email = "xandao@email.com", Datansc = new DateTime(1998, 07, 20) }
            };

            session["ListaAluno"] = lista;
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            if (lista == null)
            {
                lista = new List<Aluno>();
                session["ListaAluno"] = lista;
            }

            this.Id = lista.Count > 0 ? lista.Max(a => a.Id) + 1 : 0;
            lista.Add(this);
        }

        public static Aluno Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            return lista?.FirstOrDefault(a => a.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            var original = lista?.FirstOrDefault(a => a.Id == id);

            if (original != null)
            {
                original.Nome = this.Nome;
                original.Email = this.Email;
                original.Datansc = this.Datansc;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            lista?.RemoveAll(a => a.Id == this.Id);
        }
    }
}
