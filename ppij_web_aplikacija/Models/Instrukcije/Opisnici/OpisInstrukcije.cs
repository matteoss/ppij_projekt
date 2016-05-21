using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ppij_web_aplikacija.Models.Instrukcije.Opisnici
{
	public class OpisInstrukcije
	{
		public Osoba Instruktor { get; set; }
		public double Ocjena { get; set; }
		public int BrojInstrukcija { get; set; }
		public decimal Cijena { get; set; }

		[Display(Name = "Lokacija")]
		public int OdabranaLokacijaID { get; set; }
		public List<Lokacija> Lokacije {get; set; }
		public IEnumerable<SelectListItem> SelectListLokacija
		{
			get { return new SelectList(Lokacije, "Id", "opis"); }
		}
		public string Status { get; set; }
	}
}