using System.Web;
using System.Web.Optimization;

namespace AgendaUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.Unobtrusive.Ajax", "https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css")
                .Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/Style.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome", "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css")
                .Include("~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryCascade")
                .Include("~/Scripts/jquery.cascade.0.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate", "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include(
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/jqueryUiCSS", "https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUi", "https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryMask", "https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.11/jquery.mask.min.js"));


            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;
        }

    }
}