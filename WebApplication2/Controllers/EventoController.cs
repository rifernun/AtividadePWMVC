using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

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
    }
}