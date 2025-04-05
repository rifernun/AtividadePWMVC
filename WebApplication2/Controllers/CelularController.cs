using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CelularController : Controller
    {
        // GET: Celular
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            Celular.GerarLista(Session);

            return View(Session["ListaCelular"] as List<Celular>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCelular"] as List<Celular>).ElementAt(id));
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaCelular"] as List<Celular>).ElementAt(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(Celular.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Celular Celular)
        {
            {
                Celular.Procurar(Session, id)?.Excluir(Session);
                Celular.Excluir(Session);

                return RedirectToAction("Listar");
            }
        }


        public ActionResult Create()
        {
            return View(new Celular());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular Celular)
        {
            Celular.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Edit(int id, Celular Celular)
        {
            Celular.Editar(Session, id);

            return RedirectToAction("Listar");
        }
        public ActionResult GerarPdf()
        {
            var lista = Session["ListaCelular"] as List<Celular>;

            if (lista == null || lista.Count == 0)
            {
                return Content("Nenhum celular encontrado na sessão.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Relatório de Celulares", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable tabela = new PdfPTable(4);
                tabela.WidthPercentage = 100;

                // Largura proporcional entre colunas
                float[] larguras = new float[] { 1.5f, 2.5f, 1.2f, 2f };
                tabela.SetWidths(larguras);

                var fontCabecalho = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                tabela.AddCell(new PdfPCell(new Phrase("Número", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Marca", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Novo", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Data de Fabricação", fontCabecalho)));

                var fontNormal = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);

                foreach (var celular in lista)
                {
                    tabela.AddCell(new Phrase(celular.Numero.ToString(), fontNormal));
                    tabela.AddCell(new Phrase(celular.Marca, fontNormal));
                    tabela.AddCell(new Phrase(celular.Novo ? "Sim" : "Não", fontNormal));
                    tabela.AddCell(new Phrase(celular.DataFabricacao.ToString("dd/MM/yyyy"), fontNormal));
                }

                doc.Add(tabela);
                doc.Close();

                byte[] pdfBytes = ms.ToArray();
                return File(pdfBytes, "application/pdf", "ListaCelulares.pdf");
            }
        }

    }
}