using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WebApplication2.Models;
using OfficeOpenXml;

namespace WebApplication2.Controllers
{
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index() => RedirectToAction("Listar");

        // Listar todos os carros
        public ActionResult Listar()
        {
            Carro.GerarLista(Session);
            return View(Session["ListaCarro"] as List<Carro>);
        }

        // Criar um novo carro
        public ActionResult Create() => View(new Carro());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carro carro)
        {
            if (ModelState.IsValid)
            {
                carro.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(carro);
        }

        // Editar um carro
        public ActionResult Edit(int id)
        {
            var carro = Carro.Procurar(Session, id);
            if (carro == null)
                return HttpNotFound();

            return View(carro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Carro carro)
        {
            if (ModelState.IsValid)
            {
                carro.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(carro);
        }

        // Exclusão de carro com Ajax
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var carro = Carro.Procurar(Session, id);
            if (carro != null)
            {
                carro.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Carro não encontrado");
        }

        // Gerar PDF com a lista de carros
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
        public ActionResult DownloadExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("<Richard>");

            var lista = Session["ListaCarro"] as List<Carro>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Carros");
                planilha.Cells[1, 1].Value = "Placa";
                planilha.Cells[1, 2].Value = "Ano";
                planilha.Cells[1, 3].Value = "Cor";
                planilha.Cells[1, 4].Value = "Data de Fabricação";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var Carro = lista[i];
                    planilha.Cells[i + 2, 1].Value = Carro.Placa;
                    planilha.Cells[i + 2, 2].Value = Carro.Ano;
                    planilha.Cells[i + 2, 3].Value = Carro.Cor;
                    planilha.Cells[i + 2, 4].Value = Carro.DataFabricacao.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();

                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Carros.xlsx");
            }
        }
    }
}
