﻿using Lunggo.Framework.Database;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Model;
using Lunggo.Repository.TableRecord;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Query
{
    public class GetFlightHistory : QueryBase<GetFlightHistory, GetFlightHistoryRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT	a.RsvNo, a.PaymentMethodCd, a.MemberCd, a.OverallTripTypeCd,  b.ItineraryId, c.OriginAirportCd , c.DestinationAirportCd , c.DepartureDate, d.TripId, d.AirLineCd  FROM FlightReservation a INNER JOIN FlightItinerary b ON a.RsvNo = b.RsvNo INNER JOIN FlightTrip c ON b.ItineraryId = c.ItineraryId INNER JOIN FlightSegment d ON c.TripId = d.TripId WHERE MemberCd = @idMember ORDER BY  c.DepartureDate";
        }

    }
}