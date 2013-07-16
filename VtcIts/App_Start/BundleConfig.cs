using System.Web;
using System.Web.Optimization;

namespace VtcIts
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        //"~/Scripts/jquery.unobtrusive-ajax.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/globalize.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/mousewheel").Include(
                        "~/Scripts/jquery.mousewheel.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                        "~/Scripts/fullcalendar.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        //"~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/timespinner").Include(
                        "~/Scripts/ui.timespinner.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/rrule").Include(
                //"~/Scripts/rrule/underscore-min.js",
                        "~/Scripts/rrule/underscore.js",
                        "~/Scripts/rrule/rrule.js",
                        "~/Scripts/rrule/nlp.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                "~/Scripts/i18n/grid.locale-en.js",
                "~/Scripts/jquery.jqGrid.*"));


            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                    "~/Content/themes/base/jquery-ui.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.all.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.base.css",
                    "~/Content/themes/base/jquery.ui.button.css",
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/Content/themes/base/jquery.ui.dialog.css",
                    "~/Content/themes/base/jquery.ui.menu.css",
                    "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.slider.css",
                    "~/Content/themes/base/jquery.ui.spinner.css",
                    "~/Content/themes/base/jquery.ui.tabs.css",
                    "~/Content/themes/base/jquery.ui.theme.css",
                    "~/Content/themes/base/jquery.ui.tooltip.css"
            ));

            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include(
                   "~/Content/fullcalendar.css",
                   "~/Content/fullcalendar.print.css"
            ));


            bundles.Add(new StyleBundle("~/Content/jqgrid/css").Include(
                "~/Content/jquery.jqGrid/ui.jqgrid.css",
                "~/Content/jqGrid.Custom.css"
            ));
        }
    }
}