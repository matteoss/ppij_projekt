using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Helpers;

namespace ppij_web_aplikacija.Controllers
{
    public class ProfilController : Controller
    {
        // GET: Profil
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.title = User.Identity.Name;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.ChangePasswordBindingModel model)
        {
            if (ModelState.IsValid) { 
            using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                Osoba osoba = data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault();
                if (Crypto.VerifyHashedPassword(osoba.lozinka, model.OldPassword + osoba.salt) == false)
                {
                    ModelState.AddModelError("error_old_password", "pogrešna lozinka");
                    return View();
                }
                string salt = Crypto.GenerateSalt(12);
                osoba.lozinka = Crypto.HashPassword(model.NewPassword + salt);
                osoba.salt = salt;
                data.SaveChanges();
                
                //String alert = model.NewPassword;
                //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=JavaScript>alert('" + alert + "')</SCRIPT>");
            }
            }
            return View();
        }
    }
}