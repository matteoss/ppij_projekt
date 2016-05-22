using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security;

namespace ppij_web_aplikacija.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        [Authorize(Roles="0")]
        public ActionResult Index()
        {
            List<Osoba> listaOsoba = new List<Osoba>();
            using (var database = new ppij_databaseEntities())
            {
                var query = database.Osoba;
                listaOsoba.AddRange(query.ToList<Osoba>());
                ViewBag.Title = "naslov testa";
                ViewBag.lista = listaOsoba;

                /*Termin t;
                for (int dan = 1; dan <= 7; dan++)
                {
                    for (int sat = 0; sat <= 23; sat++)
                    {
                        t = new Termin();
                        t.dan_termin = dan;
                        t.period_termin = sat;
                        t.ID_termin = 100 * dan + sat;
                        database.Termin.Add(t);
                    }
                }*/

                
                //database.SaveChanges();
            }
            return View();
        }
        public string test()
        {
            return "test";
        }
    }
}