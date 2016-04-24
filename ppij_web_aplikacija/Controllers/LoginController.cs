﻿using System;
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
            using(ppij_databaseEntities database = new ppij_databaseEntities()){
                Debug.WriteLine("kor_ime  " + o.korisnicko_ime);
                Debug.WriteLine("lozinka  " + o.lozinka);
                var osoba =  database.Osoba.Where(i => i.korisnicko_ime_osoba == o.korisnicko_ime).FirstOrDefault();
                if (ModelState.IsValid)
                    if (osoba != null)
                    {
                        {
                            if (String.Compare(o.lozinka, ((Osoba)osoba).lozinka) == 0)
                            {
                                Debug.WriteLine("uspjesna prijava");
                                FormsAuthentication.SetAuthCookie(o.korisnicko_ime, false);
                                return RedirectToAction("Index", "Profil");
                            }
                            else
                            {
                                Debug.WriteLine("kriva lozinka");
                                ModelState.AddModelError("error_loz", "Pogrešna Lozinka");
                            }
                        }
                    }
                    else
                    {
                       ModelState.AddModelError("error_kor_ime", "Nepostojeće korisničko ime");
                    }
                else
                {
                    Debug.WriteLine("model not valid");
                }
            }
            return View();
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
        public ActionResult Registracija(Models.RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                Debug.WriteLine(model.ime + " " + model.prezime + " " + model.Password + " " + model.Email);
                
                using (ppij_databaseEntities data = new ppij_databaseEntities())
                {
                    Osoba osoba = new Osoba();
                    osoba.ime_osoba = model.ime;
                    osoba.prezime_osoba = model.prezime;
                    osoba.korisnicko_ime_osoba = model.korisnicko_ime;
                    osoba.lozinka = model.Password;
                    if (model.jeInstruktor == true)
                    {
                        osoba.razina_prava = 1;
                    }
                    else
                    {
                        osoba.razina_prava = 2;
                    }
                    osoba.email_osoba = model.Email;
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
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


    }
}