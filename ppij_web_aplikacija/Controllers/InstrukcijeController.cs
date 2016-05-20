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
					Debug.WriteLine("Termin: " + blok);
				}

				// uzmi sve instruktore koji predaju navedeni predmet
				int ID_predmet = Int32.Parse((string)RouteData.Values["predmet_id"]);
				List<osoba_predmet> instrukcije = db.osoba_predmet.Where(i => i.ID_predmet == ID_predmet).ToList();

				// uzmi sve instruktore koji imaju slobodne SVE termine
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
					Osoba instruktor = instrukcija.Osoba;
					// uzmi sve dogovorene instrukcije
					List<dogovor_termin> dogovoreni_termini = instruktor.dogovor_termin
						.Where(d => d.dogovor_status == 1 || d.dogovor_status == 11).ToList();
					foreach (dogovor_termin dogovoren_termin in dogovoreni_termini)
					{
						DateTime pocetak_termina = (DateTime)dogovoren_termin.datum_dogovor; // ne bi trebao biti null, ikada
						// CONTINUE HERE ---> DateTime zavrsetak_termina = pocetak_termina.AddHours(dogovoren_termin.Termin.Count)
					}
				}

			}
			return View(model);
		}

		public static bool TerminiSlobodni(List<int> a, List<int> b)
		{
			return !b.Except(a).Any();
		}
	}
}