using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using System;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentRequestInput : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Activity; }
        }

        public int Page { get; set; }
        public int PerPage { get; set; }

        public override decimal GetTotalSupplierPrice()
        {
            throw new NotImplementedException();
        }
    }
}
