using ppij_web_aplikacija.Models.Instrukcije;
using ppij_web_aplikacija.Models.Instrukcije.Opisnici;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ppij_web_aplikacija.Controllers
{
	[Authorize]
	public class InstrukcijeController : Controller
	{

		public ActionResult Kategorija(KategorijaModel model)
		{
			List<OpisKategorije> opisi = new List<OpisKategorije>();
			using (var db = new ppij_databaseEntities())
			{
				ICollection<Kategorija> kategorije = db.Kategorija.ToList();
				foreach (Kategorija kategorija in kategorije)
				{
					OpisKategorije opis = new OpisKategorije();
					opis.Kategorija = kategorija;
					opis.BrojInstrukcija = 0;
					ICollection<Predmet> predmeti = db.Predmet.Where(p => p.ID_kategorija == kategorija.ID_kategorija).ToList();
					foreach (Predmet predmet in predmeti)
						foreach (osoba_predmet instrukcija in predmet.osoba_predmet)
							opis.BrojInstrukcija++;
					opisi.Add(opis);
				}
			}
			if (model.Trazilica != null)
				opisi = opisi.Where(k => k.Kategorija.naziv_kategorija.ToUpper().Contains(model.Trazilica.Trim().ToUpper())).ToList();
			opisi = opisi.OrderByDescending(o => o.BrojInstrukcija).ToList();
			model.Opisi = opisi;

			return View(model);
		}

		public ActionResult Ustanova(UstanovaModel model)
		{
			List<OpisUstanove> opisi = new List<OpisUstanove>();
			using (var db = new ppij_databaseEntities())
			{
				List<Ustanova> ustanove = db.Ustanova.ToList();
				foreach (Ustanova ustanova in ustanove)
				{
					OpisUstanove opis = new OpisUstanove();
					opis.Ustanova = ustanova;
					opis.BrojInstrukcija = 0;
					int ID_kategorija = Int32.Parse((string)RouteData.Values["kategorija_id"]);
					int ID_ustanova = ustanova.ID_ustanova;
					ICollection<Predmet> predmeti = db.Predmet.Where(p => p.ID_kategorija == ID_kategorija
																	   && p.ID_ustanova == ID_ustanova).ToList();
					foreach (Predmet predmet in predmeti)
						foreach (osoba_predmet instrukcija in predmet.osoba_predmet)
							opis.BrojInstrukcija++;
					opisi.Add(opis);
				}
			}
			model.Opisi = opisi;
			return View(model);
		}

		public ActionResult Predmet(PredmetModel model)
		{
			List<OpisPredmeta> opisi = new List<OpisPredmeta>();
			using (var db = new ppij_databaseEntities())
			{
				int ID_kategorija = Int32.Parse((string)RouteData.Values["kategorija_id"]);
				int ID_ustanova = Int32.Parse((string)RouteData.Values["ustanova_id"]);
				ICollection<Predmet> predmeti = db.Predmet.Where(p => p.ID_kategorija == ID_kategorija)
														  .Where(p => p.ID_ustanova == ID_ustanova)
														  .ToList();
				foreach (Predmet predmet in predmeti)
				{
					OpisPredmeta opis = new OpisPredmeta();
					opis.Predmet = predmet;
					opis.BrojInstrukcija = predmet.osoba_predmet.Count();
					opisi.Add(opis);
				}
			}
			if (model.Trazilica != null)
				opisi = opisi.Where(p => p.Predmet.naziv_predmet.ToUpper().Contains(model.Trazilica.Trim().ToUpper())).ToList();
			opisi = opisi.OrderByDescending(o => o.BrojInstrukcija).ToList();
			model.Opisi = opisi;

			return View(model);
		}

		public ActionResult Instrukcija(InstrukcijaModel model, string naredba)
		{
			// pohrani novi zahtjev
			if (naredba != null && naredba != "Traži")
			{
				using (var db = new ppij_databaseEntities())
				{
					dogovor_termin zahtjev = new dogovor_termin();
					int ID_instruktor = Int32.Parse(naredba);
					string id = "lokacija:" + ID_instruktor;
					int ID_lokacija = Int32.Parse(Request.Form[id].ToString());

					zahtjev.ID_dogovor_termin = db.dogovor_termin.Select(d => d.ID_dogovor_termin).Max() + 1;
					zahtjev.dogovor_status = 10;
					zahtjev.dogovor_ocijena = null;
					zahtjev.ID_instruktor = ID_instruktor;
					zahtjev.ID_klijent = db.Osoba.First(o => o.korisnicko_ime_osoba == User.Identity.Name).ID_osoba;
					zahtjev.datum_dogovor = model.Datum.AddHours(model.OdabraniSatID);
					zahtjev.ID_predmet = Int32.Parse((string)RouteData.Values["predmet_id"]);
					zahtjev.trajanje = model.OdabranoTrajanjeID;
					zahtjev.ID_lokacija = ID_lokacija;
					db.dogovor_termin.Add(zahtjev);
					db.SaveChanges();
				}
			}

			List<OpisInstrukcije> opisi = new List<OpisInstrukcije>();
			using (var db = new ppij_databaseEntities())
			{
				// datetime pocetka i zavrsetka
				DateTime pocetak = new DateTime(model.Datum.Year, model.Datum.Month, model.Datum.Day, model.OdabraniSatID, 0, 0);
				DateTime zavrsetak = pocetak.AddHours(model.OdabranoTrajanjeID);

				// lista svih termina koji obuhvaćaju interval [pocetak, zavrsetak]
				List<int> termini = new List<int>();
				for (int i = 0; i < model.OdabranoTrajanjeID; i++)
				{
					DateTime termin = pocetak.AddHours(i);
					int dan = (int)termin.DayOfWeek;
					if (dan == 0)
						dan += 7; // pretvori nedjelju iz 0 u 7
					int sat = termin.Hour;
					int blok = (dan * 100 + sat);
					termini.Add(blok);
				}

				// uzmi sve instruktore koji predaju navedeni predmet
				int ID_predmet = Int32.Parse((string)RouteData.Values["predmet_id"]);
				List<osoba_predmet> instrukcije = db.osoba_predmet.Where(i => i.ID_predmet == ID_predmet).ToList();
				model.Predmet = db.Predmet.First(p => p.ID_predmet == ID_predmet);

				// makni sebe iz liste instruktora (LOL)
				Osoba korisnik = db.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
				instrukcije = instrukcije.Where(i => i.Osoba.ID_osoba != korisnik.ID_osoba).ToList();

				// izracunaj opise instruktora
				foreach (osoba_predmet instrukcija in instrukcije)
				{
					OpisInstrukcije opis = new OpisInstrukcije();
					opis.Instruktor = instrukcija.Osoba;

					// pogledaj koji instruktori imaju barem jedan termin postavljen kao ne slobodan
					Osoba instruktor = instrukcija.Osoba;
					List<int> slobodni_termini = instruktor.Termin.Select(t => t.ID_termin).ToList();
					if (!TerminiSlobodni(slobodni_termini, termini))
						opis.Status = "NEDOSTUPAN";

					if (opis.Status == null)
					{
						// pogledaj koji instruktori imaju preklapanja sa vec postojecim dogovorenim terminima
						bool preklapanje = false;
						List<dogovor_termin> dogovoreni_termini = instruktor.dogovor_termin
							.Where(d => d.dogovor_status == 1 || d.dogovor_status == 11).ToList();
						foreach (dogovor_termin dogovoren_termin in dogovoreni_termini)
						{
							DateTime pocetak_termina = (DateTime)dogovoren_termin.datum_dogovor;
							DateTime zavrsetak_termina = pocetak_termina.AddHours((int)dogovoren_termin.trajanje);
							if ((pocetak >= pocetak_termina && pocetak < zavrsetak_termina)
							  || (zavrsetak > pocetak_termina && zavrsetak <= zavrsetak_termina)
							  || (pocetak <= pocetak_termina && zavrsetak >= zavrsetak_termina))
							{
								preklapanje = true;
								break;
							}
						}
						if (preklapanje)
							opis.Status = "REZERVIRAN";
					}

					if (opis.Status == null) // POPRAVI, nešt ne radi
					{
						// provjeri da li je već isti zahtjev tom instruktoru
						DateTime termin = model.Datum.AddHours(model.OdabraniSatID);
						int vec_poslan = db.dogovor_termin
							.Where(d => d.Osoba1.korisnicko_ime_osoba == User.Identity.Name)
							.Where(d => d.ID_instruktor == instrukcija.Osoba.ID_osoba)
							.Where(d => d.datum_dogovor == termin)
							.Where(d => d.trajanje == model.OdabranoTrajanjeID)
							.Where(d => d.dogovor_status == 10).Count(); // TODO još dodaj one druge statuse
						if (vec_poslan != 0)
							opis.Status = "POSLAN";
						else
							opis.Status = "SLOBODAN";
					}

					// izracunaj prosjecnu ocjenu iz predmeta
					List<int> ocjene = db.dogovor_termin.Where(i => i.ID_instruktor == opis.Instruktor.ID_osoba
																  && i.ID_predmet == ID_predmet
																  && i.dogovor_ocijena != null)
										  .Select(i => (int)i.dogovor_ocijena).ToList();
					opis.Ocjena = 1.0 * ocjene.Sum(x => Convert.ToInt32(x)) / ocjene.Count;
					if (double.IsNaN(opis.Ocjena))
						opis.Ocjena = 0;
					// nadji broj instrukcija iz predmeta
					opis.BrojInstrukcija = db.dogovor_termin.Where(i => i.ID_instruktor == opis.Instruktor.ID_osoba
																	 && i.ID_predmet == ID_predmet
																	 && i.dogovor_ocijena != null).Count();
					// izracunaj cijenu (trajanje * cijena)
					opis.Cijena = (decimal)instrukcija.cijena * model.OdabranoTrajanjeID;

					// stavi listu lokacija u combo box opisa
					opis.Lokacije = instrukcija.Osoba.Lokacija.ToList();

					opisi.Add(opis);
				}
			}
			// TODO : filtriraj po minimalnoj ocjeni, minimalnom broju instrukcija, rasponu cijene, 
			if (model.Ime != null)
				opisi = opisi.Where(i => i.Instruktor.ime_osoba.ToUpper().Contains(model.Ime.Trim().ToUpper())).ToList();
			if (model.Prezime != null)
				opisi = opisi.Where(i => i.Instruktor.prezime_osoba.ToUpper().Contains(model.Prezime.Trim().ToUpper())).ToList();
			opisi = opisi.Where(i => i.BrojInstrukcija >= model.BrojInstrukcija && i.Ocjena >= model.Ocjena).ToList();
			model.Opisi = opisi;

			return View(model);
		}

		public static bool TerminiSlobodni(List<int> a, List<int> b)
		{
			return !b.Except(a).Any();
		}







    }
}