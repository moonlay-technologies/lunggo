using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightIssueApiResponse IssueFlight(FlightIssueApiRequest request)
        {
            if (IsValid(request))
            {
                var issueServiceRequest = PreprocessServiceRequest(request);
                FlightService.GetInstance().IssueTicket(issueServiceRequest);
                //var apiResponse = AssembleApiResponse(issueServiceResponse, request);
                return new FlightIssueApiResponse
                {
                    IsSuccess = true,
                    OriginalRequest = request
                };
            }
            else
            {
                return new FlightIssueApiResponse
                {
                    IsSuccess = false,
                    OriginalRequest = request
                };
            }
        }

        private static bool IsValid(FlightIssueApiRequest request)
        {
            return
                request != null &&
                request.RsvNo != null;
        }

        private static FlightIssueApiResponse AssembleApiResponse(IssueTicketOutput issueServiceResponse, FlightIssueApiRequest request)
        {
            return new FlightIssueApiResponse
            {
                IsSuccess = issueServiceResponse.IsSuccess,
                OriginalRequest = request
            };
        }

        private static IssueTicketInput PreprocessServiceRequest(FlightIssueApiRequest request)
        {
            var issueServiceRequest = new IssueTicketInput
            {
                RsvNo = request.RsvNo
            };
            return issueServiceRequest;
        }
    }
}