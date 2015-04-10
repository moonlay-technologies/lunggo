using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
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
