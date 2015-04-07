using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum FlightSource
    {
        Wholesaler = 0,
        Airline = 1
    }
    public enum FlightError
    {
        TechnicalError = 0,
        FareIdNoLongerValid = 1,
        BookingIdNoLongerValid = 2,
        AlreadyBooked = 3,
        InvalidInputData = 4,
        ProcessFailed = 5
    }
    public enum TargetServer
    {
        Test = 0,
        Development = 1,
        Production = 2,
    }

    public enum TripType
    {
        OneWay = 0,
        Return = 1,
        MultiCity = 2,
        OpenJaw = 3,
        Circle = 4
    }

    public enum CabinClass
    {
        Economy = 0,
        Business = 1,
        First = 2
    }

    public enum PassengerType
    {
        Adult = 0,
        Child = 1,
        Infant = 2
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public enum Title
    {
        Mister = 0,
        Mistress = 1,
        Miss = 2
    }

    public enum BookingStatus
    {
        Pending = 0,
        Booked = 1,
        Ticketing = 2,
        Ticketed = 3,
        Cancelled = 4
    }
}
