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
using Org.BouncyCastle.Tls;

namespace WebApplication2.Controllers
{
    public class EventoController : Controller
    {
        // GET: Evento
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            Evento.GerarLista(Session);

            return View(Session["ListaEvento"] as List<Evento>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaEvento"] as List<Evento>).ElementAt(id));
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaEvento"] as List<Evento>).ElementAt(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(Evento.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Evento Evento)
        {
            {
                Evento.Procurar(Session, id)?.Excluir(Session);
                Evento.Excluir(Session);

                return RedirectToAction("Listar");
            }
        }


        public ActionResult Create()
        {
            return View(new Evento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento Evento)
        {
            Evento.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Edit(int id, Evento Evento)
        {
            Evento.Editar(Session, id);

            return RedirectToAction("Listar");
        }
        public ActionResult GerarPdf()
        {
            var lista = Session["ListaEvento"] as List<Evento>;

            if (lista == null || lista.Count == 0)
            {
                return Content("Nenhum Evento encontrado na sessão.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Relatório de Eventos", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable tabela = new PdfPTable(2);
                tabela.WidthPercentage = 100;

                float[] larguras = new float[] { 3f, 2f };
                tabela.SetWidths(larguras);

                var fontCabecalho = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                tabela.AddCell(new PdfPCell(new Phrase("Local", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Data do Evento", fontCabecalho)));

                var fontNormal = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);

                foreach (var Evento in lista)
                {
                    tabela.AddCell(new Phrase(Evento.Local, fontNormal));
                    tabela.AddCell(new Phrase(Evento.Data.ToString("dd/MM/yyyy"), fontNormal));
                }

                doc.Add(tabela);
                doc.Close();

                byte[] pdfBytes = ms.ToArray();
                return File(pdfBytes, "application/pdf", "ListaEventos.pdf");
            }
        }
    }
}