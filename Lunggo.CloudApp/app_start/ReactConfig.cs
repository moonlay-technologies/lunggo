using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Lunggo.CustomerWeb.ReactConfig), "Configure")]

namespace Lunggo.CustomerWeb
{
    public static class ReactConfig
    {
        public static void Configure()
        {
            // If you want to use server-side rendering of React components, 
            // add all the necessary JavaScript files here. This includes 
            // your components as well as all of their dependencies.
            // See http://reactjs.net/ for more information. Example:
            
            //.AddScript("~/Areas/Payment_v2/Views/Payment/PaymentPageStateContainer.js");
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentController.js")
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentDataForm.jsx")
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentDataStateContainer.js")
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentInstruction.jsx")
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentModalLayout.jsx")
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentModalStateContainer.js")
            //    .AddScript("~/Areas/Payment_v2/Views/Payment/PaymentPageLayout.jsx");
            //	.AddScript("~/Scripts/Second.jsx");
            // If you use an external build too (for example, Babel, Webpack,
            // Browserify or Gulp), you can improve performance by disabling 
            // ReactJS.NET's version of Babel and loading the pre-transpiled 
            // scripts. Example:
            //ReactSiteConfiguration.Configuration
            //	.SetLoadBabel(false)
            //	.AddScriptWithoutTransform("~/Scripts/bundle.server.js")
        }
    }
}