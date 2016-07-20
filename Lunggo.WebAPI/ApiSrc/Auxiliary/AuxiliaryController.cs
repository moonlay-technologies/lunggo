using System;
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

        [HttpGet]
        [LunggoCorsPolicy]
        //[Authorize]
        [Route("v1/promo")]
        [Route("v1/promo/{lang}")]
        public ApiResponseBase GetAllPromo(string lang = "id")
        {
            try
            {
                return AuxiliaryLogic.GetAllPromo("id");
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        //[Authorize]
        [Route("v1/hotpromo")]
        [Route("v1/hotpromo/{lang}")]
        public ApiResponseBase GetFeaturePromo(string lang = "id")
        {
            try
            {
                return AuxiliaryLogic.GetFeaturePromo("id");
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        //[Authorize]
        [Route("v1/promo/{name}")]
        [Route("v1/promo/{lang}/{name}")]
        public ApiResponseBase GetDetailPromo(string lang = "id", string name = "Holiday")
        {
            try
            {
                return AuxiliaryLogic.GetDetailPromo(lang, name);
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
