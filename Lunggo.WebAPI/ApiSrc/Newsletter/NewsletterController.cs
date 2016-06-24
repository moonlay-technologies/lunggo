using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Config;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Newsletter.Logic;
using Lunggo.WebAPI.ApiSrc.Newsletter.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Lunggo.WebAPI.ApiSrc.Newsletter
{
    public class NewsletterController : ApiController
    {
        [LunggoCorsPolicy]
        [Route("v1/newsletter/subscribe")]
        [HttpPost]
        public ApiResponseBase NewsletterSubscribe()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<NewsletterSubscribeApiRequest>();
                var apiResponse = NewsletterLogic.NewsletterSubscribe(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
