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

            var androidLatestVersion = "0.1.2";
            var iosLatestVersion = "0.1.2";
            var androidUpdateUrl = "https://play.google.com/store/apps/details?id=com.travorama&hl=in";
            var iosUpdateUrl = "https://itunes.apple.com/id/app/travorama/id1142546215?mt=8";
            var forceToUpdate = true;
            if (apiRequest.Platform == "android")
            {
                if (apiRequest.CurrentVersion != androidLatestVersion && apiRequest.CurrentVersion == "0.1.1")
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = false,
                        UpdateUrl = androidUpdateUrl,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else if(apiRequest.CurrentVersion != androidLatestVersion)
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = forceToUpdate,
                        UpdateUrl = androidUpdateUrl,
                        StatusCode = HttpStatusCode.OK,
                    };
                }
                else
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = androidLatestVersion,
                        MustUpdate = false,
                        ForceToUpdate = false,
                        UpdateUrl = androidUpdateUrl,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            else if(apiRequest.Platform == "ios")
            {
                if (apiRequest.CurrentVersion != iosLatestVersion && apiRequest.CurrentVersion == "0.1.1")
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = false,
                        UpdateUrl  = iosUpdateUrl,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else if (apiRequest.CurrentVersion != iosLatestVersion)
                {
                    return new CheckVersionApiResponse
                    {
                        LatestVersion = iosLatestVersion,
                        MustUpdate = true,
                        ForceToUpdate = forceToUpdate,
                        UpdateUrl = iosUpdateUrl,
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
                        UpdateUrl = iosUpdateUrl,
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