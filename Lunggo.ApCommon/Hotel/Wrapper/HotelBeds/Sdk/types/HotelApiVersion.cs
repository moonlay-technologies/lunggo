namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.types
{
    public class HotelApiVersion
    {
        public enum versions { V0_2, V1 };

        public versions version { get; set; }

        public HotelApiVersion(versions version)
        {
            this.version = version;
        }
    }
}
