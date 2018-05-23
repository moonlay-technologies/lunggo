using System.Web.Optimization;
using System.Web.Optimization.React;

namespace Lunggo.CustomerWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new BabelBundle("~/bundles/payment").Include(
                "~/Assets/js/jquery-{version}.js",
                "~/Assets/js/PaymentPageStateContainer.js",
                "~/Assets/js/PaymentController.js",
                "~/Assets/js/PaymentDataForm.jsx",
                "~/Assets/js/PaymentDataStateContainer.js",
                "~/Assets/js/PaymentInstruction.jsx",
                "~/Assets/js/PaymentModalLayout.jsx",
                "~/Assets/js/PaymentModalStateContainer.js",
                "~/Assets/js/PaymentPageLayout.jsx"));
        }
    }
}
