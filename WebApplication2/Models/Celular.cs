﻿using System;
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
                new Celular { Id = 2, Numero = 1, Marca = "Iphone", Novo = false, DataFabricacao = new DateTime(2022, 02, 20) },
                new Celular { Id = 3, Numero = 35, Marca = "Samsung", Novo = true, DataFabricacao = new DateTime(2025, 01, 15) },
                new Celular { Id = 4, Numero = 99, Marca = "Huawei", Novo = false, DataFabricacao = new DateTime(2021, 07, 23) },
                new Celular { Id = 5, Numero = 12, Marca = "Xiaomi", Novo = true, DataFabricacao = new DateTime(2025, 03, 10) },
                new Celular { Id = 6, Numero = 58, Marca = "Nokia", Novo = false, DataFabricacao = new DateTime(2019, 10, 05) },
                new Celular { Id = 7, Numero = 83, Marca = "Motorola", Novo = true, DataFabricacao = new DateTime(2024, 12, 01) },
                new Celular { Id = 8, Numero = 27, Marca = "Iphone", Novo = false, DataFabricacao = new DateTime(2023, 05, 14) },
                new Celular { Id = 9, Numero = 44, Marca = "LG", Novo = false, DataFabricacao = new DateTime(2018, 09, 30) },
                new Celular { Id = 10, Numero = 76, Marca = "Samsung", Novo = true, DataFabricacao = new DateTime(2025, 04, 01) },
                new Celular { Id = 11, Numero = 3, Marca = "Asus", Novo = false, DataFabricacao = new DateTime(2020, 06, 18) },
                new Celular { Id = 12, Numero = 64, Marca = "Realme", Novo = true, DataFabricacao = new DateTime(2025, 02, 28) }

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
