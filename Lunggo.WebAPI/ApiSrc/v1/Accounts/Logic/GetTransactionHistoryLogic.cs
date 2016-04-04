using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Accounts.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Microsoft.AspNet.Identity;
using RestSharp;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Logic
{
    public static partial class AccountsLogic
    {
        public static TransactionHistoryApiResponse GetTransactionHistory(IPrincipal user)
        {
            try
            {
                var email = user.Identity.GetEmail();
                var flight = FlightService.GetInstance();

                var rsvs = flight.GetOverviewReservationsByContactEmail(email);
                return new TransactionHistoryApiResponse
                {
                    FlightReservations = rsvs,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                return new TransactionHistoryApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERATRH99"
                };
            }
        }
    }
}