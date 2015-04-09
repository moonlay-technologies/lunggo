using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lunggo.BackendWeb.Query;
using Lunggo.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.BackendWeb.Model
{
    public class FlightReservationIntegrated
    {
        public FlightReservationsTableRecord Reservation { get; set; }
        public List<FlightTripTableRecord> Trip { get; set; }
        public Dictionary<long,List<FlightTripDetailTableRecord>> TripDetail { get; set; }
        public Dictionary<long,List<FlightPassengerTableRecord>> Passenger { get; set; }

        public FlightReservationIntegrated()
        {
            Trip = new List<FlightTripTableRecord>();
            TripDetail = new Dictionary<long, List<FlightTripDetailTableRecord>>();
            Passenger = new Dictionary<long, List<FlightPassengerTableRecord>>();
        }

        public static IEnumerable<FlightReservationIntegrated> GetFromDb(
            FlightReservationSearch search,
            QueryType queryType)
        {
            List<FlightReservationIntegrated> integratedList;
            var integratedLookup = new List<FlightReservationIntegrated>();
            var condition = new { Param = search, QueryType = queryType };
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var query = GetFlightReservationExcPassengerQuery.GetInstance();
                integratedList = query.ExecuteMultiMap(
                    conn,
                    search,
                    condition,
                    (reservation, trip, tripDetail) => PopulateIntegrated(integratedLookup, reservation, trip, tripDetail),
                    "TripId,TripDetailId")
                        .Distinct().ToList();
                PopulatePassenger(conn, integratedList, search, queryType);
            }
            return integratedList;
        }

        private static FlightReservationIntegrated PopulateIntegrated(
            ICollection<FlightReservationIntegrated> integratedLookup,
            FlightReservationsTableRecord reservation,
            FlightTripTableRecord trip,
            FlightTripDetailTableRecord tripDetail)
        {
            var integrated =
                integratedLookup.SingleOrDefault(x => x.Reservation.RsvNo == reservation.RsvNo);
            var tripId = trip.TripId.GetValueOrDefault();
            if (integrated == null)
            {
                integrated = InsertIntegrated(integratedLookup, reservation, trip, tripDetail, tripId);
            }
            else
            {
                var integratedTrip = integrated.Trip.SingleOrDefault(x => x.TripId == trip.TripId);
                if (integratedTrip == null)
                {
                    InsertTrip(integrated, trip, tripDetail, tripId);
                }
                else
                {
                    InsertTripDetail(integrated, tripDetail, tripId);
                }
            }
            SortTripDetail(integrated);
            return integrated;
        }
        private static FlightReservationIntegrated InsertIntegrated(
            ICollection<FlightReservationIntegrated> integratedLookup,
            FlightReservationsTableRecord reservation,
            FlightTripTableRecord trip,
            FlightTripDetailTableRecord tripDetail,
            long tripId)
        {
            var integrated = new FlightReservationIntegrated { Reservation = reservation };
            integrated.Trip.Add(trip);
            integrated.TripDetail.Add(tripId, new List<FlightTripDetailTableRecord>());
            integrated.TripDetail[tripId].Add(tripDetail);
            integratedLookup.Add(integrated);
            return integrated;
        }
        private static void InsertTrip(
            FlightReservationIntegrated integrated,
            FlightTripTableRecord trip,
            FlightTripDetailTableRecord tripDetail,
            long tripId)
        {
            integrated.Trip.Add(trip);
            integrated.TripDetail.Add(tripId, new List<FlightTripDetailTableRecord>());
            integrated.TripDetail[tripId].Add(tripDetail);
        }
        private static void InsertTripDetail(
            FlightReservationIntegrated integrated,
            FlightTripDetailTableRecord tripDetail,
            long tripId)
        {
            integrated.TripDetail[tripId].Add(tripDetail);
        }
        private static void SortTripDetail(FlightReservationIntegrated integrated)
        {
            foreach (var id in integrated.Trip.Select(tripItem => tripItem.TripId.GetValueOrDefault()))
            {
                integrated.TripDetail[id].Sort(
                    (x, y) => x.SequenceNo.GetValueOrDefault() - y.SequenceNo.GetValueOrDefault());
            }
        }
        private static void PopulatePassenger(
            IDbConnection conn,
            IEnumerable<FlightReservationIntegrated> integratedList,
            FlightReservationSearch search,
            QueryType queryType)
        {
            foreach (var integrated in integratedList)
            {
                foreach (var trip in integrated.Trip)
                {
                    var query = GetFlightPassengerQuery.GetInstance();
                    var tripId = trip.TripId.GetValueOrDefault();
                    var passenger = query.Execute(
                        conn,
                        new { tripId, search.PassengerName },
                        new { QueryType = queryType })
                            .ToList();
                    integrated.Passenger.Add(tripId, passenger);
                }
            }
        }
    }
}
