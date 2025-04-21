using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace WebApplication2.Controllers
{
    public class CelularController : Controller
    {
        // GET: Celular
        public ActionResult Index() => RedirectToAction("Listar");

        // Listar todos os Celulares
        public ActionResult Listar()
        {
            Celular.GerarLista(Session);
            return View(Session["ListaCelular"] as List<Celular>);
        }

        // Criar um novo Celular
        public ActionResult Create() => View(new Celular());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular celular)
        {
            if (ModelState.IsValid)
            {
                celular.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(celular);
        }

        // Editar um Celular
        public ActionResult Edit(int id)
        {
            var celular = Celular.Procurar(Session, id);
            if (celular == null)
                return HttpNotFound();

            return View(celular);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Celular celular)
        {
            if (ModelState.IsValid)
            {
                celular.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(celular);
        }

        // Exclusão com Ajax
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var celular = Celular.Procurar(Session, id);
            if (celular != null)
            {
                celular.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Celular não encontrado");
        }

        // Geração de PDF
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
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Relatório de Celulares", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable tabela = new PdfPTable(4) { WidthPercentage = 100 };
                tabela.SetWidths(new float[] { 2f, 2f, 1f, 2f });

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

        // Exportar para Excel
        public ActionResult DownloadExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("<Richard>");

            var lista = Session["ListaCelular"] as List<Celular>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Celulares");
                planilha.Cells[1, 1].Value = "Número";
                planilha.Cells[1, 2].Value = "Marca";
                planilha.Cells[1, 3].Value = "Novo";
                planilha.Cells[1, 4].Value = "Data de Fabricação";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var celular = lista[i];
                    planilha.Cells[i + 2, 1].Value = celular.Numero;
                    planilha.Cells[i + 2, 2].Value = celular.Marca;
                    planilha.Cells[i + 2, 3].Value = celular.Novo ? "Sim" : "Não";
                    planilha.Cells[i + 2, 4].Value = celular.DataFabricacao.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();

                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Celulares.xlsx");
            }
        }
    }
}
