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
using Microsoft.AspNet.Identity;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase UpdateActivity(ActivityUpdateApiRequest request, ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER" //ERAGPR01
                };
            }
            var role = userManager.GetRoles(user.Identity.GetUser().Id).FirstOrDefault();
            if (role != "Operator")
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_NOT_OPERATOR"
                };
            }
            ActivityUpdateInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            }
            var serviceResponse = ActivityService.GetInstance().UpdateActivity(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);
            return apiResponse;
        }

        public static bool TryPreprocess(ActivityUpdateApiRequest request, out ActivityUpdateInput serviceRequest)
        {
            serviceRequest = new ActivityUpdateInput();
            if (request == null)
            {
                return false;
            }
            
            if (request.ActivityId <= 0) { return false; }

            if (string.IsNullOrEmpty(request.Name)) { return false; }
            
            if(request.Price < 0) { return false; }

            int amountDuration;
            var isNumeric = int.TryParse(request.Duration.Amount, out amountDuration);
            if(!isNumeric) { return false; }

            serviceRequest.ActivityId = request.ActivityId;
            serviceRequest.Name = request.Name;
            serviceRequest.Category = request.Category;
            serviceRequest.ShortDesc = request.ShortDesc;
            serviceRequest.City = request.City;
            serviceRequest.Country = request.Country;
            serviceRequest.Address = request.Address;
            serviceRequest.Latitude = request.Latitude;
            serviceRequest.Longitude = request.Longitude;
            serviceRequest.Price = request.Price;
            serviceRequest.PriceDetail = request.PriceDetail;
            serviceRequest.Duration = request.Duration;
            serviceRequest.OperationTime = request.OperationTime;
            serviceRequest.Cancellation = request.Cancellation;
            serviceRequest.Contents = request.Contents;
            serviceRequest.IsPassportNumberNeeded = string.IsNullOrEmpty(request.RequiredPaxData[0]) ? false : true;
            serviceRequest.IsPassportIssuedDateNeeded = string.IsNullOrEmpty(request.RequiredPaxData[1]) ? false : true;
            serviceRequest.IsPaxDoBNeeded = string.IsNullOrEmpty(request.RequiredPaxData[2]) ? false : true;
            return true;
        }
        
        public static ApiResponseBase AssembleApiResponse(ActivityUpdateOutput serviceResponse)
        {
            if (serviceResponse.IsSuccess)
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.OK
                };
            else
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERR_UPDATE_FAILED"
                };
            }
        }
    }
}