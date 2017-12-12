using System;
using System.Collections.Generic;
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
        public static ApiResponseBase GetBookedActivities()
        {
            var serviceResponse = ActivityService.GetInstance().GetBookedActivities();
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }
        
        public static ApiResponseBase AssembleApiResponse(List<ActivityReservation> serviceResponse)
        {
            var apiResponse = new ApiResponseBase
            {
                
            };

            return apiResponse;

        }
    }
}