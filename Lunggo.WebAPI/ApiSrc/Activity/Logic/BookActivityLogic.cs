using System;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Product.Constant;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase BookActivity(ActivityBookApiRequest request)
        {
            if (!IsValid(request))
                return new ActivityBookApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            var bookServiceRequest = PreprocessServiceRequest(request);
            var searchServiceResponse = ActivityService.GetInstance().BookActivity(bookServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);

            return apiResponse;
        }

        public static bool IsValid(ActivityBookApiRequest request)
        {
            try
            {
                return
                    request != null && request.Contact != null &&
                    !string.IsNullOrEmpty(request.ActivityId) &&
                    !string.IsNullOrEmpty(request.Date) &&
                    request.Contact.Title != Title.Undefined &&
                    !string.IsNullOrEmpty(request.Contact.Name) &&
                    !string.IsNullOrEmpty(request.Contact.Phone) &&
                    !string.IsNullOrEmpty(request.Contact.Email) &&
                    new MailAddress(request.Contact.Email) != null &&
                    !string.IsNullOrEmpty(request.Contact.CountryCallingCode);
            }
            catch
            {
                return false;
            }
        }

        private static BookActivityInput PreprocessServiceRequest(ActivityBookApiRequest request)
        {
            var pax = ActivityService.GetInstance().ConvertToPax(request.Passengers);
            var selectServiceRequest = new BookActivityInput
            {
                Passengers = pax,
                Contact = request.Contact,
                ActivityId = request.ActivityId,
                Date = DateTime.Parse(request.Date),
                TicketCount = request.TicketCount
            };
            return selectServiceRequest;
        }


        public static ActivityBookApiResponse AssembleApiResponse(BookActivityOutput bookActivityServiceResponse)
        {
            if (bookActivityServiceResponse == null)
            {
                return new ActivityBookApiResponse();
            }
            if (!bookActivityServiceResponse.IsValid)
                return new ActivityBookApiResponse
                {
                    IsValid = bookActivityServiceResponse.IsValid,
                    StatusCode = HttpStatusCode.OK,
                };
            //if (bookActivityServiceResponse.IsPriceChanged)
            //{
            //    return new ActivityBookApiResponse
            //    {
            //        IsPriceChanged = bookActivityServiceResponse.IsPriceChanged,
            //        NewPrice = bookActivityServiceResponse.NewPrice,
            //        IsValid = bookActivityServiceResponse.IsValid,
            //        StatusCode = HttpStatusCode.OK,
            //    };
            //}
            return new ActivityBookApiResponse
            {
                IsValid = bookActivityServiceResponse.IsValid,
                RsvNo = bookActivityServiceResponse.RsvNo,
                TimeLimit = bookActivityServiceResponse.TimeLimit,
                StatusCode = HttpStatusCode.OK,
            };

        }
    }
}