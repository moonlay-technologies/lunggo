using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Web.Http;
using Lunggo.ApCommon.Activity.Model;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace Lunggo.WebAPI.ApiSrc.Activity
{
    public class ActivityController : ApiController
    {
        #region Managers
        public ActivityController()
        {
        }

        public ActivityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }
        #endregion

        #region Customer
        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities")]
        public ApiResponseBase SearchActivity(string name="", string startDate = "", string endDate = "", string page="1", string perPage="10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            
            try
            {
                var request = new ActivitySearchApiRequest
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Page = page,
                    PerPage = perPage
                };

                var apiResponse = ActivityLogic.Search(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities/{id}")]
        public ApiResponseBase GetActivityDetail(string id = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetDetailActivityApiRequest { ActivityId = id};

                var apiResponse = ActivityLogic.GetDetail(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities/{id}/availabledates")]
        public ApiResponseBase GetAvailableDate(string id = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetAvailableDatesApiRequest { ActivityId = id };

                var apiResponse = ActivityLogic.GetAvailable(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/book")]
        public ApiResponseBase BookActivity()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            ActivityBookApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ActivityBookApiRequest>();
                var apiResponse = ActivityLogic.BookActivity(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/mybooking")]
        public ApiResponseBase MyBooking(string page = "1", string perPage = "10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetMyBookingsApiRequest()
                {
                    Page = page,
                    PerPage = perPage
                };
                var apiResponse = ActivityLogic.GetMyBookings(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/mybooking/{rsvNo}")]
        
        public ApiResponseBase MyBookingDetail(string rsvNo = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetMyBookingDetailApiRequest()
                {
                    RsvNo = rsvNo
                };
                var apiResponse = ActivityLogic.GetMyBookingDetail(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
        #endregion

        #region Operator
        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/request")]

        public ApiResponseBase AppointmentRequest(string page = "1", string perPage = "10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetAppointmentRequestApiRequest()
                {
                    Page = page,
                    PerPage = perPage
                };
                var apiResponse = ActivityLogic.GetAppointmentRequest(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/request/{rsvNo}")]

        public ApiResponseBase AppointmentConfirmation(string rsvNo = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            //ConfirmationStatusApiRequest request = null;
            try
            {
                //request = ApiRequestBase.DeserializeRequest<ConfirmationStatusApiRequest>();
                //var a = new ConfirmationStatusApiRequest()
                //{
                //    RsvNo = rsvNo,
                //    Status = request.Status
                //};
                var apiResponse = ActivityLogic.ConfirmAppointment(rsvNo, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/appointments")]

        public ApiResponseBase AppointmentList(string page = "1", string perPage = "10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetAppointmentListApiRequest()
                {
                    Page = page,
                    PerPage = perPage
                };
                var apiResponse = ActivityLogic.GetAppointmentList(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/appointments/{apppointmentId}")]

        public ApiResponseBase AppointmentDetail(string apppointmentId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            GetAppointmentDetailApiRequest request = null;
            try
            {
                request = new GetAppointmentDetailApiRequest()
                {
                    AppointmentId = apppointmentId
                };
                var apiResponse = ActivityLogic.GetAppointmentDetail(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/myactivity")]

        public ApiResponseBase ListActivities(string page = "1", string perPage = "10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetListActivityApiRequest()
                {
                    Page = page,
                    PerPage = perPage
                };
                var apiResponse = ActivityLogic.GetListActivity(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
        #endregion

    }
}