using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ppij_web_aplikacija.Models.Instrukcije.Konstante;

namespace ppij_web_aplikacija.Models.Instrukcije
{
	public class InstrukcijaModel
	{
		public InstrukcijaModel()
		{
			Datum = DateTime.Now;
		}

		[DataType(DataType.Date)]
		public DateTime Datum { get; set; }

		[Display(Name = "Termin")]
		public int OdabraniTerminID { get; set; }
		public IEnumerable<SelectListItem> Termini
		{
			get { return new SelectList(Konstante.Termin.TERMINI, "ID", "Naziv"); }
		}

		[Display(Name = "Trajanje")]
		public int OdabranoTrajanjeID { get; set; }
		public IEnumerable<SelectListItem> Trajanja
		{
			get { return new SelectList(Konstante.Trajanje.TRAJANJA, "ID", "Naziv");  }
		}

		public string Ime { get; set; }
		public string Prezime { get; set; }
		public double OcjenaOd { get; set; }
	}
}