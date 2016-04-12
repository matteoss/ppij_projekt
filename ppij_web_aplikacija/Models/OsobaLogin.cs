using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models
{
    public class OsobaLogin
    {
        [Required]
        [Display(Name= "korisnicko ime")]
        public string korisnicko_ime { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="lozinka")]
        public string lozinka { get; set; }
        public Boolean zapamtiMe { get; set; }
        public Boolean JeIspravna(string korisnicko_ime_p, string lozinka_p)
        {
            using (ppij_databaseEntities data = new ppij_databaseEntities())
            {
                var v = data.Osoba.Where(i=>i.korisnicko_ime_osoba == korisnicko_ime_p);
                if (v != null)
                {
                    if(String.Compare( ((Osoba) v).lozinka, lozinka_p) == 0){
                        return true;
                    }
                }
            }
            return false;
        }
    }
}