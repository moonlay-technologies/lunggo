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
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.Framework.Mail;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Lunggo.Framework.TableStorage;
using Lunggo.ApCommon.Log;
using Lunggo.Framework.Environment;

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
            var TableLog = new GlobalLog();
            TableLog.PartitionKey = "ERROR LOG";
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
            var env = EnvVariables.Get("general", "environment");
            TableLog.Log = "```Error 500 API Log```"
                + "\n*Environment :* " + env.ToUpper()
                + "\n*Exception :* "
                + e.Message
                + "\n*Platform :* "
                + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
            log.Post(TableLog.Log
                ,
                env == "production" ? "#logging-prod" : "#logging-dev",
                new List<LogAttachment>
                {
                        new LogAttachment("STACK TRACE", e.StackTrace),
                        new LogAttachment("REQUEST", requestString)
                });
            TableLog.Log = TableLog.Log + "\n*STACK TRACE:*\n" + e.StackTrace
                                        + "\n*REQUEST:* " + requestString;
            TableLog.Logging();
            var inputException = new ExceptionTable();
            inputException.ErrorCode = "500";
            inputException.Exception = e.Message;
            inputException.Platform = Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId()).ToString();
            inputException.StackTrace = e.StackTrace;
            inputException.Request = requestString;
            inputException.PartitionKey = "ERROR LOG";
            inputException.RowKey = Guid.NewGuid().ToString();
            TableStorageService.GetInstance().InsertEntityToTableStorage(inputException, "LogExceptionError");
            return e.GetType() == typeof(JsonReaderException)
                ? ErrorInvalidJson(e.Message)
                : Error500();
        }

        public static ApiResponseBase Error(HttpStatusCode statusCode, string errorMessage)
        {
            return new ApiResponseBase
            {
                StatusCode = statusCode,
                ErrorCode = errorMessage
            };
        }

        public static ApiResponseBase Error500()
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorCode = "ERRGEN99"
            };
        }

        public static ApiResponseBase ErrorInvalidJson(string message)
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERRGEN98 : " + message
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