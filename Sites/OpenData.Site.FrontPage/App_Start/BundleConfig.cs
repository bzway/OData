using System.Web.Optimization;

namespace OpenData.Sites.FrontPage
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include(
                                          "~/Scripts/jquery.mobile-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryfileupload").Include(
                    "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                    "~/Scripts/jQuery.FileUpload/jquery.fileupload-*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/html5shiv.js",
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/respond.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                    "~/Scripts/admin/metisMenu.min.js",
                    "~/Scripts/admin/site.js"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                    "~/content/bootstrap.css",
                    "~/content/font-awesome.min.css",
                    "~/content/site.css"));


            bundles.Add(new StyleBundle("~/content/bootstrap").Include(
                    "~/content/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/content/admin").Include(
                    "~/content/admin/metisMenu.min.css",
                    "~/content/admin/timeline.css",
                    "~/content/font-awesome.min.css",
                    "~/content/admin/site.css"));

            bundles.Add(new StyleBundle("~/content/fileupload").Include(
                      "~/content/jQuery.FileUpload/css/*.css"));


            bundles.Add(new StyleBundle("~/content/css/ui").Include(
                      "~/content/themes/base/easyui.css",
                      "~/content/themes/base/all.css"));

            bundles.Add(new StyleBundle("~/content/css/mobile").Include(
                      "~/content/jquery.mobile-{version}.css"));


        }
    }
}
