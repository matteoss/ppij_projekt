using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Helpers;
using ppij_web_aplikacija.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using System.Globalization;

namespace ppij_web_aplikacija.Controllers {
    public class ProfilController : Controller {
        // GET: Profil
        [Authorize]
        public ActionResult Index() {
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                ViewBag.Title = User.Identity.Name;
                Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                ViewBag.razinaPrava = osoba.razina_prava;
                //Debug.WriteLine("razina prava" + osoba.razina_prava);
                PostavkeModel model = new PostavkeModel();
                model.changePassword = new ChangePasswordBindingModel();
                model.ostalePostavke = new OstalePostavke();
                model.mojeVlastiteInstrukcije = new MojeInstrukcije();

                var queryPredmeti = from predmet in data.Predmet
                                    join osobaPredmet in data.osoba_predmet on predmet.ID_predmet equals osobaPredmet.ID_predmet
                                    where osobaPredmet.ID_osoba == osoba.ID_osoba
                                    select predmet;
                ViewBag.predmeti = queryPredmeti.ToList<Predmet>();

                model.mojeVlastiteInstrukcije.mojiTermini = osoba.Termin.Select(t => t.ID_termin).ToList().ConvertAll<string>(x => x.ToString());
                model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_instruktor = new List<dogovor_term_osoba>();
                model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_klijent = new List<dogovor_term_osoba>();
                model.mojeVlastiteInstrukcije.mojiPredmeti = new List<OpisanPredmet>();
                model.mojeVlastiteInstrukcije.sveKategorije = new List<Kategorija>();
                model.mojeVlastiteInstrukcije.sveUstanove = new List<Ustanova>();
                model.mojeVlastiteInstrukcije.sviPredmeti = new List<Predmet>();

                foreach (dogovor_termin dogovor in osoba.dogovor_termin.ToList()) {
                    if (dogovor.dogovor_status != 20 && dogovor.dogovor_status != 0 
                        && dogovor.datum_dogovor.Value > DateTime.Now.AddDays(-10)
                        && dogovor.dogovor_status != 3)
                    {
                        //dogovor.datum_dogovor = dogovor.datum_dogovor.Value.AddHours((int)dogovor.Termin.FirstOrDefault().period_termin);
                        //Debug.WriteLine(dogovor.datum_dogovor.Value + " " + (int)dogovor.Termin.FirstOrDefault().period_termin);
                        model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_instruktor.Add(new dogovor_term_osoba() {
                            termin = dogovor,
                            ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().ime_osoba,
                            prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().prezime_osoba,
                            predmet = data.Predmet.Where(k => k.ID_predmet == dogovor.ID_predmet).FirstOrDefault().kratica_predmet,
                            odustani = false,
                            seen = false
                        });
                    }
                }
                foreach (dogovor_termin dogovor in osoba.dogovor_termin1.ToList()) {
                    if (dogovor.dogovor_status != 0 && dogovor.datum_dogovor.Value > DateTime.Now.AddDays(-10) && dogovor.dogovor_status != 2 && dogovor.dogovor_status != 2)
                    {
                        //dogovor.datum_dogovor = dogovor.datum_dogovor.Value.AddHours((int)dogovor.Termin.FirstOrDefault().period_termin);
                        //Debug.WriteLine(dogovor.datum_dogovor.Value + " " + (int)dogovor.Termin.FirstOrDefault().period_termin);
                        model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_klijent.Add(new dogovor_term_osoba()
                        {
                            termin = dogovor,
                            ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().ime_osoba,
                            prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().prezime_osoba,
                            predmet = data.Predmet.Where(k => k.ID_predmet == dogovor.ID_predmet).FirstOrDefault().kratica_predmet,
                            odustani = false,
                            seen = false
                        });
                    }
                }


                #region populate OpisanPredmet
                var detaljiPredmetOsoba = from p in data.Predmet
                                          join op in data.osoba_predmet on p.ID_predmet equals op.ID_predmet
                                          join u in data.Ustanova on p.ID_ustanova equals u.ID_ustanova
                                          join k in data.Kategorija on p.ID_kategorija equals k.ID_kategorija
                                          where osoba.ID_osoba == op.ID_osoba
                                          select new { p.ID_predmet, p.naziv_predmet, p.kratica_predmet, op.cijena, u.ID_ustanova, k.ID_kategorija };

                foreach (var row in detaljiPredmetOsoba) {
                    Ustanova ust = data.Ustanova.Find(row.ID_ustanova);
                    Kategorija kat = data.Kategorija.Find(row.ID_kategorija);
                    model.mojeVlastiteInstrukcije.mojiPredmeti.Add(new OpisanPredmet() {
                        IDpredmet = row.ID_predmet,
                        nazivPredmet = row.naziv_predmet,
                        kraticaPredmet = row.kratica_predmet,
                        cijenaPredmet = row.cijena,
                        IDkategorija = row.ID_kategorija,
                        IDustanova = row.ID_ustanova,
                        ustanova = ust,
                        kategorija = kat
                    });
                }
                #endregion

                #region populate Ustanova Kategorija

                var ustanovaQuery = from u in data.Ustanova
                                    select u;
                var kategorijaQuery = from k in data.Kategorija
                                      select k;
                model.mojeVlastiteInstrukcije.sveUstanove.AddRange(ustanovaQuery.ToList());
                model.mojeVlastiteInstrukcije.sveKategorije.AddRange(kategorijaQuery.ToList());
                #endregion

                #region populate Predmeti
                var predmetiQuery = from p in data.Predmet
                                    select p;

                model.mojeVlastiteInstrukcije.sviPredmeti.AddRange(predmetiQuery.ToList());
                #endregion




                model.mojeVlastiteInstrukcije.popis_kategorija = new List<odabranaKategorija>();
                foreach (Kategorija kateg in data.Kategorija) {
                    odabranaKategorija odabrananadkat = new odabranaKategorija();
                    odabrananadkat.mojiPredmeti = new List<odabranPredmet>();
                    foreach (Predmet pred in data.Predmet.Where(k => k.ID_kategorija == kateg.ID_kategorija)) {
                        odabranPredmet item = new odabranPredmet();
                        item.predmet = pred;
                        if (osoba.osoba_predmet.Where(k => k.ID_predmet == pred.ID_kategorija).Count() == 1) {
                            item.odabran = true;
                        } else {
                            item.odabran = false;
                        }
                        odabrananadkat.mojiPredmeti.Add(item);
                    }
                    odabrananadkat.kategorija_ime = kateg.naziv_kategorija;
                    model.mojeVlastiteInstrukcije.popis_kategorija.Add(odabrananadkat);
                }
                model.mojeVlastiteInstrukcije.MojeLokacijeJson = convertLokacije(data.Lokacija.Where(l => l.Osoba.ID_osoba == osoba.ID_osoba).ToList());
                model.trenutniTab = "11";
                if (osoba.razina_prava == 1) {
                    model.ostalePostavke.instruktor = true;
                } else {
                    model.ostalePostavke.instruktor = false;
                }
                return View(model);
            }
        }






