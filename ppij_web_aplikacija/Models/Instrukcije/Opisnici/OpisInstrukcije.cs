using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Opisnici
{
	public class OpisInstrukcije
	{
		public Osoba Instruktor { get; set; }
		public double Ocjena { get; set; }
		public int BrojInstrukcija { get; set; }
		public decimal Cijena { get; set; }
		public string Status { get; set; }
	}
}