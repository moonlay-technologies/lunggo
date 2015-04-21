﻿using System;
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
            return "SELECT RsvNo, RsvTime, PaymentStatusCd, HotelNo, ContactName, ContactEmail, ContactPhone, ContactAddress, PaymentMethodCd, FinalPrice from HotelReservations WHERE RsvNo = @rsvno ";
        }

    }
}