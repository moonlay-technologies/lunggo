using Lunggo.WebAPI.ApiSrc.Auxiliary.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Logic
{
    public static partial class AuxiliaryLogic
    {
        public static ApiResponseBase CheckVersion(CheckVersionApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var androidLatestVersion = "1.0.1";
            var iosLatestVersion = "1.0.0";
            if (apiRequest.Platform == "android")
            {
                if (apiRequest.CurrentVersion != androidLatestVersion)
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = true,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            else if(apiRequest.Platform == "ios")
            {
                if (apiRequest.CurrentVersion != iosLatestVersion)
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        MustUpdate = true,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        MustUpdate = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_PLATFORM"
                };
            }
        }
    }
}