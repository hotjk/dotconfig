using System.Web;
using System.Web.Optimization;

namespace Settings.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/requirejs").Include(
                      "~/Scripts/require.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
                      "~/Scripts/jstree.js"));

            Grit.Utility.Web.JS.AppScriptsBandles.AddFolder("~/Scripts/app/", bundles);

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/toastr").Include(
                      "~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Content/jstree/themes/proton/style").Include(
                      "~/Content/jstree/themes/proton/style.css"));

            bundles.Add(new StyleBundle("~/Content/jstree/themes/default/style").Include(
                      "~/Content/jstree/themes/default/style.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include(
                      "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = !HttpContext.Current.IsDebuggingEnabled;

            if (!BundleTable.EnableOptimizations)
            {
                foreach (var b in bundles)
                {
                    // And strip out any transformations (minify)
                    b.Transforms.Clear();
                }
            }
        }
    }
}
