using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using System;

using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservationForDisplay : ReservationForDisplayBase
    {
        public override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        [JsonProperty("itin", NullValueHandling = NullValueHandling.Ignore)]
        public FlightItineraryForDisplay Itinerary { get; set; }
    }

    public class FlightReservation : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        public List<FlightItinerary> Itineraries { get; set; }

        public override decimal GetTotalSupplierPrice()
        {
            return Itineraries.Sum(i => i.Price.Supplier);
        }
    }
}
