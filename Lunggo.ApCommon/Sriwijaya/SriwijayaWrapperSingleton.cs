using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Sriwijaya
{
    internal partial class SriwijayaWrapper : WrapperBase
    {
        private static readonly SriwijayaWrapper Instance = new SriwijayaWrapper();
        private bool _isInitialized;
        private static readonly SriwijayaClientHandler Client = SriwijayaClientHandler.GetClientInstance();

        private static readonly string UserName = "MLWAG0215";
        private static readonly string Password = "TRAVELMADEZY";

        private SriwijayaWrapper()
        {
            
        }

        internal static SriwijayaWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                Client.Init(UserName, Password);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("SriwijayaWrapper is already initialized");
            }
        }

        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            throw new NotImplementedException();
        }

        internal override CancelBookingResult CancelBooking(string bookingId)
        {
            throw new NotImplementedException();
        }

        internal override List<BookingStatusInfo> GetBookingStatus()
        {
            throw new NotImplementedException();
        }

        internal override GetRulesResult GetRules(string fareId)
        {
            throw new NotImplementedException();
        }

        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            throw new NotImplementedException();
        }

        internal override OrderTicketResult OrderTicket(string bookingId)
        {
            throw new NotImplementedException();
        }

        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            throw new NotImplementedException();
        }

        internal override SearchFlightResult SpecificSearchFlight(SpecificSearchConditions flightFareItinerary)
        {
            throw new NotImplementedException();
        }
    }
}
