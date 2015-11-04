using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetAllHotel : QueryBase<GetAllHotel, GetAllHotelQueryRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            //return "select * from HotelReservations a INNER JOIN HotelReservationRoomDetails b on a.RsvNo = b.RsvNo WHERE a.RsvNo = @rsvno ";
            return "SELECT a.RsvNo, a.RsvTime, a.ContactName, a.PaymentStatusCd, b.GuestName, a.HotelNo, a.CheckInDate, a.CheckOutDate  From HotelReservations a INNER JOIN HotelReservationRoomDetails b on a.RsvNo = b.RsvNo";
        }

    }
}