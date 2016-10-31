using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum HotelError
    {
        TechnicalError = 0, 
        RateKeyNoLongerValid = 1,
        BookingIdNoLongerValid = 2,
        AlreadyBooked = 3,
        InvalidInputData = 4,
        FailedOnSupplier = 5,
        PartialSuccess = 6,
        NotEligibleToIssue = 7,
        PaymentFailed = 8,
        PriceChanged = 9,
        SearchIdNoLongerValid = 10,
    }
}
