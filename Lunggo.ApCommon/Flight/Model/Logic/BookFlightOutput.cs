using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightOutput : OutputBase
    {
        public List<BookResult> BookResults { get; set; }
        public string RsvNo { get; set; }
        public bool IsPaymentThroughThirdPartyUrl { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime? TimeLimit { get; set; }
    }

    public class BookResult
    {
        public bool IsSuccess { get; set; }
        public DateTime? TimeLimit { get; set; }
    }
}
