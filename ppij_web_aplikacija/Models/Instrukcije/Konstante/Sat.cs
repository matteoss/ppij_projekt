using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Konstante
{
	public class Sat
	{
		private static Sat _00 = new Sat() { ID = 0, Naziv = "00" };
		private static Sat _01 = new Sat() { ID = 1, Naziv = "01" };
		private static Sat _02 = new Sat() { ID = 2, Naziv = "02" };
		private static Sat _03 = new Sat() { ID = 3, Naziv = "03" };
		private static Sat _04 = new Sat() { ID = 4, Naziv = "04" };
		private static Sat _05 = new Sat() { ID = 5, Naziv = "05" };
		private static Sat _06 = new Sat() { ID = 6, Naziv = "06" };
		private static Sat _07 = new Sat() { ID = 7, Naziv = "07" };
		private static Sat _08 = new Sat() { ID = 8, Naziv = "08" };
		private static Sat _09 = new Sat() { ID = 9, Naziv = "09" };
		private static Sat _10 = new Sat() { ID = 10, Naziv = "10" };
		private static Sat _11 = new Sat() { ID = 11, Naziv = "11" };
		private static Sat _12 = new Sat() { ID = 12, Naziv = "12" };
		private static Sat _13 = new Sat() { ID = 13, Naziv = "13" };
		private static Sat _14 = new Sat() { ID = 14, Naziv = "14" };
		private static Sat _15 = new Sat() { ID = 15, Naziv = "15" };
		private static Sat _16 = new Sat() { ID = 16, Naziv = "16" };
		private static Sat _17 = new Sat() { ID = 17, Naziv = "17" };
		private static Sat _18 = new Sat() { ID = 18, Naziv = "18" };
		private static Sat _19 = new Sat() { ID = 19, Naziv = "19" };
		private static Sat _20 = new Sat() { ID = 20, Naziv = "20" };
		private static Sat _21 = new Sat() { ID = 21, Naziv = "21" };
		private static Sat _22 = new Sat() { ID = 22, Naziv = "22" };
		private static Sat _23 = new Sat() { ID = 23, Naziv = "23" };
		public static List<Sat> SATI = new List<Sat>() 
		{ _00, _01, _02, _03, _04, _05, _06, _07, _08, _09, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23 };

		public int ID { get; set; }
		public String Naziv { get; set; }
	}
}