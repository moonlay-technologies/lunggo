using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GenerateQuestion(GenerateQuestionApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new GenerateQuestionApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var output = ActivityService.GetInstance().GenerateQuestion(apiRequest.RsvNo);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        public static GenerateQuestionApiResponse AssembleApiResponse(GenerateQuestionOutput output)
        {
            if (output == null)
            {
                return new GenerateQuestionApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_RSVNO"
                };
            }
            return new GenerateQuestionApiResponse
            {
                Questions = output.Questions,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}