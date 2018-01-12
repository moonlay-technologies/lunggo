using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        #region InsertRegularSession
        public static ApiResponseBase InsertRegularAvailableDates(ActivityAddSessionApiRequest apiRequest, ApplicationUserManager userManager)
        {
            /*var user = HttpContext.Current.User;
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
            }*/

            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var input = PreProcess(apiRequest);
            var output = ActivityService.GetInstance().ActivityAddSession(input);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        

        public static ActivityAddSessionInput PreProcess(ActivityAddSessionApiRequest apiRequest)
        {
            return new ActivityAddSessionInput
            {
                ActivityId = apiRequest.ActivityId,
                RegularAvailableDates = apiRequest.AvailableRegularDates
            };
        }


        public static ApiResponseBase AssembleApiResponse(ActivityAddSessionOutput serviceResponse)
        {
            if (serviceResponse.isSuccess == false)
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

        #endregion

        #region DeleteRegularSession


        public static ApiResponseBase DeleteRegularAvailableDates(ActivityDeleteSessionApiRequest apiRequest, ApplicationUserManager userManager)
        {
            /*var user = HttpContext.Current.User;
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
            }*/

            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var input = PreProcess(apiRequest);
            var output = ActivityService.GetInstance().ActivityDeleteSession(input);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        public static ActivityDeleteSessionInput PreProcess(ActivityDeleteSessionApiRequest apiRequest)
        {
            return new ActivityDeleteSessionInput
            {
                ActivityId = apiRequest.ActivityId,
                RegularAvailableDates = apiRequest.AvailableDates
            };
        }

        public static ApiResponseBase AssembleApiResponse(ActivityDeleteSessionOutput serviceResponse)
        {
            if (serviceResponse.isSuccess == false)
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

        #endregion



        #region CustomDate1

        public static ApiResponseBase SetOrUnsetCustomDate(SetOrUnsetCustomDateApiRequest apiRequest, ApplicationUserManager userManager)
        {
            /*var user = HttpContext.Current.User;
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
            }*/

            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var input = PreProcess(apiRequest);
            var output = ActivityService.GetInstance().ActivityCustomDateSetOrUnset(input);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        public static ActivityCustomDateInput PreProcess(SetOrUnsetCustomDateApiRequest apiRequest)
        {
            return new ActivityCustomDateInput
            {
                ActivityId = apiRequest.ActivityId,
                CustomDate = apiRequest.CustomDate,
                CustomHour = apiRequest.CustomHour
            };
        }

        public static ApiResponseBase AssembleApiResponse(ActivityCustomDateOutput serviceResponse)
        {
            if (serviceResponse.isSuccess == false)
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

        #endregion

        #region AddCustomDate
        public static ApiResponseBase AddCustomDate(CustomDateApiRequest apiRequest, ApplicationUserManager userManager)
        {
            /*var user = HttpContext.Current.User;
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
            }*/

            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var input = PreProcess(apiRequest);
            var output = ActivityService.GetInstance().AddCustomDate(input);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        public static CustomDateInput PreProcess(CustomDateApiRequest apiRequest)
        {
            return new CustomDateInput
            {
                ActivityId = apiRequest.ActivityId,
                CustomDate = apiRequest.CustomDate,
                CustomHour = apiRequest.CustomHour,
                DateStatus = apiRequest.DateStatus
            };
        }

        public static ApiResponseBase AssembleApiResponse(CustomDateOutput serviceResponse)
        {
            if (serviceResponse.isSuccess == false)
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
        #endregion


        public static ApiResponseBase DeleteCustomDate(CustomDateApiRequest apiRequest, ApplicationUserManager userManager)
        {
            /*var user = HttpContext.Current.User;
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
            }*/

            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var input = PreProcess(apiRequest);
            var output = ActivityService.GetInstance().DeleteCustomDate(input);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }
    }
}