using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Konstante
{
	public class Termin
	{
		private static Termin _00 = new Termin() { ID = 0, Naziv = "00:00" };
		private static Termin _01 = new Termin() { ID = 1, Naziv = "01:00" };
		private static Termin _02 = new Termin() { ID = 2, Naziv = "02:00" };
		private static Termin _03 = new Termin() { ID = 3, Naziv = "03:00" };
		private static Termin _04 = new Termin() { ID = 4, Naziv = "04:00" };
		private static Termin _05 = new Termin() { ID = 5, Naziv = "05:00" };
		private static Termin _06 = new Termin() { ID = 6, Naziv = "06:00" };
		private static Termin _07 = new Termin() { ID = 7, Naziv = "07:00" };
		private static Termin _08 = new Termin() { ID = 8, Naziv = "08:00" };
		private static Termin _09 = new Termin() { ID = 9, Naziv = "09:00" };
		private static Termin _10 = new Termin() { ID = 10, Naziv = "10:00" };
		private static Termin _11 = new Termin() { ID = 11, Naziv = "11:00" };
		private static Termin _12 = new Termin() { ID = 12, Naziv = "12:00" };
		private static Termin _13 = new Termin() { ID = 13, Naziv = "13:00" };
		private static Termin _14 = new Termin() { ID = 14, Naziv = "14:00" };
		private static Termin _15 = new Termin() { ID = 15, Naziv = "15:00" };
		private static Termin _16 = new Termin() { ID = 16, Naziv = "16:00" };
		private static Termin _17 = new Termin() { ID = 17, Naziv = "17:00" };
		private static Termin _18 = new Termin() { ID = 18, Naziv = "18:00" };
		private static Termin _19 = new Termin() { ID = 19, Naziv = "19:00" };
		private static Termin _20 = new Termin() { ID = 20, Naziv = "20:00" };
		private static Termin _21 = new Termin() { ID = 21, Naziv = "21:00" };
		private static Termin _22 = new Termin() { ID = 22, Naziv = "22:00" };
		private static Termin _23 = new Termin() { ID = 23, Naziv = "23:00" };
		public static List<Termin> TERMINI = new List<Termin>() 
		{ _00, _01, _02, _03, _04, _05, _06, _07, _08, _09, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23 };

		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}