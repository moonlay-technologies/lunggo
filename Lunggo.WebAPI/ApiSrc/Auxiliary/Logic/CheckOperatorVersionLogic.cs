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
        public static ApiResponseBase CheckOperatorVersion(CheckVersionApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var androidLatestVersion = "0.1.2";
            var iosLatestVersion = "0.1.2";

            var forceToUpdate = true;
            if (apiRequest.Platform == "android")
            {
                if (apiRequest.CurrentVersion != androidLatestVersion && apiRequest.CurrentVersion == "0.1.0")
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = forceToUpdate,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else if(apiRequest.CurrentVersion != androidLatestVersion && apiRequest.CurrentVersion == "0.1.1")
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = false,
                        ForceToUpdate = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            else if(apiRequest.Platform == "ios")
            {
                if (apiRequest.CurrentVersion != iosLatestVersion && apiRequest.CurrentVersion == "0.1.0")
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = forceToUpdate,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else if (apiRequest.CurrentVersion != iosLatestVersion && apiRequest.CurrentVersion == "0.1.1")
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = false,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        ForceToUpdate = false,
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