using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetReservationListInput : ReservationBase
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
