using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Web.Http;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.WebAPI.ApiSrc.Activity
{
    public class ActivityController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activities")]
        public ApiResponseBase SearchActivity(string searchActivityType="ActivityName", string name="", string date="", string page="1", string perPage="10")
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            if (searchActivityType != "ActivityName")
            {
                if(searchActivityType != "ActivityDate")
                    return ActivityLogic.Search(null);
            }

            var searchType = SearchActivityTypeCd.Mnemonic(searchActivityType);

            try
            {
                var closeYear = "20" + date.Substring(4, 2);
                var closeDate = new DateTime(Convert.ToInt32(closeYear), Convert.ToInt32(date.Substring(2, 2)),
                    Convert.ToInt32(date.Substring(0, 2)));

                var request = new ActivitySearchApiRequest
                {
                    SearchType = searchType,
                    Filter = new ActivityFilter { Name = name, CloseDate = closeDate },
                    Page = Convert.ToInt32(page),
                    PerPage = Convert.ToInt32(perPage)
                };

                var apiResponse = ActivityLogic.Search(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/activity/select")]
        public ApiResponseBase SelectActivity()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            ActivitySelectApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ActivitySelectApiRequest>();
                var apiResponse = ActivityLogic.Select(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
    }
}