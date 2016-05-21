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
				opisi = opisi.Where(k => k.Kategorija.naziv_kategorija.Contains(model.Trazilica.Trim())).ToList();
			opisi = opisi.OrderByDescending(o => o.BrojInstrukcija).ToList();
			model.Opisi = opisi;

			return View(model);
		}

		public ActionResult Ustanova(UstanovaModel model)
		{
			List<Ustanova> ustanove;
			using (var db = new ppij_databaseEntities())
			{
				ustanove = db.Ustanova.ToList();
			}
			model.Ustanove = ustanove;
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
				opisi = opisi.Where(p => p.Predmet.naziv_predmet.Contains(model.Trazilica.Trim())).ToList();
			opisi = opisi.OrderByDescending(o => o.BrojInstrukcija).ToList();
			model.Opisi = opisi;

			return View(model);
		}

		public ActionResult Instrukcija(InstrukcijaModel model)
		{
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
					//Debug.WriteLine("Termin: " + blok);
				}

				// uzmi sve instruktore koji predaju navedeni predmet
				int ID_predmet = Int32.Parse((string)RouteData.Values["predmet_id"]);
				List<osoba_predmet> instrukcije = db.osoba_predmet.Where(i => i.ID_predmet == ID_predmet).ToList();

				// makni sebe iz liste instruktora (LOL)
				Osoba korisnik = db.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
				instrukcije = instrukcije.Where(i => i.Osoba.ID_osoba != korisnik.ID_osoba).ToList();

				// uzmi sve instruktore koji imaju slobodne SVE odabrane termine
				List<osoba_predmet> slobodne_instrukcije = new List<osoba_predmet>();
				foreach (osoba_predmet instrukcija in instrukcije)
				{
					Osoba instruktor = instrukcija.Osoba;
					List<int> slobodni_termini = instruktor.Termin.Select(t => t.ID_termin).ToList();
					if (TerminiSlobodni(slobodni_termini, termini))
						slobodne_instrukcije.Add(instrukcija);
				}

				// uzmi one instruktore koji nemaju preklapanja sa vec postojecim dogovorenim terminima
				List<osoba_predmet> potpuno_slobodne_instrukcije = new List<osoba_predmet>();
				foreach (osoba_predmet instrukcija in slobodne_instrukcije)
				{
					bool preklapanje = false;
					Osoba instruktor = instrukcija.Osoba;
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
					if (!preklapanje)
						potpuno_slobodne_instrukcije.Add(instrukcija);
				}

				// izracunaj statistiku instruktora
				foreach (osoba_predmet instrukcija in potpuno_slobodne_instrukcije)
				{
					OpisInstrukcije opis = new OpisInstrukcije();
					opis.Instruktor = instrukcija.Osoba;

					// izracunaj prosjecnu ocjenu iz predmeta
					List<int> ocjene = db.dogovor_termin.Where(i => i.ID_instruktor == opis.Instruktor.ID_osoba
															      && i.ID_predmet == ID_predmet
															      && i.dogovor_ocijena != null)
										  .Select(i => (int)i.dogovor_ocijena).ToList();
					opis.Ocjena = 1.0 * ocjene.Sum(x => Convert.ToInt32(x)) / ocjene.Count;
					// nadji broj instrukcija iz predmeta
					opis.BrojInstrukcija = db.dogovor_termin.Where(i => i.ID_instruktor == opis.Instruktor.ID_osoba
																	 && i.ID_predmet == ID_predmet
																	 && i.dogovor_ocijena != null).Count();
					// izracunaj cijenu (trajanje * cijena)
					opis.Cijena = (decimal)instrukcija.cijena * model.OdabranoTrajanjeID;
	
					// stavi listu lokacija u combo box opisa
					opis.Lokacije = instrukcija.Osoba.Lokacija.ToList();

					// provjeri da li si već poslao isti zahtjev tom instruktoru
					int vec_poslan = db.dogovor_termin
						.Where(d => d.Osoba1.korisnicko_ime_osoba == User.Identity.Name)
						.Where(d => d.ID_instruktor == instrukcija.Osoba.ID_osoba)
						.Where(d => d.datum_dogovor == model.Datum)
						.Where(d => d.trajanje == model.OdabranoTrajanjeID).Count();
					if (vec_poslan != 0)
						opis.Status = "POSLAN";
					else
						opis.Status = "POŠALJI";
					opisi.Add(opis);
				}
			}
			// TODO : filtriraj po minimalnoj ocjeni, minimalnom broju instrukcija, rasponu cijene, 
			if (model.Ime != null)
				opisi = opisi.Where(i => i.Instruktor.ime_osoba.Contains(model.Ime.Trim())).ToList();
			if (model.Prezime != null)
				opisi = opisi.Where(i => i.Instruktor.prezime_osoba.Contains(model.Prezime.Trim())).ToList();

			model.Opisi = opisi;
			return View(model);
		}

		public static bool TerminiSlobodni(List<int> a, List<int> b)
		{
			return !b.Except(a).Any();
		}
	}
}