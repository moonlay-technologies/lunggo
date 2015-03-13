using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetSearchHotelDetail : QueryBase<GetSearchHotelDetail, GetSearchHotelRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT a.RsvNo, a.ContactName, a.HotelNo, a.RsvTime, a.CheckInDate, a.CheckOutDate, b.GuestName, a.RsvStatusCd, a.FinalPrice, a.PaymentMethodCd, a.PaymentStatusCd FROM HotelReservations a INNER JOIN HotelReservationRoomDetails b on a.RsvNo = b.RsvNo ");



            var queryWhereBuilder = new StringBuilder();
            queryWhereBuilder.Append("WHERE(");

            if (condition.HotelNo != null && condition.HotelNo != "")
            {
                queryWhereBuilder.Append(" HotelNo = @HotelNo AND"); 
            }

            if (condition.rdSelection == 0)
            {
                if (condition.RsvTime != null)
                {
                    queryWhereBuilder.Append("( RsvTime BETWEEN @RsvTime AND @RsvTime + ' 23:59:59') AND");
                }
            }
            else if (condition.rdSelection == 1)
            {
                if (condition.RsvDateStart != null && condition.RsvDateEnd != null)
                {
                    queryWhereBuilder.Append("( RsvTime BETWEEN @RsvDateStart AND @RsvDateEnd ) AND");
                }
            }
            else if (condition.rdSelection == 2)
            {
                if (condition.RsvMonth != null && condition.RsvYear != null)
                {
                    queryWhereBuilder.Append("( MONTH(a.RsvTime) =  @RsvMonth AND YEAR(a.RsvTime) = @RsvYear ) AND");
                }
            }
       
            if (condition.CheckInDate != null)
            {
                queryWhereBuilder.Append(" ( CheckInDate BETWEEN @CheckInDate AND @CheckInDate + ' 23:59:59') AND");
            }
            if (condition.CheckOutDate != null)
            {
                queryWhereBuilder.Append(" ( CheckOutDate BETWEEN @CheckOutDate AND @CheckOutDate + ' 23:59:59') AND");
            }
            if (condition.GuestName != null && condition.GuestName != "")
            {
                queryWhereBuilder.Append("GuestName = @GuestName AND");
            }
            if (condition.ContactName != null && condition.ContactName != "")
            {
                queryWhereBuilder.Append("ContactName = @ContactName AND");
            }

            if (queryWhereBuilder.Length == 6)
            {
                queryWhereBuilder.Clear();
            }
            else
            {
                queryWhereBuilder.Remove(queryWhereBuilder.Length - 4, 4);
            }

            queryWhereBuilder.Append(" ) ");

            queryBuilder.Append(queryWhereBuilder);

            return queryBuilder.ToString();    
        }

    }
}