using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{
    public abstract class AbstractGenericResponse
    {
        public string  echoToken { get; set; }
        public AuditData auditData { get; set; }
        public string lsection { get; set; }
        public HotelbedsError error { get; set; }

        // Only for debug
        public string jsonData { get; set; }
    }
}
