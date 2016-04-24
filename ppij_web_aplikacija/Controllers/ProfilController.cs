using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            /*using (ppij_databaseEntities data = new ppij_databaseEntities()) {
                if (data.Osoba.Where(o => o.korisnicko_ime_osoba == User.Identity.Name).FirstOrDefault().lozinka != model.OldPassword)
                {
                    ModelState.AddModelError("error_old_password", "pogrešna lozinka");
                    return View();
                }
                String alert = model.NewPassword;
                System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=JavaScript>alert('" + alert + "')</SCRIPT>");
                
            }*/
            AccountController a = new AccountController();
            a.ChangePassword(model).RunSynchronously();
            return View();
        }
    }
}