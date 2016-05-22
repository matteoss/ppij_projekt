using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ppij_web_aplikacija.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page " + User.Identity.Name;

            return View();
        }
        public string testni()
        {
            return "pozdrav!";
        }
    }
}
