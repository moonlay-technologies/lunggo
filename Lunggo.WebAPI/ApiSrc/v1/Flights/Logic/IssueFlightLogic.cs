using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Microsoft.Data.Edm.Csdl;
using StackExchange.Redis;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightIssueApiResponse IssueFlight(FlightIssueApiRequest request)
        {
            if (IsValid(request))
            {
                var issueServiceRequest = PreprocessServiceRequest(request);
                var issueServiceResponse = FlightService.GetInstance().IssueTicket(issueServiceRequest);
                var apiResponse = AssembleApiResponse(issueServiceResponse, request);
                return apiResponse;
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