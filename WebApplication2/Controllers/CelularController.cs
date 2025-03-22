using System;
using System.Collections.Generic;
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
    }
}