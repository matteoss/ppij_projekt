using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ppij_web_aplikacija
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sneder, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (ppij_databaseEntities data = new ppij_databaseEntities())
                        {
                            Osoba os = (Osoba) data.Osoba.SingleOrDefault(o => o.korisnicko_ime_osoba == username);
                            roles = os.razina_prava.ToString();
                        }

                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                            new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch(HttpException he)
                    {
                        
                    }
                }
            }
        }
    }
}
