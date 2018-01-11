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
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase InsertContactLandingPage(LandingPageApiRequest request)
        {
            LandingPageInput input;
            if (request == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var succeed = TryPreprocess(request, out input);
            var serviceResponse = ActivityService.GetInstance().InsertContactLandingPageToBlob(input);
            var apiResponse = AssembleApiResponse(serviceResponse);
            return apiResponse;
        }

        public static bool TryPreprocess(LandingPageApiRequest request, out LandingPageInput serviceRequest)
        {
            serviceRequest = new LandingPageInput();
            if (request == null)
                { return false; }
            serviceRequest.Contact = request.Contact;
            return true;            
        }

        public static ApiResponseBase AssembleApiResponse(LandingPageOutput serviceResponse)
        {
            if(serviceResponse.isSuccess == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}