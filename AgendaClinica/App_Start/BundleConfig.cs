﻿using System.Web;
using System.Web.Optimization;

namespace AgendaClinica.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://code.jquery.com/jquery-3.2.1.min.js").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css").Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Styles.css"));

            bundles.Add(new StyleBundle("~/Content/metisMenuCSS", "https://cdnjs.cloudflare.com/ajax/libs/metisMenu/2.7.0/metisMenu.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/metisMenuJS", "https://cdnjs.cloudflare.com/ajax/libs/metisMenu/2.7.0/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sb-admin").Include("~/Scripts/sb-admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryDatatables", "https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryDatatablesResponsive", "https://cdn.datatables.net/responsive/2.1.1/js/dataTables.responsive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datablesBootstrap", "https://cdn.datatables.net/1.10.15/js/dataTables.bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/responsiveBootstrap", "https://cdn.datatables.net/responsive/2.1.1/js/responsive.bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/datatableBootstrapCSS", "https://cdn.datatables.net/1.10.15/css/dataTables.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/datatableBootstrapResponsiveCSS", "https://cdn.datatables.net/responsive/2.1.1/css/responsive.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/Content/responsiveBootstrapCSS", "https://cdn.datatables.net/responsive/2.1.1/css/responsive.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryUiCSS", "https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUi", "https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/Content/JqueryUiDatetimePicker", "https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.4/build/jquery.datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUiDateTime", "https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.4/build/jquery.datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryMask", "https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.11/jquery.mask.min.js"));

            bundles.Add(new StyleBundle("~/Content/toastrCSS", "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastrJS", "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DatatablesPT", "https://cdn.datatables.net/plug-ins/1.10.16/i18n/Portuguese-Brasil.json"));

            bundles.Add(new ScriptBundle("~/bundles/DatatablesPT", "https://cdn.datatables.net/plug-ins/1.10.16/i18n/Portuguese-Brasil.json"));

            bundles.Add(new ScriptBundle("~/bundles/JqueryValidate", "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/JqueryValidateUnob", "https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.6/jquery.validate.unobtrusive.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/globalize", "https://cdnjs.cloudflare.com/ajax/libs/globalize/1.3.0/globalize.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/JqueryValidate", "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js"));

            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;
        }
    }
}