using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ppij_web_aplikacija.Models
{
    public class RegistracijaModel
    {
        [Required(ErrorMessage="Nedostaje ime")]
        public string ime { get; set; }
        [Required(ErrorMessage = "Nedostaje prezime")]
        public string prezime { get; set; }
        [Required(ErrorMessage = "Nedostaje korisničko ime")]
        public string korisnicko_ime { get; set; }
        [Required(ErrorMessage = "Nedostaje lozinka")]
        [DataType(DataType.Password)]
        public string lozinka { get; set; }
        [Required(ErrorMessage = "Nedostaje lozinka")]
        [DataType(DataType.Password)]
        public string lozinka_ponovljeno { get; set; }
        [Required(ErrorMessage = "Nedostaje email")]
        public string email { get; set; }
        [Required]
        public Boolean jeInstruktor { get; set; }
        public Boolean jeIstaLozinka()
        {
            if(String.Compare(lozinka, lozinka_ponovljeno) == 0){
                return true;
            }
            else{
                return false;
            }
        }
    }
}