using System.Net;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetReservationApiResponse GetReservation(string rsvNo)
        {
            var user = HttpContext.Current.User;
            if (rsvNo.StartsWith("1"))
            {
                var flight = FlightService.GetInstance();
                var rsv = flight.GetReservationForDisplay(rsvNo);

                if (rsv == null)
                    return new GetReservationApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERARSV01"
                    };

                if (user.IsInRole("Admin") ||
                    (user.Identity.IsUserAuthorized() && user.Identity.GetUser().Id == rsv.Booker.Id) ||
                    user.Identity.GetDeviceId() == rsv.DeviceId)
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
            else
            {
                var hotel = HotelService.GetInstance();
                var rsv = hotel.GetReservationForDisplay(rsvNo);

                if (rsv == null)
                    return new GetReservationApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERARSV01"
                    };

                if (user.IsInRole("Admin") ||
                    (user.Identity.IsUserAuthorized() && user.Identity.GetUser().Id == rsv.Booker.Id) ||
                    user.Identity.GetDeviceId() == rsv.DeviceId)
                    return new GetReservationApiResponse
                    {
                        ProductType = ProductType.Hotel,
                        HotelReservation = rsv,
                        StatusCode = HttpStatusCode.OK
                    };
                else
                    return new GetReservationApiResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        ErrorCode = "ERARSV02"
                    };
            }
            
        }
    }
}