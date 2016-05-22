using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Konstante
{
	public class Trajanje
	{
		private static Trajanje _1 = new Trajanje() { ID = 1, Naziv = "1 sat" };
		private static Trajanje _2 = new Trajanje() { ID = 2, Naziv = "2 sata" };
		private static Trajanje _3 = new Trajanje() { ID = 3, Naziv = "3 sati" };
		private static Trajanje _4 = new Trajanje() { ID = 4, Naziv = "4 sati" };
		private static Trajanje _5 = new Trajanje() { ID = 5, Naziv = "5 sati" };
		private static Trajanje _6 = new Trajanje() { ID = 6, Naziv = "6 sati" };
		public static List<Trajanje> TRAJANJA = new List<Trajanje>()
		{ _1, _2, _3, _4, _5, _6 };

		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}