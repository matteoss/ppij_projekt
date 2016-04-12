using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ppij_web_aplikacija.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        [Authorize]
        public ActionResult Index()
        {
            List<Osoba> listaOsoba = new List<Osoba>();
            var database = new ppij_databaseEntities();
            var query = database.Osoba;
            listaOsoba.AddRange(query.ToList<Osoba>());
            ViewBag.Title = "naslov testa";
            ViewBag.lista = listaOsoba;
            return View();
        }
        public string test()
        {
            return "test";
        }
    }
}