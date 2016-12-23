using System;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.types
{
    class HotelSDKException : Exception
    {
        public const long serialVersionUID = 1L;
        private readonly HotelbedsError error;

        HotelSDKException(HotelbedsError error) : base()
        {
            this.error = error;
        }

        HotelSDKException(HotelbedsError error, String message, Exception innerEx) : base(message, innerEx)
        {
            this.error = error;
        }
    }
}
