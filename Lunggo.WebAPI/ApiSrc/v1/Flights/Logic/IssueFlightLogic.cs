using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightIssueApiResponse IssueFlight(FlightIssueApiRequest request)
        {
            if (IsValid(request))
            {
                var issueServiceRequest = PreprocessServiceRequest(request);
                FlightService.GetInstance().IssueTicket(issueServiceRequest);
                return new FlightIssueApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Success, reservation is being processed for issuance."
                };
            }
            else
            {
                return new FlightIssueApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Reservation number is missing.",
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
    }
}