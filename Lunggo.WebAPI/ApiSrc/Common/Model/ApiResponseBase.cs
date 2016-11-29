using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.Framework.Mail;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Common.Model
{
    [KnownType("GetDerivedTypes")]
    public class ApiResponseBase
    {
        private static readonly IEnumerable<Type> DerivedTypes = PopulateDerivedTypes();

        [JsonProperty("status")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        public static ApiResponseBase ExceptionHandling(Exception e, object request = null)
        {
            while (e.InnerException != null)
                e = e.InnerException;

            string requestString;
            try
            {
                requestString = request.Serialize();
            }
            catch
            {
                requestString = "Invalid JSON";
            }
            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env == "production")
                log.Post(
                    "```Error 500 API Log```"
                    + "\n*Environment :* " + env.ToUpper()
                    + "\n*Exception :* "
                    + e.Message
                    + "\n*Stack Trace :* \n"
                    + e.StackTrace
                    + "\n*Request :* \n"
                    + requestString
                    + "\n*Platform :* "
                    + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId()));
            return e.GetType() == typeof(JsonReaderException)
                ? ErrorInvalidJson()
                : Error500();
        }

        public static ApiResponseBase Error500()
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorCode = "ERRGEN99"
            };
        }

        public static ApiResponseBase ErrorInvalidJson()
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERRGEN98"
            };
        }

        private static IEnumerable<Type> PopulateDerivedTypes()
        {
            return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where typeof(ApiResponseBase).IsAssignableFrom(assemblyType)
                    select assemblyType);
        }

        private static IEnumerable<Type> GetDerivedTypes()
        {
            return DerivedTypes;
        }
    }
}