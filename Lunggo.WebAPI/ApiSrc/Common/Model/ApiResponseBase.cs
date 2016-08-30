using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
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

        public static ApiResponseBase ExceptionHandling(Exception e)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";
            MailService.GetInstance().SendPlainEmail(new MailModel
            {
                FromMail = "log@travorama.com",
                FromName = "Travorama Log",
                RecipientList = new[] { "developer@travelmadezy.com" },
                Subject = envPrefix + "Error 500 API Log",
            },
                "<html><body>Exception : "
                + e.Message
                + "<br/><br/>Stack Trace : <br/><br/>"
                + e.StackTrace
                + "<br/><br/>Platform : "
                + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId())
                + "<br/><br/></body></html>");
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