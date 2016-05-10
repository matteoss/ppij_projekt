using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Helpers;
using ppij_web_aplikacija.Models;
using System.Diagnostics;

namespace ppij_web_aplikacija.Controllers
{
    public class ProfilController : Controller
    {
        // GET: Profil
        [Authorize]
        public ActionResult Index()
        {
            using (ppij_databaseEntities data = new ppij_databaseEntities())
            {
                ViewBag.Title = User.Identity.Name;
                Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                ViewBag.razinaPrava = osoba.razina_prava;
                Debug.WriteLine("razina prava" + osoba.razina_prava);
                PostavkeModel model = new PostavkeModel();
                model.changePassword = new ChangePasswordBindingModel();
                model.ostalePostavke = new OstalePostavke();
                model.mojeInstrukcije = new MojeInstrukcije();

                var queryKategorije = from kategorija in data.Kategorija
                                      join osobakategorija in data.osoba_kategorija on kategorija.ID_kategorija equals osobakategorija.ID_kategorija
                                      where osobakategorija.ID_osoba == osoba.ID_osoba
                                      select kategorija;
                ViewBag.kategorije = queryKategorije.ToList<Kategorija>();

                ViewBag.termini = data.Termin.Where(t => t.Osoba.Where(o => o.ID_osoba == osoba.ID_osoba).Count() > 0).ToList<Termin>();

                model.trenutniTab = "2";
                if (osoba.razina_prava == 1)
                {
                    model.ostalePostavke.instruktor = true;
                }
                else
                {
                    model.ostalePostavke.instruktor = false;
                }
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Index(Models.PostavkeModel model)
        {
            using (ppij_databaseEntities data = new ppij_databaseEntities())
            {
                Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                Debug.WriteLine("tab: " + model.trenutniTab);
                if (ModelState.IsValid)
                {
                    String trenutniTab = model.trenutniTab;
                    if (trenutniTab.Equals("1"))
                    {
                        if (Crypto.VerifyHashedPassword(osoba.lozinka, model.changePassword.OldPassword + osoba.salt) == false)
                        {
                            ModelState.AddModelError("error_old_password", "pogrešna lozinka");
                            return View();
                        }
                        string salt = Crypto.GenerateSalt(12);
                        osoba.lozinka = Crypto.HashPassword(model.changePassword.NewPassword + salt);
                        osoba.salt = salt;
                    }
                    else if (trenutniTab.Equals("2"))
                    {
                        if (osoba.razina_prava != 0)
                        {
                            if (model.ostalePostavke.instruktor == true)
                            {
                                osoba.razina_prava = 1;
                            }
                            else
                            {
                                osoba.razina_prava = 2;
                            }
                        }
                    }
                    data.SaveChanges();

                    
                    //String alert = model.NewPassword;
                    //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=JavaScript>alert('" + alert + "')</SCRIPT>");
                }
                ViewBag.razinaPrava = osoba.razina_prava;
                var queryKategorije = from kategorija in data.Kategorija
                                      join osobakategorija in data.osoba_kategorija on kategorija.ID_kategorija equals osobakategorija.ID_kategorija
                                      where osobakategorija.ID_osoba == osoba.ID_osoba
                                      select kategorija;
                ViewBag.kategorije = queryKategorije.ToList<Kategorija>();

                ViewBag.termini = data.Termin.Where(t => t.Osoba.Where(o => o.ID_osoba == osoba.ID_osoba).Count() > 0).ToList<Termin>();
                return View(model);
            }
        }
    }


}