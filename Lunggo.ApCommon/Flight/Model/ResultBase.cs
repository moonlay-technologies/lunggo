using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.Framework.Error;

namespace Lunggo.ApCommon.Flight.Model
{
    public abstract class ResultBase
    {
        public List<FlightError> Errors { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool Success { get; set; }

        protected ResultBase()
        {
            Errors = new List<FlightError>();
            ErrorMessages = new List<string>();
        }
    }
}
