using ppij_web_aplikacija.Models;
using System;
using System.Collections.Generic;
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
			return View(model);
		}
	}

}