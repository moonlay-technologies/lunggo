using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Auxiliary.Logic;
using Lunggo.WebAPI.ApiSrc.Auxiliary.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary
{
    public class AuxiliaryController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/calendar")]
        [Route("v1/calendar/{lang}")]
        public ApiResponseBase GetCalendar(string lang = "id")
        {
            try
            {
                return AuxiliaryLogic.GetCalendar(lang);
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/newsletter/subscribe")]
        public ApiResponseBase NewsletterSubscribe()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<NewsletterSubscribeApiRequest>();
                var apiResponse = AuxiliaryLogic.NewsletterSubscribe(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
