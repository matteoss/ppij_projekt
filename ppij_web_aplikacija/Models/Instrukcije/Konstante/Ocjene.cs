using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models.Instrukcije.Konstante
{
    public class Ocjene
    {
        private static Ocjene undef = new Ocjene() { vrijednost = 0, tekst = "Nedefinirano" };
        private static Ocjene jedan = new Ocjene() { vrijednost = 1, tekst = "Nedovoljan (1)" };
        private static Ocjene dva = new Ocjene() { vrijednost = 2, tekst = "Dovoljan (2)" };
        private static Ocjene tri = new Ocjene() { vrijednost = 3, tekst = "Dobar (3)" };
        private static Ocjene cetiri = new Ocjene() { vrijednost = 4, tekst = "Vrlo dobar (4)" };
        private static Ocjene pet = new Ocjene() { vrijednost = 5, tekst = "Odličan (5)" };
        public static List<Ocjene> ocjeneTekst = new List<Ocjene>() { undef, jedan, dva, tri, cetiri, pet};

        public int vrijednost { get; set; }
        public String tekst { get; set; }
    }
}