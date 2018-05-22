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
using static Lunggo.WebAPI.App_Start.FilterConfig;

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
        [Route("v1/activities/mybooking/cart/active")]
        public ApiResponseBase MyBookingCartActive(string lastUpdate = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetMyBookingsCartActiveApiRequest()
                {
                    LastUpdate = lastUpdate
                };
                var apiResponse = ActivityLogic.GetMyBookingsCartActive(request);
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

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/wishlist")]
        public ApiResponseBase GetWishlist()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var apiResponse = ActivityLogic.GetWishlist();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPut]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/wishlist/{activityId}")]
        public ApiResponseBase AddToWishlist(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new AddToWishlistApiRequest { ActivityId = activityId };
                var apiResponse = ActivityLogic.AddToWishlist(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpDelete]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/wishlist/{activityId}")]
        public ApiResponseBase DeleteFromWishlist(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new DeleteFromWishlistApiRequest { ActivityId = activityId };
                var apiResponse = ActivityLogic.DeleteFromWishlist(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPut]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/landingpage/contact")]
        public ApiResponseBase InsertContactLandingPage()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            LandingPageApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<LandingPageApiRequest>();
                var apiResponse = ActivityLogic.InsertContactLandingPage(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities/{activityId}/ticket")]
        public ApiResponseBase GetActivityTicketDetail(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetActivityTicketDetailApiRequest { ActivityId = activityId };

                var apiResponse = ActivityLogic.GetActivityTicketDetail(request);
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
        [Route("v1/activities/{activityId}/reviews")]
        public ApiResponseBase GetActivityReviews(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetActivityReviewsApiRequest { ActivityId = Int64.Parse(activityId) };

                var apiResponse = ActivityLogic.GetActivityReviews(request);
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
        [Route("v1/activities/mybooking/{rsvNo}/ratingquestion")]

        public ApiResponseBase GenerateQuestion(string rsvNo = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GenerateQuestionApiRequest()
                {
                    RsvNo = rsvNo
                };
                var apiResponse = ActivityLogic.GenerateQuestion(request);
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
        [Route("v1/activities/mybooking/{rsvNo}/ratinganswer")]

        public ApiResponseBase InsertActivityRating(string rsvNo = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            InsertActivityRatingApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<InsertActivityRatingApiRequest>();
                request.RsvNo = rsvNo;
                var apiResponse = ActivityLogic.InsertActivityRating(request);
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
        [Route("v1/activities/mybooking/{rsvNo}/review")]

        public ApiResponseBase InsertActivityReview(string rsvNo = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            InsertActivityReviewApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<InsertActivityReviewApiRequest>();
                request.RsvNo = rsvNo;
                var apiResponse = ActivityLogic.InsertActivityReview(request);
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
        [Route("v1/operator/appointments/request")]

        public ApiResponseBase AppointmentRequest(string lastUpdate = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetAppointmentRequestApiRequest()
                {
                    LastUpdate = lastUpdate
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
        [Route("v1/operator/appointments/confirm/{rsvNo}")]

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

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/appointments/decline/{rsvNo}")]

        public ApiResponseBase AppointmentDeclination(string rsvNo = "")
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
                var apiResponse = ActivityLogic.DeclineAppointment(rsvNo, UserManager);
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
        [Route("v1/operator/appointments/cancel/{rsvNo}")]

        public ApiResponseBase AppointmentCancellation(string rsvNo = "")
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
                var apiResponse = ActivityLogic.CancelAppointment(rsvNo, UserManager);
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

        public ApiResponseBase AppointmentList(string page = "1", string perPage = "10", string type = null, string startDate = "", string endDate = "", string bookingStatusCd = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetAppointmentListApiRequest()
                {
                    Type = type,
                    Page = page,
                    StartDate = startDate,
                    EndDate = endDate,
                    PerPage = perPage,
                    BookingStatusCd = bookingStatusCd
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
        [Route("v1/operator/appointments/{activityId}/{date}")]

        public ApiResponseBase AppointmentDetail(string activityId = "", string date = "", string session = "")
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
                    ActivityId = activityId,
                    Date = date,
                    Session = session
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

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/myactivity/{id}")]
        public ApiResponseBase GetActivityDetailOperator(string id = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetDetailActivityApiRequest { ActivityId = id };

                var apiResponse = ActivityLogic.GetDetail(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPut]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/activity/{activityId}/session")]
        public ApiResponseBase InsertRegularAvailableDates(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {       
                var request = ApiRequestBase.DeserializeRequest<ActivityAddSessionApiRequest>();
                request.ActivityId = Int64.Parse(activityId);
                var apiResponse = ActivityLogic.InsertRegularAvailableDates(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpDelete]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/activity/{activityId}/session")]
        public ApiResponseBase DeleteRegularAvailableDates(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = ApiRequestBase.DeserializeRequest<ActivityDeleteSessionApiRequest>();
                request.ActivityId = Int64.Parse(activityId);
                var apiResponse = ActivityLogic.DeleteRegularAvailableDates(request, UserManager);
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
        [Route("v1/operator/activity/{activityId}/customdate")]
        public ApiResponseBase SetOrUnsetCustomDate(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = ApiRequestBase.DeserializeRequest<SetOrUnsetCustomDateApiRequest>();
                request.ActivityId = Int64.Parse(activityId);
                var apiResponse = ActivityLogic.SetOrUnsetCustomDate(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPut]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/activity/{activityId}/customdate")]
        public ApiResponseBase AddCustomDate(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = ApiRequestBase.DeserializeRequest<CustomDateApiRequest>();
                request.ActivityId = Int64.Parse(activityId);
                var apiResponse = ActivityLogic.AddCustomDate(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpDelete]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/operator/activity/{activityId}/customdate")]
        public ApiResponseBase DeleteCustomDate(string activityId = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = ApiRequestBase.DeserializeRequest<CustomDateApiRequest>();
                request.ActivityId = Int64.Parse(activityId);
                var apiResponse = ActivityLogic.DeleteCustomDate(request, UserManager);
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
        [Route("v1/operator/transactionstatement")]
        public ApiResponseBase GetTransactionStatement(string startDate = "", string endDate = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = new GetTransactionStatementApiRequest
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
                var apiResponse = ActivityLogic.GetTransactionStatement(request, UserManager);
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
        [Route("v1/operator/verifyticket")]
        public ApiResponseBase VerifyTicket()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            VerifyTicketApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<VerifyTicketApiRequest>();
                var apiResponse = ActivityLogic.VerifyTicket(request,UserManager);
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
        [Route("v1/operator/pendingrefunds")]
        public ApiResponseBase GetPendingRefunds(string page = "1", string perPage = "10", string type = null, string startDate = "", string endDate = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetPendingRefundApiRequest()
                {
                    Page = page,
                    StartDate = startDate,
                    EndDate = endDate,
                    PerPage = perPage
                };
                var apiResponse = ActivityLogic.GetPendingRefunds(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        //[HttpGet]
        //[LunggoCorsPolicy]
        //[Level2Authorize]
        //[Route("v1/operator/reservations")]
        //
        //public ApiResponseBase GetReservationList(string page = "1", string perPage = "10")
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //
        //    try
        //    {
        //        var request = new GetAppointmentListApiRequest()
        //        {
        //            Page = page,
        //            PerPage = perPage
        //        };
        //        var apiResponse = ActivityLogic.GetReservationList(request, UserManager);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e);
        //    }
        //}



        #endregion

        #region Admin
        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/operator/transactionstatement")]
        public ApiResponseBase InsertTransactionStatement()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            InsertTransactionStatementApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<InsertTransactionStatementApiRequest>();
                var apiResponse = ActivityLogic.InsertTransactionStatement(request,UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }


        #endregion

        #region Update
        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/update")]
        public ApiResponseBase UpdateActivity()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            ActivityUpdateApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ActivityUpdateApiRequest>();
                var apiResponse = ActivityLogic.UpdateActivity(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
        #endregion

    }
}