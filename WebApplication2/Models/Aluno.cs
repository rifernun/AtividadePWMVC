﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace WebApplication1.Models
{
    public class Aluno
    {
        public string Nome { get; set; }
        public string RA { get; set; }


        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null) 
            {
                if (((List<Aluno>)session["ListaAluno"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Aluno>();
            lista.Add(new Aluno { Nome = "Richard", RA = "19" });
            lista.Add(new Aluno { Nome = "Matheus", RA = "16" });
            lista.Add(new Aluno { Nome = "Xandão", RA = "333" });

            session.Remove("ListaAluno");
            session.Add("ListaAluno", lista);
        }
        
        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                (session["ListaAluno"] as List<Aluno>).Add(this);
            }
        }
        public static Aluno Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaAluno"] != null)
            {
                return (session["ListaAluno"] as List<Aluno>).ElementAt(id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                (session["ListaAluno"] as List<Aluno>).Remove(this);
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaAluno"] != null)
            {
                var aluno = Aluno.Procurar(session,id);
                aluno.Nome = this.Nome;
                aluno.RA = this.RA;
            }
        }
    }
}