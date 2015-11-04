using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public abstract class ResultBase
    {
        public List<FlightError> Errors { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsSuccess { get; set; }

        public void DistinguishErrors()
        {
            if (Errors != null)
                Errors = Errors.Distinct().ToList();
            if (ErrorMessages != null)
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
