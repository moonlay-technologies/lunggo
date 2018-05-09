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

namespace Lunggo.WebAPI.ApiSrc.Common.Model
{
    [KnownType("GetDerivedTypes")]
    public class ApiRequestBase
    {
        private static readonly IEnumerable<Type> DerivedTypes = PopulateDerivedTypes();

        public static T DeserializeRequest<T>()
        {
            using (var sr = new StreamReader(HttpContext.Current.Request.GetBufferedInputStream()))
            {
                var requestString = sr.ReadToEnd();
                HttpContext.Current.Request.RequestContext.RouteData.Values["body"] = requestString;
                return requestString.Deserialize<T>();
            }

        }

        public static string GetHeaderValue(string key)
        {
            var sr = HttpContext.Current.Request.Headers.GetValues(key);
            if (sr != null)
            {
                return sr[0];
            }
            return "";
        }

        private static IEnumerable<Type> PopulateDerivedTypes()
        {
            return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where typeof(ApiRequestBase).IsAssignableFrom(assemblyType)
                    select assemblyType);
        }

        private static IEnumerable<Type> GetDerivedTypes()
        {
            return DerivedTypes;
        }
    }
}