        [HttpPost]
        [Authorize]
        public ActionResult Index(Models.PostavkeModel model) {
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                Debug.WriteLine("tab: " + model.trenutniTab);
                if (ModelState.IsValid) {
                    String trenutniTab = model.trenutniTab;


                    if (trenutniTab.Equals("11") || trenutniTab.Equals("12")) {
                        if (model.mojeVlastiteInstrukcije != null && model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_klijent != null) {
                            foreach (dogovor_term_osoba dto in model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_klijent) {
                                dogovor_termin dogovor = data.dogovor_termin.Where(d => d.ID_dogovor_termin == dto.termin.ID_dogovor_termin).FirstOrDefault();

                                if (dto.seen == true) {
                                    if (dogovor.dogovor_status == 3) {
                                        dogovor.dogovor_status = 0;
                                    } else if (dogovor.dogovor_status == 11) {
                                        dogovor.dogovor_status = 1;
                                    }
                                }
                                if (dto.odustani == true) {
                                    if (dogovor.dogovor_status == 10) {
                                        dogovor.dogovor_status = 20;
                                    } else if (dogovor.dogovor_status == 11 || dogovor.dogovor_status == 1) {
                                        dogovor.dogovor_status = 2;
                                    }
                                    //Debug.WriteLine("odustano od dogovora: " + dogovor.ID_dogovor_termin);
                                }
                            }
                        }

                        if (model.mojeVlastiteInstrukcije != null && model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_instruktor != null) {
                            foreach (dogovor_term_osoba dto in model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_instruktor) {
                                dogovor_termin dogovor = data.dogovor_termin.Where(d => d.ID_dogovor_termin == dto.termin.ID_dogovor_termin).FirstOrDefault();
                                if (dto.seen == true) {
                                    if (dogovor.dogovor_status == 2) {
                                        dogovor.dogovor_status = 0;
                                    } else if (dogovor.dogovor_status == 10) {
                                        dogovor.dogovor_status = 11;
                                    }
                                }
                                if (dto.odustani == true) {
                                    if (dogovor.dogovor_status == 1 || dogovor.dogovor_status == 11 || dogovor.dogovor_status == 10)
                                    {
                                        dogovor.dogovor_status = 3;
                                    }
                                    
                                    Debug.WriteLine("odustano od dogovora: " + dogovor.ID_dogovor_termin);
                                }
                            }
                        }
                    } else if (trenutniTab.Equals("21") || trenutniTab.Equals("22")) {
                        /*foreach (String t in model.mojeVlastiteInstrukcije.mojiTermini)
                        {
                            Debug.WriteLine("post: " + t);
                        }*/
                        model.mojeVlastiteInstrukcije.mojiTermini = model.mojeVlastiteInstrukcije.mojiTermini.FirstOrDefault().Split(',').ToList();
                        List<Termin> toBeDel = new List<Termin>();
                        foreach (Termin s in osoba.Termin) //provjera starih termina - da li su još aktualni
                        {
                            //Debug.WriteLine("check to be deleted: " + s.ID_termin);
                            if (model.mojeVlastiteInstrukcije.mojiTermini.Contains(s.ID_termin.ToString()) == false) {
                                //Debug.WriteLine("to be deleted: " + s.ID_termin);
                                toBeDel.Add(s);
                            }
                        }
                        foreach (Termin t in toBeDel) {
                            osoba.Termin.Remove(t);
                        }
                        data.SaveChanges();
                        foreach (String s in model.mojeVlastiteInstrukcije.mojiTermini) //provjera novih termina - koje treba dodati u bazu
                        {
                            //Debug.WriteLine("check to be added: " + s);
                            if (s.Length > 0) {
                                int id = Int32.Parse(s);
                                if (osoba.Termin.Where(st => st.ID_termin == id).Count() == 0) {
                                    //Debug.WriteLine("to be added: " + s);
                                    Termin ter = data.Termin.Where(t => t.ID_termin == id).FirstOrDefault();
                                    osoba.Termin.Add(ter);
                                }
                            }
                        }
                    } else if (trenutniTab.Equals("4")) {
                        if (Crypto.VerifyHashedPassword(osoba.lozinka, model.changePassword.OldPassword + osoba.salt) == false) {
                            ModelState.AddModelError("error_old_password", "pogrešna lozinka");
                            return View();
                        }
                        string salt = Crypto.GenerateSalt(12);
                        osoba.lozinka = Crypto.HashPassword(model.changePassword.NewPassword + salt);
                        osoba.salt = salt;
                    } else if (trenutniTab.Equals("3")) {
                        if (osoba.razina_prava != 0) {
                            if (model.ostalePostavke.instruktor == true) {
                                osoba.razina_prava = 1;
                            } else {
                                osoba.razina_prava = 2;
                            }
                        }
                        if (model.mojeVlastiteInstrukcije.MojeLokacijeJson != null) {
                            List<lokacijeJsonObject> lokacijeObj = JsonConvert.DeserializeObject<List<lokacijeJsonObject>>(model.mojeVlastiteInstrukcije.MojeLokacijeJson);
                            if (lokacijeObj != null) {

                                List<Lokacija> lokacije = new List<Lokacija>();
                                foreach (lokacijeJsonObject objekt in lokacijeObj) {
                                    Lokacija lok = new Lokacija() {
                                        Geo_sirina = objekt.lat,
                                        Geo_duzina = objekt.lon,
                                        opis = objekt.opis,
                                        ID_instruktor = osoba.ID_osoba,
                                        Osoba = osoba
                                    };
                                    lokacije.Add(lok);
                                }

                                List<Lokacija> toBeDel = new List<Lokacija>();
                                foreach (Lokacija lok in osoba.Lokacija) {
                                    if (lokacije.Where(l => l.Geo_duzina == lok.Geo_duzina && l.Geo_sirina == lok.Geo_sirina).Count() < 1) {
                                        toBeDel.Add(lok);
                                    }
                                }
                                foreach (Lokacija t in toBeDel) {
                                    osoba.Lokacija.Remove(t);
                                }
                                data.SaveChanges();
                                foreach (Lokacija nova in lokacije) {
                                    if (osoba.Lokacija.Where(l => l.Geo_duzina == nova.Geo_duzina && l.Geo_sirina == nova.Geo_sirina).Count() == 0) {
                                        nova.Id = data.Lokacija.Max(l => l.Id) + 1;
                                        osoba.Lokacija.Add(nova);
                                        data.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    data.SaveChanges();
                }

                model.ostalePostavke = new OstalePostavke();
                model.mojeVlastiteInstrukcije = new MojeInstrukcije();
                ViewBag.razinaPrava = osoba.razina_prava;
                var queryPredmeti = from predmet in data.Predmet
                                    join osobaPredmet in data.osoba_predmet on predmet.ID_predmet equals osobaPredmet.ID_predmet
                                    where osobaPredmet.ID_osoba == osoba.ID_osoba
                                    select predmet;
                ViewBag.predmet = queryPredmeti.ToList<Predmet>();

                model.mojeVlastiteInstrukcije.mojiTermini = osoba.Termin.Select(t => t.ID_termin).ToList().ConvertAll<string>(x => x.ToString());
                model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_instruktor = new List<dogovor_term_osoba>();
                model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_klijent = new List<dogovor_term_osoba>();
                model.mojeVlastiteInstrukcije.mojiPredmeti = new List<OpisanPredmet>();
                model.mojeVlastiteInstrukcije.sveKategorije = new List<Kategorija>();
                model.mojeVlastiteInstrukcije.sveUstanove = new List<Ustanova>();
                model.mojeVlastiteInstrukcije.sviPredmeti = new List<Predmet>();



                foreach (dogovor_termin dogovor in osoba.dogovor_termin.ToList()) {
                    if (dogovor.dogovor_status != 20 && dogovor.dogovor_status != 0
                        && dogovor.datum_dogovor.Value > DateTime.Now.AddDays(-10)
                        && dogovor.dogovor_status != 3)
                    {
                        //dogovor.datum_dogovor.Value.AddHours((int)dogovor.Termin.FirstOrDefault().period_termin);
                        //Debug.WriteLine(dogovor.datum_dogovor.Value);
                        model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_instruktor.Add(new dogovor_term_osoba()
                        {
                            termin = dogovor,
                            ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().ime_osoba,
                            prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_klijent).FirstOrDefault().prezime_osoba,
                            predmet = data.Predmet.Where(k => k.ID_predmet == dogovor.ID_predmet).FirstOrDefault().kratica_predmet,
                            odustani = false,
                            seen = false
                        });
                    }
                }
                foreach (dogovor_termin dogovor in osoba.dogovor_termin1.ToList()) {
                    if (dogovor.dogovor_status != 0 && dogovor.datum_dogovor.Value > DateTime.Now.AddDays(-10) && dogovor.dogovor_status != 2 && dogovor.dogovor_status != 2)
                    {
                        //dogovor.datum_dogovor.Value.AddHours((int)dogovor.Termin.OrderBy(o => o.period_termin).FirstOrDefault().period_termin);
                        //Debug.WriteLine(dogovor.datum_dogovor.Value);
                        model.mojeVlastiteInstrukcije.dogovoreni_termini_kao_klijent.Add(new dogovor_term_osoba()
                        {
                            termin = dogovor,
                            ime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().ime_osoba,
                            prezime = data.Osoba.Where(o => o.ID_osoba == dogovor.ID_instruktor).FirstOrDefault().prezime_osoba,
                            predmet = data.Predmet.Where(k => k.ID_predmet == dogovor.ID_predmet).FirstOrDefault().kratica_predmet,
                            odustani = false,
                            seen = false
                        });
                    }
                }


                #region populate OpisanPredmet
                var detaljiPredmetOsoba = from p in data.Predmet
                                          join op in data.osoba_predmet on p.ID_predmet equals op.ID_predmet
                                          join u in data.Ustanova on p.ID_ustanova equals u.ID_ustanova
                                          join k in data.Kategorija on p.ID_kategorija equals k.ID_kategorija
                                          where osoba.ID_osoba == op.ID_osoba
                                          select new { p.ID_predmet, p.naziv_predmet, p.kratica_predmet, op.cijena, u.ID_ustanova, k.ID_kategorija };

                foreach (var row in detaljiPredmetOsoba) {
                    Ustanova ust = data.Ustanova.Find(row.ID_ustanova);
                    Kategorija kat = data.Kategorija.Find(row.ID_kategorija);
                    model.mojeVlastiteInstrukcije.mojiPredmeti.Add(new OpisanPredmet() {
                        IDpredmet = row.ID_predmet,
                        nazivPredmet = row.naziv_predmet,
                        kraticaPredmet = row.kratica_predmet,
                        cijenaPredmet = row.cijena,
                        IDkategorija = row.ID_kategorija,
                        IDustanova = row.ID_ustanova,
                        ustanova = ust,
                        kategorija = kat
                    });
                }
                #endregion

                #region populate Ustanova Kategorija

                var ustanovaQuery = from u in data.Ustanova
                                    select u;
                var kategorijaQuery = from k in data.Kategorija
                                      select k;
                model.mojeVlastiteInstrukcije.sveUstanove.AddRange(ustanovaQuery.ToList());
                model.mojeVlastiteInstrukcije.sveKategorije.AddRange(kategorijaQuery.ToList());
                #endregion

                #region populate Predmeti
                var predmetiQuery = from p in data.Predmet
                                    select p;

                model.mojeVlastiteInstrukcije.sviPredmeti.AddRange(predmetiQuery.ToList());
                #endregion

                model.mojeVlastiteInstrukcije.popis_kategorija = new List<odabranaKategorija>();
                foreach (Kategorija kateg in data.Kategorija) {
                    odabranaKategorija odabrananadkat = new odabranaKategorija();
                    odabrananadkat.mojiPredmeti = new List<odabranPredmet>();
                    foreach (Predmet pred in data.Predmet.Where(k => k.ID_kategorija == kateg.ID_kategorija)) {
                        odabranPredmet item = new odabranPredmet();
                        item.predmet = pred;
                        if (osoba.osoba_predmet.Where(k => k.ID_predmet == pred.ID_kategorija).Count() == 1) {
                            item.odabran = true;
                        } else {
                            item.odabran = false;
                        }
                        odabrananadkat.mojiPredmeti.Add(item);
                    }
                    odabrananadkat.kategorija_ime = kateg.naziv_kategorija;
                    model.mojeVlastiteInstrukcije.popis_kategorija.Add(odabrananadkat);
                }
                model.mojeVlastiteInstrukcije.MojeLokacijeJson = convertLokacije(data.Lokacija.Where(l => l.Osoba.ID_osoba == osoba.ID_osoba).ToList());
                if (osoba.razina_prava == 1) {
                    model.ostalePostavke.instruktor = true;
                } else {
                    model.ostalePostavke.instruktor = false;
                }
                return View(model);
            }
        }




        [Authorize]
        [HttpGet]
        public String notification() {
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                Osoba trenutna = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                int notificationsIns = 0;
                if (trenutna.razina_prava != 2) {
                    notificationsIns = trenutna.dogovor_termin.Where(d => (d.dogovor_status == 2 || d.dogovor_status == 10 ) && d.datum_dogovor > DateTime.Now.AddDays(-10)).Count();
                }
                int notificationsKlij = trenutna.dogovor_termin1.Where(d => (d.dogovor_status == 3 || d.dogovor_status == 11 ) && d.datum_dogovor > DateTime.Now.AddDays(-10)).Count();
                return "{\"klijent\":" + notificationsKlij + ",\"instruktor\":" + notificationsIns + "}";
            }
        }

        #region ajax Brisanje predmeta

        public ActionResult Delete(int? id) {
            ppij_databaseEntities data = new ppij_databaseEntities();
            Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            osoba_predmet relacija = data.osoba_predmet.Find(osoba.ID_osoba, id);
            if (relacija == null) {
                return HttpNotFound();
            }
            return View(relacija);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) {
            try {
                ppij_databaseEntities data = new ppij_databaseEntities();
                Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                osoba_predmet relacija = data.osoba_predmet.Find(osoba.ID_osoba, id);
                data.osoba_predmet.Remove(relacija);
                data.SaveChanges();
            } catch (Exception) {
                return null;
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region ajax stvaranje novog predmeta

        public int? Insert(string nazivPredmet, string kraticaPredmet, string opisPredmet, string IDUstanova, string IDKategorija, string cijenaPredmet) {
            int? idPredmet = null;
            Debug.Write(nazivPredmet);
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                try {
                    Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                    Predmet predmet = new Predmet() {
                        ID_kategorija = int.Parse(IDKategorija),
                        kratica_predmet = kraticaPredmet,
                        naziv_predmet = nazivPredmet,
                        ID_ustanova = int.Parse(IDUstanova)
                    };
                    data.Predmet.Add(predmet);
                    osoba_predmet op = new osoba_predmet() {
                        cijena = decimal.Parse(cijenaPredmet, CultureInfo.InvariantCulture),
                        ID_osoba = osoba.ID_osoba,
                        ID_predmet = predmet.ID_predmet
                    };
                    data.osoba_predmet.Add(op);
                    data.SaveChanges();
                    idPredmet = op.ID_predmet;
                    Debug.Write(idPredmet);
                } catch (Exception) {
                    return null;
                }
            }
            return idPredmet;
        }
        #endregion
        #region ajax dodavanje postojeceg predmeta
        public string InsertExisting(string IDPredmet, string cijenaPredmet) {
            Predmet p;
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                try {
                    p = data.Predmet.Find(int.Parse(IDPredmet));
                    Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                    osoba_predmet op = new osoba_predmet() {
                        cijena = decimal.Parse(cijenaPredmet, CultureInfo.InvariantCulture),
                        ID_osoba = osoba.ID_osoba,
                        ID_predmet = int.Parse(IDPredmet)
                    };
                    data.osoba_predmet.Add(op);
                    data.SaveChanges();
                } catch (System.Data.Entity.Infrastructure.DbUpdateException) {
                    return "-1"; //duplicate status code
                } catch (Exception) {
                    return null;
                }
            }
            return p.naziv_predmet;
        }

        #endregion
        #region ajax promjena cijene
        public bool? UpdatePrice(string IDPredmet, string cijenaPredmet) {
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                try {
                    Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                    osoba_predmet op = data.osoba_predmet.Find(osoba.ID_osoba, int.Parse(IDPredmet));
                    op.cijena = decimal.Parse(cijenaPredmet, CultureInfo.InvariantCulture);
                    data.SaveChanges();
                } catch (Exception) {
                    return null;
                }
            }
            return true;
        }
        #endregion
        public String convertLokacije(List<Lokacija> lokacije) {
            Boolean prvi = true;
            String json = "{\"lokacije\":[";
            foreach (Lokacija lok in lokacije) {
                if (prvi == true) {
                    prvi = false;
                } else {
                    json += ",";
                }
                json += "{";
                json += "\"lat\":" + lok.Geo_sirina + ",";
                json += "\"lon\":" + lok.Geo_duzina + ",";
                json += "\"opis\":\"" + lok.opis + "\",";
                json += "\"id\":" + lok.Id;
                json += "}";
            }
            json += "]}";
            return json;
        }

    }
}