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
                    Date = new DateTime(2016,9,12),
                    Event = Event.Holiday,
                    Name = "Idul Adha 1437 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2016,10,2),
                    Event = Event.Holiday,
                    Name = "Tahun Baru Hijriyah 1438 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2016,12,12),
                    Event = Event.Holiday,
                    Name = "Maulid Nabi Muhammad SAW 1438 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2016,12,25),
                    Event = Event.Holiday,
                    Name = "Natal"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,1,1),
                    Event = Event.Holiday,
                    Name = "Tahun Baru 2017"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,2,28),
                    Event = Event.Holiday,
                    Name = "Tahun Baru Imlek 2568"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,3,28),
                    Event = Event.Holiday,
                    Name = "Nyepi"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,4,14),
                    Event = Event.Holiday,
                    Name = "Wafat Isa Al Masih"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,4,24),
                    Event = Event.Holiday,
                    Name = "Isra Miraj Nabi Muhammad SAW 1438 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,5,1),
                    Event = Event.Holiday,
                    Name = "Hari Buruh"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,5,11),
                    Event = Event.Holiday,
                    Name = "Waisak"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,5,25),
                    Event = Event.Holiday,
                    Name = "Kenaikan Isa Almasih"
                },
                //new CalendarEvent
                //{
                //    Date = new DateTime(2017,6,1),
                //    Event = Event.Holiday,
                //    Name = "Hari Lahir Pancasila"
                //},
                new CalendarEvent
                {
                    Date = new DateTime(2017,6,25),
                    Event = Event.Holiday,
                    Name = "Idul Fitri 1438 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,6,26),
                    Event = Event.Holiday,
                    Name = "Idul Fitri 1438 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,8,17),
                    Event = Event.Holiday,
                    Name = "Hari Kemerdekaan Indonesia"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,9,1),
                    Event = Event.Holiday,
                    Name = "Idul Adha 1438 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,9,21),
                    Event = Event.Holiday,
                    Name = "Tahun Baru Hijriyah 1439 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,12,1),
                    Event = Event.Holiday,
                    Name = "Maulid Nabi Muhammad SAW 1439 H"
                },
                new CalendarEvent
                {
                    Date = new DateTime(2017,12,25),
                    Event = Event.Holiday,
                    Name = "Natal"
                },


            }.Where(e => e.Date >= DateTime.Now).ToList();
        }
    }
}