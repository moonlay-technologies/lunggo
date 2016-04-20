using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetReservationQuery : QueryBase<GetReservationQuery, FlightReservation, FlightReservationTableRecord, FlightItineraryTableRecord, FlightTripTableRecord, FlightSegmentTableRecord, FlightPassengerTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT r.RsvNo, r.RsvTime, r.InvoiceNo, r.OverallTripTypeCd, ");
            clauseBuilder.Append(@"r.ContactName, r.ContactEmail, r.ContactCountryCd, r.ContactPhone, ");
            clauseBuilder.Append(@"r.PaymentId, r.PaymentMediumCd, r.PaymentMethodCd, r.PaymentTimeLimit, ");
            clauseBuilder.Append(@"r.PaymentTime, r.PaymentStatusCd, r.PaymentTargetAccount, r.PaymentUrl, ");
            clauseBuilder.Append(@"r.RefundAmount, r.RefundTime, r.RefundTargetBank, r.RefundTargetAccount, ");
            clauseBuilder.Append(@"r.DiscountId, r.DiscountPercentage, r.DiscountConstant, ");
            clauseBuilder.Append(@"r.DiscountNominal, r.DiscountName, r.TransferCode, ");
            clauseBuilder.Append(@"r.VoucherCode, r.FinalPrice, r.PaidAmount, ");
            clauseBuilder.Append(@"i.ItineraryId, i.BookingId, i.RsvNo, i.BookingStatusCd, i.FareTypeCd, i.CanHold, ");
            clauseBuilder.Append(@"i.LocalPrice, i.MarginId, i.MarginCoefficient, i.MarginConstant, i.MarginNominal, ");
            clauseBuilder.Append(@"t.TripId, t.ItineraryId, t.OriginAirportCd, t.DestinationAirportCd, t.DepartureDate, ");
            clauseBuilder.Append(@"s.SegmentId, s.TripId, s.Pnr, s.OperatingAirlineCd, s.AirlineCd, s.FlightNumber, ");
            clauseBuilder.Append(@"s.DepartureAirportCd, s.DepartureTerminal, s.DepartureTime, ");
            clauseBuilder.Append(@"s.ArrivalAirportCd, s.ArrivalTerminal, s.ArrivalTime, ");
            clauseBuilder.Append(@"s.Duration, s.CabinClassCd, s.Baggage, ");
            clauseBuilder.Append(@"p.PassengerId, p.RsvNo, p.TitleCd, p.FirstName, p.LastName, p.PassengerTypeCd ");
            clauseBuilder.Append(@"FROM FlightReservation AS r ");
            clauseBuilder.Append(@"INNER JOIN FlightItinerary AS i ON i.RsvNo = r.RsvNo ");
            clauseBuilder.Append(@"INNER JOIN FlightTrip AS t ON t.ItineraryId = i.ItineraryId ");
            clauseBuilder.Append(@"INNER JOIN FlightSegment AS s ON s.TripId = t.TripId ");
            clauseBuilder.Append(@"INNER JOIN FlightPassenger AS p ON p.RsvNo = r.RsvNo ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE r.RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
