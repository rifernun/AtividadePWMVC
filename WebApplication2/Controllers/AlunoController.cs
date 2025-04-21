using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Drawing;


namespace WebApplication1.Controllers

{

    public class AlunoController : Controller

    {

        public ActionResult Index() => RedirectToAction("Listar");


        public ActionResult Listar()

        {

            Aluno.GerarLista(Session);

            return View(Session["ListaAluno"] as List<Aluno>);

        }


        public ActionResult Create() => View(new Aluno());


        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Create(Aluno aluno)

        {

            if (ModelState.IsValid)

            {

                aluno.Adicionar(Session);

                return RedirectToAction("Listar");
            }

            return View(aluno);

        }

        public ActionResult Edit(int id)

        {

            return View(Aluno.Procurar(Session, id));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                aluno.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(aluno);
        }

        [HttpPost]

        public ActionResult DeleteAjax(int id)
        {
            var aluno = Aluno.Procurar(Session, id);
            if (aluno != null)
            {
                aluno.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Aluno não encontrado");
        }

        public ActionResult GerarPdf()
        {
            var lista = Session["ListaAluno"] as List<Aluno>;

            if (lista == null || lista.Count == 0)
            {
                return Content("Nenhum aluno encontrado na sessão.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Relatório de Alunos", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable tabela = new PdfPTable(3);
                tabela.WidthPercentage = 100;

                float[] larguras = new float[] { 3f, 1.5f, 2f };
                tabela.SetWidths(larguras);

                var fontCabecalho = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD);
                var fontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL);

                tabela.AddCell(new PdfPCell(new Phrase("Nome", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Email", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Data de Nascimento", fontCabecalho)));

                foreach (var aluno in lista)
                {
                    tabela.AddCell(new Phrase(aluno.Nome, fontNormal));
                    tabela.AddCell(new Phrase(aluno.Email, fontNormal));
                    tabela.AddCell(new Phrase(aluno.Datansc.ToString("dd/MM/yyyy"), fontNormal));
                }

                doc.Add(tabela);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaAlunos.pdf");
            }
        }


        public ActionResult DownloadExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("<Richard>");

            var lista = Session["ListaAluno"] as List<Aluno>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Alunos");
                planilha.Cells[1, 1].Value = "Nome";
                planilha.Cells[1, 2].Value = "Email";
                planilha.Cells[1, 3].Value = "Data de Nascimento";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var aluno = lista[i];
                    planilha.Cells[i + 2, 1].Value = aluno.Nome;
                    planilha.Cells[i + 2, 2].Value = aluno.Email;
                    planilha.Cells[i + 2, 3].Value = aluno.Datansc.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();

                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Alunos.xlsx");
            }
        }
    }

}