using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
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
        [JsonProperty("ver")]
        public string Version { get; set; }

        public ApiResponseBase()
        {
            Version = "1.0";
        }

        public static ApiResponseBase Return500()
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorCode = "ERRGEN99"
            };
        }

        public static ApiResponseBase ReturnInvalidJson()
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