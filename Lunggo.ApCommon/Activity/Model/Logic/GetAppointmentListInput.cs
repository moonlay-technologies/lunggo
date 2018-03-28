using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using System;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetAppointmentListInput : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Activity; }
        }

        public bool OrderParam { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public override decimal GetTotalSupplierPrice()
        {
            throw new NotImplementedException();
        }
    }
}
