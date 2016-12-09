using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelReservationForDisplay : ReservationForDisplayBase
    {
        public override ProductType Type
        {
            get { return ProductType.Hotel; }
        }

        [JsonProperty("hotelDetail", NullValueHandling = NullValueHandling.Ignore)]
        public HotelDetailForDisplay HotelDetail { get; set; }
    }
    public class HotelReservation : ReservationBase
    {
        public override ProductType Type
        {
            get { return ProductType.Hotel; }
        }

        public HotelDetail HotelDetails { get; set; }

        public override decimal GetTotalSupplierPrice()
        {
            return HotelDetails.Rooms.SelectMany(r => r.Rates).Sum(r => r.Price.Supplier);
        }
    }
}
