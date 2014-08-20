using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Lunggo.CustomerWeb
{
    public class LunggoBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/pluginjs").Include(
"~/Statics/BootstrapPlugin/bootstrap-clockpicker/assets/js/html5shiv.js",
"~/Statics/BootstrapPlugin/bootstrap-clockpicker/dist/bootstrap-clockpicker.js",
"~/Statics/BootstrapPlugin/bootstrap-touchspin/bootstrap-touchspin/bootstrap.touchspin.js",
                //"~/Statics/JQuery/jquery-ui-1.10.4.custom/js/jquery-ui-1.10.4.custom.js"
"~/Statics/BootstrapPlugin/bootstrap-datepicker/js/bootstrap-datepicker.js",
"~/Statics/Stepper-master/src/jquery.fs.stepper.js",
                //"~/Statics/jQuery-Mask-Plugin-master/jquery.mask.min.js",
"~/Statics/Lunggo/Initializer.js",
"~/Statics/jQuery-linkify-master/dist/jquery.linkify.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/bootstrap/plugincss").Include(
"~/Statics/BootstrapPlugin/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
"~/Statics/BootstrapPlugin/bootstrap-clockpicker/dist/bootstrap-clockpicker.css",
                //"~/Statics/JQuery/jquery-ui-1.10.4.custom/css/ui-lightness/jquery-ui-1.10.4.custom.css"
"~/Statics/BootstrapPlugin/bootstrap-datepicker/css/bootstrap-datepicker.css",
"~/Statics/Stepper-master/src/jquery.fs.stepper.css"
                ));
        }
    }
}