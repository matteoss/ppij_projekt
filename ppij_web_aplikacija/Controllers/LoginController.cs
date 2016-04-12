using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Security;

namespace ppij_web_aplikacija.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            Debug.WriteLine("Index ispis");
            return View();
        }
        
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Potvrdi(Models.OsobaLogin o)
        {
            var database = new ppij_databaseEntities();
            Debug.WriteLine("kor_ime  " + o.korisnicko_ime);
            Debug.WriteLine("lozinka  " + o.lozinka);
            var osoba =  database.Osoba.Where(i => i.korisnicko_ime_osoba == o.korisnicko_ime).FirstOrDefault();
            if (ModelState.IsValid && osoba != null)
            {
                if (String.Compare(o.lozinka, ((Osoba) osoba).lozinka) == 0)
                {
                    Debug.WriteLine("uspjesna prijava");
                    FormsAuthentication.SetAuthCookie(o.korisnicko_ime, o.zapamtiMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Debug.WriteLine("kriva lozinka");
                }
            }
            else
            {
                Debug.WriteLine("nema osobe");
            }
            return RedirectToAction("Index", "Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}