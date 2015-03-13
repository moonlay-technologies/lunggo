using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetHotelBookingDetail : QueryBase<GetHotelBookingDetail, GetHotelBookingDetailRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT a.RsvNo, a.RsvTime, a.PaymentStatusCd, a.MemberCd, a.HotelNo, a.PaymentMethodCd, a.FinalPrice from HotelReservations a INNER JOIN HotelReservationRoomDetails b on a.RsvNo = b.RsvNo WHERE a.RsvNo = @rsvno ";
        }

    }
}