using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightItineraryQueryRecord : QueryRecord
    {
        internal string RsvNo { get; set; }
        internal FlightFareItinerary Itinerary { get; set; }
        internal BookResult BookResult { get; set; }
        internal TripType TripType { get; set; }
        internal decimal SourcePrice { get; set; }
        internal decimal SourceCurrency { get; set; }
        internal decimal SourceExchangeRate { get; set; }
        internal decimal LocalPrice { get; set; }
        internal decimal LocalCurrency { get; set; }
        internal decimal LocalExchangeRate { get; set; }
        internal decimal IdrPrice { get; set; }
    }
}
