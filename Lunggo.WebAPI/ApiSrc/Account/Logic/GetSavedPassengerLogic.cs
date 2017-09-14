using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Service;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetSavedPassengerResponse GetSavedPassenger()
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                return new GetSavedPassengerResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERAGSPR01"
                };
            }
            var email = user.Identity.GetUser().Email;
            var paxList = FlightService.GetInstance().GetSavedPassengerListByUser(email);
            return new GetSavedPassengerResponse
            {
                StatusCode = HttpStatusCode.OK,
                PaxList = paxList
            };
        }
    }
}