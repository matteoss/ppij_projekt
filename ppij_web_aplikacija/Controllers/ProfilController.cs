using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Helpers;
using ppij_web_aplikacija.Models;
using System.Diagnostics;
using Newtonsoft.Json;

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

                model.mojeInstrukcije.mojiTermini = osoba.Termin.Select(t => t.ID_termin).ToList().ConvertAll<string>(x => x.ToString());
                model.mojeInstrukcije.dogovoreni_termini_kao_instruktor = new List<dogovor_term_osoba>();
                model.mojeInstrukcije.dogovoreni_termini_kao_klijent = new List<dogovor_term_osoba>();

                foreach(dogovor_termin dogovor in osoba.dogovor_termin.ToList()){
                    model.mojeInstrukcije.dogovoreni_termini_kao_instruktor.Add(new dogovor_term_osoba()
                    {
                        termin = dogovor,
                        ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().ime_osoba,
                        prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().prezime_osoba,
                        kategorija = data.Kategorija.Where(k => k.ID_kategorija == dogovor.ID_kategorija).FirstOrDefault().kratica_kategorija,
                        odustani = false
                    });
                }
                foreach (dogovor_termin dogovor in osoba.dogovor_termin1.ToList())
                {
                    model.mojeInstrukcije.dogovoreni_termini_kao_klijent.Add(new dogovor_term_osoba()
                    {
                        termin = dogovor,
                        ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().ime_osoba,
                        prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().prezime_osoba,
                        kategorija = data.Kategorija.Where(k => k.ID_kategorija == dogovor.ID_kategorija).FirstOrDefault().kratica_kategorija,
                        odustani = false
                    });
                }

                model.trenutniTab = "1";
                if (osoba.razina_prava == 1)
                {
                    model.ostalePostavke.instruktor = true;
                }
                else
                {
                    model.ostalePostavke.instruktor = false;
                }
                foreach (String t in model.mojeInstrukcije.mojiTermini)
                {
                    Debug.WriteLine("get: " + t);
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
                        if (model.mojeInstrukcije.dogovoreni_termini_kao_klijent != null)
                        {
                            foreach (dogovor_term_osoba dto in model.mojeInstrukcije.dogovoreni_termini_kao_klijent)
                            {
                                if (dto.odustani == true)
                                {
                                    dogovor_termin dogovor = data.dogovor_termin.Where(d => d.ID_dogovor_termin == dto.termin.ID_dogovor_termin).FirstOrDefault();
                                    if (dogovor.dogovor_status == 1)
                                    {
                                        dogovor.dogovor_status = 2;
                                    }
                                    else if (dogovor.dogovor_status == 3)
                                    {
                                        dogovor.dogovor_status = 0;
                                    }
                                    Debug.WriteLine("odustano od dogovora: " + dogovor.ID_dogovor_termin);
                                }
                            }
                        }

                        if (model.mojeInstrukcije.dogovoreni_termini_kao_instruktor != null)
                        {
                            foreach (dogovor_term_osoba dto in model.mojeInstrukcije.dogovoreni_termini_kao_instruktor)
                            {
                                if (dto.odustani == true)
                                {
                                    dogovor_termin dogovor = data.dogovor_termin.Where(d => d.ID_dogovor_termin == dto.termin.ID_dogovor_termin).FirstOrDefault();
                                    if (dogovor.dogovor_status == 1)
                                    {
                                        dogovor.dogovor_status = 3;
                                    }
                                    else if (dogovor.dogovor_status == 2)
                                    {
                                        dogovor.dogovor_status = 0;
                                    }
                                    Debug.WriteLine("odustano od dogovora: " + dogovor.ID_dogovor_termin);
                                }
                            }
                        }
                    }


                    else if (trenutniTab.Equals("2"))
                    {
                        foreach (String t in model.mojeInstrukcije.mojiTermini)
                        {
                            Debug.WriteLine("post: " + t);
                        }
                        model.mojeInstrukcije.mojiTermini = model.mojeInstrukcije.mojiTermini.FirstOrDefault().Split(',').ToList();
                        List<Termin> toBeDel = new List<Termin>();
                        foreach (Termin s in osoba.Termin) //provjera starih termina - da li su još aktualni
                        {
                            Debug.WriteLine("check to be deleted: " + s.ID_termin);
                            if (model.mojeInstrukcije.mojiTermini.Contains(s.ID_termin.ToString()) == false)
                            {
                                Debug.WriteLine("to be deleted: " + s.ID_termin);
                                toBeDel.Add(s);
                            }
                        }
                        foreach(Termin t in toBeDel)
                        {
                            osoba.Termin.Remove(t);
                        }
                        data.SaveChanges();
                        foreach (String s in model.mojeInstrukcije.mojiTermini) //provjera novih termina - koje treba dodati u bazu
                        {
                            Debug.WriteLine("check to be added: " + s);
                            if (s.Length > 0)
                            {
                                int id = Int32.Parse(s);
                                if (osoba.Termin.Where(st => st.ID_termin == id).Count() == 0)
                                {
                                    Debug.WriteLine("to be added: " + s);
                                    Termin ter = data.Termin.Where(t => t.ID_termin == id).FirstOrDefault();
                                    osoba.Termin.Add(ter);
                                }
                            }
                        }
                    }



                    else if (trenutniTab.Equals("4"))
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



                    else if (trenutniTab.Equals("3"))
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
                }

                model.ostalePostavke = new OstalePostavke();
                model.mojeInstrukcije = new MojeInstrukcije();
                ViewBag.razinaPrava = osoba.razina_prava;
                var queryKategorije = from kategorija in data.Kategorija
                                      join osobakategorija in data.osoba_kategorija on kategorija.ID_kategorija equals osobakategorija.ID_kategorija
                                      where osobakategorija.ID_osoba == osoba.ID_osoba
                                      select kategorija;
                ViewBag.kategorije = queryKategorije.ToList<Kategorija>();

                model.mojeInstrukcije.mojiTermini = osoba.Termin.Select(t => t.ID_termin).ToList().ConvertAll<string>(x => x.ToString());
                model.mojeInstrukcije.dogovoreni_termini_kao_instruktor = new List<dogovor_term_osoba>();
                model.mojeInstrukcije.dogovoreni_termini_kao_klijent = new List<dogovor_term_osoba>();
                foreach (dogovor_termin dogovor in osoba.dogovor_termin.ToList())
                {
                    model.mojeInstrukcije.dogovoreni_termini_kao_instruktor.Add(new dogovor_term_osoba()
                    {
                        termin = dogovor,
                        ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().ime_osoba,
                        prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().prezime_osoba,
                        kategorija = data.Kategorija.Where(k => k.ID_kategorija == dogovor.ID_kategorija).FirstOrDefault().kratica_kategorija,
                        odustani = false
                    });
                }
                foreach (dogovor_termin dogovor in osoba.dogovor_termin1.ToList())
                {
                    model.mojeInstrukcije.dogovoreni_termini_kao_klijent.Add(new dogovor_term_osoba()
                    {
                        termin = dogovor,
                        ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().ime_osoba,
                        prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().prezime_osoba,
                        kategorija = data.Kategorija.Where(k => k.ID_kategorija == dogovor.ID_kategorija).FirstOrDefault().kratica_kategorija,
                        odustani = false
                    });
                }
                if (osoba.razina_prava == 1)
                {
                    model.ostalePostavke.instruktor = true;
                }
                else
                {
                    model.ostalePostavke.instruktor = false;
                }
                foreach (String t in model.mojeInstrukcije.mojiTermini)
                {
                    Debug.WriteLine("get: " + t);
                }
                return View(model);
            }
        }
    }
}