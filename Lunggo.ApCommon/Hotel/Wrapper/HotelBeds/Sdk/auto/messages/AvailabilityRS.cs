using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{
    public class AvailabilityRS : AbstractGenericResponse
    {
        public List<string> providerDetails { get; set; }
        public Hotels hotels;
        public Source source;
    }
}
