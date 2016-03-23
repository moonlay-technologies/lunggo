using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase IssueFlight(FlightIssueApiRequest request)
        {
            if (IsValid(request))
            {
                var issueServiceRequest = PreprocessServiceRequest(request);
                var issueServiceResponse = FlightService.GetInstance().IssueTicket(issueServiceRequest);
                var apiResponse = AssembleApiResponse(issueServiceResponse);
                return apiResponse;
            }
            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFISS01"
                };
            }
        }

        private static bool IsValid(FlightIssueApiRequest request)
        {
            return
                request != null &&
                request.RsvNo != null;
        }

        private static IssueTicketInput PreprocessServiceRequest(FlightIssueApiRequest request)
        {
            var issueServiceRequest = new IssueTicketInput
            {
                RsvNo = request.RsvNo
            };
            return issueServiceRequest;
        }

        private static ApiResponseBase AssembleApiResponse(IssueTicketOutput issueServiceResponse)
        {
            if (issueServiceResponse.IsSuccess)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            else
            {
                switch (issueServiceResponse.Errors[0])
                {
                    case FlightError.InvalidInputData:
                        return new ApiResponseBase
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorCode = "ERFISS02"
                        };
                    case FlightError.NotEligibleToIssue:
                        return new ApiResponseBase
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERFISS03"
                        };
                    default:
                        return new ApiResponseBase
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERFISS04"
                        };
                }
            }
        }
    }
}