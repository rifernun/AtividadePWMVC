using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            Carro.GerarLista(Session);

            return View(Session["ListaCarro"] as List<Carro>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(Carro.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Carro carro)
        {
            {
                Carro.Procurar(Session, id)?.Excluir(Session);
                carro.Excluir(Session);

                return RedirectToAction("Listar");
            }
        }


        public ActionResult Create()
        {
            return View(new Carro());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carro Carro)
        {
            Carro.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Edit(int id, Carro Carro)
        {
            Carro.Editar(Session, id);

            return RedirectToAction("Listar");
        }
        public ActionResult GerarPdf()
        {
            var lista = Session["ListaCarro"] as List<Carro>;

            if (lista == null || lista.Count == 0)
            {
                return Content("Nenhum Carro encontrado na sessão.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Relatório de Carros", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable tabela = new PdfPTable(4);
                tabela.WidthPercentage = 100;

                float[] larguras = new float[] { 2f, 1f, 1.5f, 2f };
                tabela.SetWidths(larguras);

                var fontCabecalho = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                tabela.AddCell(new PdfPCell(new Phrase("Placa", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Ano", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Cor", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Data de Fabricação", fontCabecalho)));

                var fontNormal = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);

                foreach (var carro in lista)
                {
                    tabela.AddCell(new Phrase(carro.Placa, fontNormal));
                    tabela.AddCell(new Phrase(carro.Ano.ToString(), fontNormal));
                    tabela.AddCell(new Phrase(carro.Cor, fontNormal));
                    tabela.AddCell(new Phrase(carro.DataFabricacao.ToString("dd/MM/yyyy"), fontNormal));
                }

                doc.Add(tabela);
                doc.Close();

                byte[] pdfBytes = ms.ToArray();
                return File(pdfBytes, "application/pdf", "ListaCarros.pdf");
            }
        }
    }
}