﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Celular
    {
        public int Numero { get; set; }
        public string Marca { get; set; }
        public bool Novo { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataFabricacao { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                if (((List<Celular>)session["ListaCelular"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Celular>();
            lista.Add(new Celular { Numero = 20, Marca = "Xiaomi", Novo = true, DataFabricacao = new DateTime(2025,02,12) });
            lista.Add(new Celular { Numero = 47, Marca = "Motorola", Novo = false, DataFabricacao = new DateTime(2020, 02, 1) });
            lista.Add(new Celular { Numero = 1, Marca = "Iphone", Novo = false, DataFabricacao = new DateTime(2022, 02, 20) });

            session.Remove("ListaCelular");
            session.Add("ListaCelular", lista);
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                (session["ListaCelular"] as List<Celular>).Add(this);
            }
        }
        public static Celular Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCelular"] != null)
            {
                return (session["ListaCelular"] as List<Celular>).ElementAt(id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                (session["ListaCelular"] as List<Celular>).Remove(this);
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCelular"] != null)
            {
                var celular = Celular.Procurar(session, id);
                celular.Numero = this.Numero;
                celular.Marca = this.Marca;
                celular.Novo = this.Novo;
                celular.DataFabricacao = this.DataFabricacao;
            }
        }
    }
}