using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Carro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória.")]
        public string Cor { get; set; }

        [Required(ErrorMessage = "A data de fabricação é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Fabricação")]
        public DateTime DataFabricacao { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null && ((List<Carro>)session["ListaCarro"]).Count > 0)
            {
                return;
            }

            var lista = new List<Carro>
            {
                new Carro { Id = 0, Placa = "ABCDE", Ano = 2020, Cor = "Cinza", DataFabricacao = new DateTime(2020, 03, 12) },
                new Carro { Id = 1, Placa = "ABCDG", Ano = 2025, Cor = "Vermelho", DataFabricacao = new DateTime(2025, 03, 12) },
                new Carro { Id = 2, Placa = "ABCDN", Ano = 2007, Cor = "Preto", DataFabricacao = new DateTime(2007, 03, 12) },
                new Carro { Id = 3, Placa = "XYZ12", Ano = 2015, Cor = "Branco", DataFabricacao = new DateTime(2015, 07, 22) },
                new Carro { Id = 4, Placa = "LMNOP", Ano = 2018, Cor = "Azul", DataFabricacao = new DateTime(2018, 11, 10) },
                new Carro { Id = 5, Placa = "QWERT", Ano = 2012, Cor = "Prata", DataFabricacao = new DateTime(2012, 05, 03) },
                new Carro { Id = 6, Placa = "ASDFG", Ano = 2010, Cor = "Verde", DataFabricacao = new DateTime(2010, 09, 15) },
                new Carro { Id = 7, Placa = "ZXCVB", Ano = 2022, Cor = "Amarelo", DataFabricacao = new DateTime(2022, 01, 27) },
                new Carro { Id = 8, Placa = "TYUIO", Ano = 2005, Cor = "Marrom", DataFabricacao = new DateTime(2005, 03, 08) },
                new Carro { Id = 9, Placa = "GHJKL", Ano = 2016, Cor = "Preto", DataFabricacao = new DateTime(2016, 08, 19) },
                new Carro { Id = 10, Placa = "BNM98", Ano = 2019, Cor = "Branco", DataFabricacao = new DateTime(2019, 06, 30) },
                new Carro { Id = 11, Placa = "CVBNM", Ano = 2017, Cor = "Rosa", DataFabricacao = new DateTime(2017, 12, 01) },
                new Carro { Id = 12, Placa = "PLMKO", Ano = 2023, Cor = "Laranja", DataFabricacao = new DateTime(2023, 02, 14) }

            };

            session["ListaCarro"] = lista;
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            if (lista == null)
            {
                lista = new List<Carro>();
                session["ListaCarro"] = lista;
            }

            this.Id = lista.Count > 0 ? lista.Max(c => c.Id) + 1 : 0;
            lista.Add(this);
        }

        public static Carro Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            return lista?.FirstOrDefault(c => c.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            var original = lista?.FirstOrDefault(c => c.Id == id);

            if (original != null)
            {
                original.Placa = this.Placa;
                original.Ano = this.Ano;
                original.Cor = this.Cor;
                original.DataFabricacao = this.DataFabricacao;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            lista?.RemoveAll(c => c.Id == this.Id);
        }
    }
}
