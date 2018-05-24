using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentListActiveInput : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Activity; }
        }

        public List<string> BookingStatusCdList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public override decimal GetTotalSupplierPrice()
        {
            throw new NotImplementedException();
        }
    }
}
