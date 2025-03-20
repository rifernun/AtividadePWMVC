using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            return View(Carro.GerarLista());
        }
        public ActionResult Exibir(int id)
        {
            return View(Carro.GerarLista().ElementAt(id));
        }
    }
}