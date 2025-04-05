using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication2.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            Aluno.GerarLista(Session);

            return View(Session["ListaAluno"] as List<Aluno>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(Aluno.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Aluno aluno)
        {
            {
                Aluno.Procurar(Session, id)?.Excluir(Session);
                aluno.Excluir(Session);

                return RedirectToAction("Listar");
            }
        }


        public ActionResult Create()
        {
            return View(new Aluno());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            aluno.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Edit(int id, Aluno aluno)
        {
            aluno.Editar(Session, id);

            return RedirectToAction("Listar");
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

                var titulo = new Paragraph("Relatório de Alunos", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph("\n"));

                PdfPTable tabela = new PdfPTable(3);
                tabela.WidthPercentage = 100;

                // Largura proporcional das colunas
                float[] larguras = new float[] { 3f, 1.5f, 2f };
                tabela.SetWidths(larguras);

                // Cabeçalhos com fonte em negrito
                var fontCabecalho = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                tabela.AddCell(new PdfPCell(new Phrase("Nome", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("RA", fontCabecalho)));
                tabela.AddCell(new PdfPCell(new Phrase("Data de Nascimento", fontCabecalho)));

                // Conteúdo da tabela
                var fontNormal = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);

                foreach (var aluno in lista)
                {
                    tabela.AddCell(new Phrase(aluno.Nome, fontNormal));
                    tabela.AddCell(new Phrase(aluno.RA, fontNormal));
                    tabela.AddCell(new Phrase(aluno.DataNsc.ToString("dd/MM/yyyy"), fontNormal));
                }

                doc.Add(tabela);
                doc.Close();

                byte[] pdfBytes = ms.ToArray();
                return File(pdfBytes, "application/pdf", "ListaAlunos.pdf");
            }
        }

    }
}