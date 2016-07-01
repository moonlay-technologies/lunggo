using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Auxiliary.Model;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Logic
{
    public static partial class AuxiliaryLogic
    {
        public static GetCalendarApiResponse GetCalendar(string lang)
        {
            if (lang == "id")
                return new GetCalendarApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    CalendarEvents = GetEvents()
                };
            else
                return GetCalendar("id");
        }

        private static List<CalendarEvent> GetEvents()
        {
            return new List<CalendarEvent>
            {
                new CalendarEvent
                {
                    Date = new DateTime(2016,12,25),
                    Event = Event.Holiday,
                    Name = "Natal"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2016,7,7),
                    Event = Event.Holiday,
                    Name = "Sebentar Lagi"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2016,7,6),
                    Event = Event.Holiday,
                    Name = "Lebaran"
                },
            }.Where(e => e.Date >= DateTime.Now).ToList();
        }
    }
}