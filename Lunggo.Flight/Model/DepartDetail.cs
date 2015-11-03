using System;

namespace Lunggo.Flight.Model
{
    public class DepartDetail
    {
        public string FlightCode { get; set; }
        public string DepartFrom { get; set; }
        public string ArrivedAt { get; set; }
        public TimeSpan DepartTime { get; set; }
        public TimeSpan ArrivedTime { get; set; }
    }
}
