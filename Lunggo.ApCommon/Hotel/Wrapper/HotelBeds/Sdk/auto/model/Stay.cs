using System;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Stay
    {
        public string checkIn { get; set; }
        public string checkOut { get; set; }
        public int shiftDays { get; set; }
        public bool allowOnlyShift { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkIn"></param>
        /// <param name="checkOut"></param>
        /// <param name="shiftDays"></param>
        /// <param name="allowOnlyShift"></param>
        public Stay(DateTime checkIn, DateTime checkOut, int shiftDays, bool allowOnlyShift)
        {
            this.checkIn = checkIn.ToString("yyyy-MM-dd");
            this.checkOut = checkOut.ToString("yyyy-MM-dd");
            this.shiftDays = shiftDays;
            this.allowOnlyShift = allowOnlyShift;
        }
    }
}
