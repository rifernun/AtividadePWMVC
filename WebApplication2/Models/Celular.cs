using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Celular
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Obrigatório.")]
        public bool Novo { get; set; }

        [Required(ErrorMessage = "A data de fabricação é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Fabricação")]
        public DateTime DataFabricacao { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null && ((List<Celular>)session["ListaCelular"]).Count > 0)
            {
                return;
            }

            var lista = new List<Celular>
            {
                new Celular { Id = 0, Numero = 20, Marca = "Xiaomi", Novo = true, DataFabricacao = new DateTime(2025, 02, 12) },
                new Celular { Id = 1, Numero = 47, Marca = "Motorola", Novo = false, DataFabricacao = new DateTime(2020, 02, 01) },
                new Celular { Id = 2, Numero = 1, Marca = "Iphone", Novo = false, DataFabricacao = new DateTime(2022, 02, 20) }
            };

            session["ListaCelular"] = lista;
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaCelular"] as List<Celular>;
            if (lista == null)
            {
                lista = new List<Celular>();
                session["ListaCelular"] = lista;
            }

            this.Id = lista.Count > 0 ? lista.Max(c => c.Id) + 1 : 0;
            lista.Add(this);
        }

        public static Celular Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCelular"] as List<Celular>;
            return lista?.FirstOrDefault(c => c.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCelular"] as List<Celular>;
            var original = lista?.FirstOrDefault(c => c.Id == id);

            if (original != null)
            {
                original.Numero = this.Numero;
                original.Marca = this.Marca;
                original.Novo = this.Novo;
                original.DataFabricacao = this.DataFabricacao;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaCelular"] as List<Celular>;
            lista?.RemoveAll(c => c.Id == this.Id);
        }
    }
}
