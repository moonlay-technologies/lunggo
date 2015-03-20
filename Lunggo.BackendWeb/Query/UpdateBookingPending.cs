using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class UpdateBookingPending : QueryBase<UpdateBookingPending, GetBookingPendingRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE HotelReservations SET PaymentStatusCd = '02' WHERE ");
            var i = 0;
            
            foreach (var whut in condition)
            {
                if (condition[i].rdSelection == "paid")
                {
                    queryBuilder.Append("RsvNo = " + condition[i].RsvNo + " OR ");
                }
                i++;
            }

            queryBuilder.Remove(queryBuilder.Length - 4, 4);
            
            
            // return "SELECT RsvNo, ContactName, RsvTime, RsvStatusCd, FinalPrice, PaymentMethodCd, PaymentStatusCd  From HotelReservations";

            return queryBuilder.ToString();
        }

    }
}