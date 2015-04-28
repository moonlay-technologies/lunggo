using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class OutputBase
    {
        public bool IsSuccess { get; set; }
        public List<FlightError> Errors { get; set; }
        public List<string> ErrorMessages { get; set; }

        public OutputBase()
        {
            Errors = new List<FlightError>();
            ErrorMessages = new List<string>();
        }
    }
}
