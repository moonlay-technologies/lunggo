using System;
using System.Collections.Generic;
using System.Linq;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelBookResponse : TravolutionaryResponseBase
    {
        public IEnumerable<TravolutionaryHotelBookSegment> HotelSegments { get; set; }
        private const String SegmentStatusOk = "OK";

        public bool IsBookingSuccessful()
        {
            if (HotelSegments == null || !HotelSegments.Any())
            {
                return false;
            }
            else
            {
                return HotelSegments.All(p => p.SegmentStatus == SegmentStatusOk);
            }
        }
    }

    public class TravolutionaryHotelBookSegment
    {
        public String SupplierBookingId { get; set; }
        public String SupplierBookingReference { get; set; }
        public long OrderId { get; set; }
        public long SegmentId { get; set; }
        public String SegmentStatus { get; set; }
    }
}

