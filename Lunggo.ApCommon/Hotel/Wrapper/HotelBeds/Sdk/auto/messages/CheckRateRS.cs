using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{
    public class CheckRateRS : AbstractGenericResponse
    {
        public List<string> providerDetails { get; set; }
        public model.Hotel hotel { get; set; }
        public Source source { get; set; }
    }
}
