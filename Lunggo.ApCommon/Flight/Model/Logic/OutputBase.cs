using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class OutputBase
    {
        public bool IsSuccess { get; set; }
        public List<FlightError> Errors { get; set; }
        public List<string> ErrorMessages { get; set; }

        public void PartiallySucceed()
        {
            if (Errors == null)
                Errors = new List<FlightError>();
            Errors.Insert(0, FlightError.PartialSuccess);
        }

        public void DistinguishErrors()
        {
            Errors = Errors.Distinct().ToList();
            ErrorMessages = ErrorMessages.Distinct().ToList();
        }

        public void AddError(FlightError error)
        {
            if (Errors == null)
                Errors = new List<FlightError>();
            Errors.Add(error);
        }

        public void AddError(string message)
        {
            if (ErrorMessages == null)
                ErrorMessages = new List<string>();
            ErrorMessages.Add(message);
        }

        public void AddError(FlightError error, string message)
        {
            if (Errors == null)
                Errors = new List<FlightError>();
            if (ErrorMessages == null)
                ErrorMessages = new List<string>();
            Errors.Add(error);
            ErrorMessages.Add(message);
        }
    }
}
