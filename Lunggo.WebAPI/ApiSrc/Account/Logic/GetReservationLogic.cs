using System.Net;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetReservationApiResponse GetReservation(string rsvNo)
        {
            var user = HttpContext.Current.User;
            try
            {
                if (rsvNo.IsFlightRsvNo())
                {
                    var flight = FlightService.GetInstance();
                    var rsv = flight.GetReservationForDisplay(rsvNo);
                    if (user.IsInRole("Admin") || user.Identity.GetEmail() == rsv.Contact.Email)
                        return new GetReservationApiResponse
                        {
                            ProductType = ProductType.Flight,
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
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}