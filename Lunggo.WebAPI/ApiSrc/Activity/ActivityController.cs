using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Web.Http;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.WebAPI.ApiSrc.Activity
{
    public class ActivityController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities")]
        public ApiResponseBase SearchActivity(string name="", string startDate = "", string endDate = "", string page="1", string perPage="10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            
            try
            {
                var request = new ActivitySearchApiRequest
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Page = page,
                    PerPage = perPage
                };

                var apiResponse = ActivityLogic.Search(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities/{id}")]
        public ApiResponseBase GetActivityDetail(string id = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetDetailActivityApiRequest { ActivityId = id};

                var apiResponse = ActivityLogic.GetDetail(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities/{id}/availabledates")]
        public ApiResponseBase GetAvailableDate(string id = "")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var request = new GetAvailableDatesApiRequest { ActivityId = id };

                var apiResponse = ActivityLogic.GetAvailable(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/activities/book")]
        public ApiResponseBase BookActivity()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            ActivityBookApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ActivityBookApiRequest>();
                var apiResponse = ActivityLogic.BookActivity(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
    }
}