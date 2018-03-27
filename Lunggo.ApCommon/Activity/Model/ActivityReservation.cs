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
        [JsonProperty("selectedDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateAndSession DateTime { get; set; }
        [JsonProperty("packageId", NullValueHandling = NullValueHandling.Ignore)]
        public long PackageId{ get; set; }
        [JsonProperty("ticketCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> TicketCount { get; set; }
    }
    public class ActivityReservation : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Activity; }
        }

        public ActivityDetail ActivityDetails { get; set; }
        public DateAndSession DateTime { get; set; }
        public long PackageId { get; set; }
        public List<ActivityPricePackageReservation> TicketCount { get; set; }
        public decimal TotalTicketAmount { get; set; }
        public override decimal GetTotalSupplierPrice()
        {
            throw new NotImplementedException();
        }
    }

    public class DateAndSession
    {
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }
        [JsonProperty("session", NullValueHandling = NullValueHandling.Ignore)]
        public string Session { get; set; }
    }
}
