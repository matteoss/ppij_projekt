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
        public MojeInstrukcije mojeInstrukcije { get; set; }
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
        public List<Kategorija> mojeKategorije { get; set; }
        public List<String> mojiTermini { get; set; }
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
