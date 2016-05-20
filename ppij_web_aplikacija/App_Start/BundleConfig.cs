using System.Web;
using System.Web.Optimization;

namespace ppij_web_aplikacija
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
					  "~/Scripts/jquery-ui-{version}.js"));
 
			bundles.Add(new StyleBundle("~/Content/cssjqryUi").Include(
					  "~/Content/themes/base/all.css"));

			bundles.Add(new ScriptBundle("~/bundles/jquery_tablesorter").Include(
					  "~/Scripts/jquery.tablesorter.js"));

			bundles.Add(new StyleBundle("~/Content/css_jquery_tablesorter").Include(
					  "~/Content/tablesorter/style.css"));
		}
    }
}
