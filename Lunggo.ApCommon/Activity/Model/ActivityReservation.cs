using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{
    public class ActivityReservationForDisplay : ReservationForDisplayBase
    {
        public override ProductType Type
        {
            get { return ProductType.Activity; }
        }

        [JsonProperty("activityDetail", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityDetailForDisplay ActivityDetail { get; set; }
        [JsonProperty("ticketCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TicketCount { get; set; }
    }
    public class ActivityReservation : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Activity; }
        }

        public ActivityDetail ActivityDetails { get; set; }
        public int? TicketCount { get; set; }
        public override decimal GetTotalSupplierPrice()
        {
            throw new NotImplementedException();
        }
    }
}
