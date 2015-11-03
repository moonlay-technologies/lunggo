using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightOutput : OutputBase
    {
        public List<BookResult> BookResults { get; set; }
        public string RsvNo { get; set; }
    }

    public class BookResult
    {
        public bool IsSuccess { get; set; }
        public DateTime? TimeLimit { get; set; }
    }
}
