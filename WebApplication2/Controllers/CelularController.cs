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
            return View(Celular.GerarLista());
        }
        public ActionResult Exibir(int id)
        {
            return View(Celular.GerarLista().ElementAt(id));
        }
    }
}