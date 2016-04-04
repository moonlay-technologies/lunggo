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
        public static GetReservationApiResponse GetReservation(string rsvNo, IPrincipal user)
        {
            try
            {
                if (rsvNo.IsFlightRsvNo())
                {
                    var flight = FlightService.GetInstance();
                    var rsv = flight.GetReservationForDisplay(rsvNo);
                    if (user.IsInRole("Admin") || user.Identity.GetEmail() == rsv.Contact.Email)
                        return new GetReservationApiResponse
                        {
                            ReservationType = ReservationType.Flight,
                            FlightReservation = rsv,
                            StatusCode = HttpStatusCode.OK
                        };
                    else
                        return new GetReservationApiResponse
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            ErrorCode = "ERARSV02"
                        };
                }
                if (rsvNo.IsHotelRsvNo())
                {
                    
                }
                if (rsvNo.IsActivityRsvNo())
                {
                    
                }
                return new GetReservationApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERARSV01"
                };
            }
            catch
            {
                return new GetReservationApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERARSV99"
                };
            }
        }
    }
}