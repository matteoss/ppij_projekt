using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Security;
using System.Data.Entity.Validation;

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
        [HttpGet]
        public ActionResult Registracija()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registracija(Models.RegistracijaModel model)
        {
            if (ModelState.IsValid)
            {
                Debug.WriteLine(model.ime + " " + model.prezime + " " + model.lozinka + " " + model.email);
                if (model.jeIstaLozinka() == false)
                {
                    
                }
                else
                {
                    using (ppij_databaseEntities data = new ppij_databaseEntities())
                    {
                        Osoba osoba = new Osoba();
                        osoba.ime_osoba = model.ime;
                        osoba.prezime_osoba = model.prezime;
                        osoba.korisnicko_ime_osoba = model.korisnicko_ime;
                        osoba.lozinka = model.lozinka;
                        if (model.jeInstruktor == true)
                        {
                            osoba.razina_prava = 1;
                        }
                        else
                        {
                            osoba.razina_prava = 2;
                        }
                        osoba.email_osoba = model.email;
                        osoba.lokacija_x = null;
                        osoba.lokacija_y = null;
                        osoba.ID_osoba = data.Osoba.OrderByDescending(o => o.ID_osoba).FirstOrDefault().ID_osoba + 1;
                        osoba.dogovor_termin = null;
                        osoba.dogovor_termin1 = null;
                        osoba.osoba_kategorija = null;
                        osoba.Termin = null;
                        data.Osoba.Add(osoba);
                        Debug.WriteLine(osoba.ID_osoba + " " + osoba.ime_osoba + " " + osoba.prezime_osoba + " " + osoba.korisnicko_ime_osoba + " " + osoba.lozinka);
                        try
                        {
                            data.SaveChanges();
                        }
                        catch(DbEntityValidationException deve)
                        {
                            foreach (var validationErrors in deve.EntityValidationErrors)
                            {
                                foreach (var error in validationErrors.ValidationErrors)
                                {
                                    Trace.TraceInformation("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);
                                }
                            }
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}