using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelRateRule : OrderRuleBase
    {
        protected override ProductType Type
        {
            get { return ProductType.Hotel; }
        }

        public List<DayOfWeek> BookingDays { get; set; }
        public List<DateSpanRule> BookingDates { get; set; }
        public List<DateSpanRule> StayDates { get; set; }
        public List<int> StayDurations { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Destinations{ get; set; }
        public List<string> RoomTypes { get; set; }
        public List<string> HotelStars { get; set; }
        public List<string> HotelChains { get; set; }
        public int MaxAdult { get; set; }
        public int MinAdult { get; set; }
        public int MaxChild { get; set; }
        public int MinChild { get; set; }
        public List<string> Boards { get; set; }

        public HotelRateRule()
        {
            BookingDays = new List<DayOfWeek>();
            BookingDates = new List<DateSpanRule>();
            StayDates = new List<DateSpanRule>();
            StayDurations = new List<int>();
            Countries = new List<string>();
            Destinations= new List<string>();
            RoomTypes = new List<string>();
            HotelStars = new List<string>();
            HotelChains = new List<string>();
            Boards = new List<string>();
            MaxAdult = 10;
            MinAdult = 1;
            MaxChild = 5;
            MinChild = 0;
        }

        internal static HotelRateRule GetFromDb(long ruleId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetHotelRateRuleQuery.GetInstance().Execute(conn, new { RuleId = ruleId }).Single();
                return new HotelRateRule
                {
                    RuleId = record.Id.GetValueOrDefault(),
                    BookingDays = record.BookingDays.Deserialize<List<DayOfWeek>>(), //
                    BookingDates = record.BookingDates.Deserialize<List<DateSpanRule>>(), //
                    StayDates = record.StayDates.Deserialize<List<DateSpanRule>>(),
                    StayDurations = record.StayDurations.Deserialize<List<int>>(), //
                    HotelStars = record.HotelStars.Deserialize<List<string>>(), //
                    Countries = record.Countries.Deserialize<List<string>>(), //
                    Destinations = record.Destinations.Deserialize<List<string>>(), //
                    HotelChains = record.HotelChains.Deserialize<List<string>>(), //
                    Boards = record.Boards.Deserialize<List<string>>(), //
                    MaxAdult = record.MaxAdult.GetValueOrDefault(),
                    MaxChild = record.MaxChild.GetValueOrDefault(),
                    MinAdult = record.MinAdult.GetValueOrDefault(),
                    MinChild = record.MinChild.GetValueOrDefault(),
                    ConstraintCount = record.ConstraintCount.GetValueOrDefault(),
                    Priority = record.Priority.GetValueOrDefault(),
                    RoomTypes = record.RoomTypes.Deserialize<List<string>>(),
                };
            }
        }

        public class DateSpanRule
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public bool Contains(DateTime date)
            {
                return date >= Start && date <= End;
            }
        }
    }

}
