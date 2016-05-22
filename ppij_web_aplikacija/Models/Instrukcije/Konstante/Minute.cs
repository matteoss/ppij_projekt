using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Konstante
{
	public class Minute
	{
		private static Minute _00 = new Minute() { ID = 0, Naziv = "00" };
		private static Minute _10 = new Minute() { ID = 10, Naziv = "10" };
		private static Minute _20 = new Minute() { ID = 20, Naziv = "20" };
		private static Minute _30 = new Minute() { ID = 30, Naziv = "30" };
		private static Minute _40 = new Minute() { ID = 40, Naziv = "40" };
		private static Minute _50 = new Minute() { ID = 50, Naziv = "50" };

		public static List<Minute> MINUTE = new List<Minute>()
		{ _00, _10, _20, _30, _40, _50 };

		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}