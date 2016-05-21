using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ppij_web_aplikacija.Models.Instrukcije.Konstante;
using ppij_web_aplikacija.Models.Instrukcije.Opisnici;

namespace ppij_web_aplikacija.Models.Instrukcije
{
	public class InstrukcijaModel
	{
		public InstrukcijaModel()
		{
			Datum = DateTime.Now;
			OdabraniSatID = 0;
			OdabranoTrajanjeID = 1;
		}

		[DataType(DataType.Date)]
		public DateTime Datum { get; set; }

		[Display(Name = "Sat")]
		public int OdabraniSatID { get; set; }
		public IEnumerable<SelectListItem> Sati
		{
			get { return new SelectList(Konstante.Sat.SATI, "ID", "Naziv"); }
		}

		[Display(Name = "Trajanje")]
		public int OdabranoTrajanjeID { get; set; }
		public IEnumerable<SelectListItem> Trajanja
		{
			get { return new SelectList(Konstante.Trajanje.TRAJANJA, "ID", "Naziv");  }
		}
		public ICollection<OpisInstrukcije> Opisi { get; set; }

		// filtriranje
		public string Ime { get; set; }
		public string Prezime { get; set; }
		public int MinOcjena { get; set; }
		public int MinBrojInstrukcija { get; set; }
		public int CijenaOd { get; set; }
		public int CijenaDo { get; set; }
	}
}