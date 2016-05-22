using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Konstante
{
	public class Dan
	{
		private static Dan PONEDJELJAK = new Dan() { ID = 1, Naziv = "Ponedjeljak" };
		private static Dan UTORAK = new Dan() { ID = 2, Naziv = "Utorak" };
		private static Dan SRIJEDA = new Dan() { ID = 3, Naziv = "Srijeda" };
		private static Dan CETVRTAK = new Dan() { ID = 4, Naziv = "Četvrtak" };
		private static Dan PETAK = new Dan() { ID = 5, Naziv = "Petak" };
		private static Dan SUBOTA = new Dan() { ID = 6, Naziv = "Subota" };
		private static Dan NEDJELJA = new Dan() { ID = 7, Naziv = "Nedjelja" };
		public static List<Dan> DANI = new List<Dan>()
		{ PONEDJELJAK, UTORAK, SRIJEDA, CETVRTAK, PETAK, SUBOTA, NEDJELJA };

		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}