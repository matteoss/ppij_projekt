using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ppij_web_aplikacija
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Kategorije",
				url: "instrukcije",
				defaults: new { controller = "Instrukcije", action = "Kategorija" }
				);

			routes.MapRoute(
				name: "Ustanove",
				url: "instrukcije/{kategorija_id}",
				defaults: new { controller = "Instrukcije", action = "Ustanova" }
				);

			routes.MapRoute(
				name: "Predmeti",
				url: "instrukcije/{kategorija_id}/{ustanova_id}",
				defaults: new { controller = "Instrukcije", action = "Predmet" }
				);

			routes.MapRoute(
				name: "Instrukcije",
				url: "instrukcije/{kategorija_id}/{ustanova_id}/{predmet_id}",
				defaults: new { controller = "Instrukcije", action = "Instrukcija" }
				);

			routes.MapRoute(
				name: "PosaljiZahtjev",
				url: "instrukcije/{kategorija_id}/{ustanova_id}/{predmet_id}/posalji_zahtjev",
				defaults: new { controller = "Instrukcije", action = "PosaljiZahtjev" }
				);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
