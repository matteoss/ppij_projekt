using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ppij_web_aplikacija.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class PostavkeModel
    {
        public MojeInstrukcije mojeVlastiteInstrukcije { get; set; }
        public ChangePasswordBindingModel changePassword { get; set; }
        public OstalePostavke ostalePostavke { get; set; }
        public String trenutniTab { get; set; }
    }

    public class OstalePostavke
    {
        public Boolean instruktor { get; set; }
    }


    public class MojeInstrukcije
    {
        public List<odabranaKategorija> popis_kategorija { get; set; }
        public List<String> mojiTermini { get; set; }
        public String MojeLokacijeJson { get; set; }

        public List<dogovor_term_osoba> dogovoreni_termini_kao_instruktor { get; set; }
        public List<dogovor_term_osoba> dogovoreni_termini_kao_klijent { get; set; }
        public List<OpisanPredmet> mojiPredmeti { get; set; }
        public List<Kategorija> sveKategorije { get; set; }
        public List<Ustanova> sveUstanove { get; set; }
        public List<Predmet> sviPredmeti { get; set; }
    }

    public class lokacijeJsonObject
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public String opis { get; set; }
        public int id { get; set; }
    }
    public class OpisanPredmet {
        public int IDpredmet { get; set; }
        public string nazivPredmet { get; set; }
        public string kraticaPredmet { get; set; }
        public decimal? cijenaPredmet { get; set; }
        public int IDkategorija { get; set; }
        public int IDustanova { get; set; }
        public virtual Kategorija kategorija { get; set; }
        public virtual Ustanova ustanova { get; set; }
    }
    public class odabranaKategorija
    {
        public string kategorija_ime { get; set; }
        public List<odabranPredmet> mojiPredmeti { get; set; }
    }

    public class odabranPredmet
    {
        public Predmet predmet { get; set;}
        public Boolean odabran { get; set; }
    }

    public class dogovor_term_osoba
    {
        public String ime { get; set; }
        public String prezime { get; set; }
        public dogovor_termin termin { get; set; }
        public Boolean odustani { get; set; }
        public Boolean seen { get; set; }
        public String predmet { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required(ErrorMessage = "Nedostaje stara lozinka")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nedostaje nova lozinka")]
        [StringLength(100, ErrorMessage = "{0} mora sadržavati barem {2} znakova", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Nedostaje ponovljena lozinka")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("NewPassword", ErrorMessage = "Nova lozinka se ne podudara sa potvrđenom lozinkom")]
        public string ConfirmPassword { get; set; }

    }

    public class RegisterBindingModel
    {
        [Required(ErrorMessage="Nedostaje e-mail adresa")]
        [EmailAddress(ErrorMessage="Neispravna Email adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nedostaje lozinka")]
        [StringLength(100, ErrorMessage = "{0} mora sadržavati barem {2} znakova", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nedostaje ponovljena lozinka")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Nova lozinka se ne podudara sa potvrđenom lozinkom")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Nedostaje ime")]
        public string ime { get; set; }

        [Required(ErrorMessage = "Nedostaje prezime")]
        public string prezime { get; set; }

        [Required(ErrorMessage = "Nedostaje korisničko ime")]
        public string korisnicko_ime { get; set; }

        [Required]
        public Boolean jeInstruktor { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
