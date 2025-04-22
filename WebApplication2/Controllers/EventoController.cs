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
using OfficeOpenXml;

namespace WebApplication2.Controllers
{
    public class EventoController : Controller
    {
        // GET: Evento
        public ActionResult Index() => RedirectToAction("Listar");
        public ActionResult Listar()
        {
            Evento.GerarLista(Session);
            return View(Session["ListaEvento"] as List<Evento>);
        }
        public ActionResult Create() => View(new Evento());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(evento);
        }

        // Editar um Evento
        public ActionResult Edit(int id)
        {
            var evento = Evento.Procurar(Session, id);
            if (evento == null)
                return HttpNotFound();

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Evento Evento)
        {
            if (ModelState.IsValid)
            {
                Evento.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(Evento);
        }

        // Exclusão com Ajax
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var evento = Evento.Procurar(Session, id);
            if (evento != null)
            {
                evento.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Evento não encontrado");
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
        public ActionResult DownloadExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("<Richard>");

            var lista = Session["ListaEvento"] as List<Evento>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Eventoes");
                planilha.Cells[1, 1].Value = "Local";
                planilha.Cells[1, 2].Value = "Data";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var evento = lista[i];
                    planilha.Cells[i + 2, 1].Value = evento.Local;
                    planilha.Cells[i + 2, 2].Value = evento.Data.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();

                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Eventoes.xlsx");
            }
        }
    }
}