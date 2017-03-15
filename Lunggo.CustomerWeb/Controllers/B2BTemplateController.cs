using System.Web.Mvc;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Config;
using RestSharp;

namespace Lunggo.CustomerWeb.Controllers
{
    public class B2BTemplateController : Controller
    {
        // GET: B2BTemplate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        public ActionResult ConfirmPassword()
        {
            return View();
        }
        public ActionResult SearchFlight()
        {
            var result = GetBookingDisabilityStatus();
            if (result.IsPaymentDisabled == null)
            {
                result.IsPaymentDisabled = true;
            }
            return View(result);
        }
        public ActionResult SearchHotel()
        {
            var result = GetBookingDisabilityStatus();
            if (result.IsPaymentDisabled == null)
            {
                result.IsPaymentDisabled = true;
            }
            return View(result);
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult OrderListFlight()
        {
            return View();
        }
        public ActionResult OrderListHotel()
        {
            return View();
        }
        public ActionResult OrderDetailFlight()
        {
            return View();
        }
        public ActionResult OrderDetailHotel()
        {
            return View();
        }
        public ActionResult UserManagement()
        {
            return View();
        }

        public GetBookingDisabilityStatusResponse GetBookingDisabilityStatus()
        {
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var client = new RestClient(baseUrl);
            
            try
            {
                var request = new RestRequest("/v1/payment/getbookingdisabilitystatus", Method.GET);
                var key = Request.Cookies["authkey"];
                if (key == null)
                    return null;

                // execute the request
                request.AddHeader("Authorization", "Bearer " + key.Value);
                IRestResponse<GetBookingDisabilityStatusResponse> response =
                    client.Execute<GetBookingDisabilityStatusResponse>(request);
                return response.Data;
            }
            catch
            {
                return new GetBookingDisabilityStatusResponse
                {
                    IsPaymentDisabled = true
                };
            }
            
        }

        
    }
}