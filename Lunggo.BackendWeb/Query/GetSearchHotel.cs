using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetSearchHotel : DbQueryBase<GetSearchHotel, GetSearchHotelRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT a.RsvNo, a.ContactName, a.HotelNo, a.RsvTime, a.CheckInDate, a.CheckOutDate, b.GuestName, a.RsvStatusCd, a.FinalPrice, a.PaymentMethodCd, a.PaymentStatusCd from HotelReservations a INNER JOIN HotelReservationRoomDetails b on a.RsvNo = b.RsvNo WHERE a.RsvNo = @RsvNo ";
        }

    }
}