using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal FlightReservationForDisplay ConvertToReservationForDisplay(FlightReservation reservation)
        {
            if (reservation == null)
                return null;

            return new FlightReservationForDisplay
            {
                RsvNo = reservation.RsvNo,
                RsvTime = reservation.RsvTime.TruncateMilliseconds(),
                RsvDisplayStatus = MapReservationStatus(reservation),
                CancellationType = reservation.CancellationType,
                CancellationTime = reservation.CancellationTime.TruncateMilliseconds(),
                Itinerary = ConvertToItineraryForDisplay(reservation.Itineraries),
                Contact = reservation.Contact,
                Pax = ConvertToPaxForDisplay(reservation.Pax),
                Payment = _paymentService.ConvertToPaymentDetailsForDisplay(reservation.Payment),
                UserId = reservation.User != null ? reservation.User.Id : null,
                DeviceId = reservation.State != null ? reservation.State.DeviceId : null
            };
        }

        private static RsvDisplayStatus MapReservationStatus(FlightReservation reservation)
        {
            var paymentStatus = reservation.Payment.Status;
            var paymentMethod = reservation.Payment.Method;
            var rsvStatus = reservation.RsvStatus;

            if (rsvStatus == RsvStatus.Cancelled || paymentStatus == PaymentStatus.Cancelled)
                return RsvDisplayStatus.Cancelled;
            if (rsvStatus == RsvStatus.Expired || paymentStatus == PaymentStatus.Expired)
                return RsvDisplayStatus.Expired;
            if (paymentStatus == PaymentStatus.Denied)
                return RsvDisplayStatus.PaymentDenied;
            if (paymentStatus == PaymentStatus.Failed)
                return RsvDisplayStatus.FailedUnpaid;
            if (rsvStatus == RsvStatus.Failed)
                return paymentStatus == PaymentStatus.Settled 
                    ? RsvDisplayStatus.FailedPaid 
                    : RsvDisplayStatus.FailedUnpaid;
            if (paymentMethod == PaymentMethod.Undefined)
                return RsvDisplayStatus.Reserved;
            if (paymentStatus == PaymentStatus.Settled)
                return reservation.Itineraries.All(i => i.BookingStatus == BookingStatus.Ticketed)
                    ? RsvDisplayStatus.Issued
                    : RsvDisplayStatus.Paid;
            if (paymentStatus != PaymentStatus.Settled)
                return (paymentMethod == PaymentMethod.VirtualAccount || paymentMethod == PaymentMethod.BankTransfer)
                    ? RsvDisplayStatus.PendingPayment
                    : RsvDisplayStatus.VerifyingPayment;
            return RsvDisplayStatus.Undefined;
        }

        internal FlightItineraryForDisplay ConvertToItineraryForDisplay(List<FlightItinerary> itins)
        {
            if (itins == null || itins.Count == 0 || itins.Contains(null))
                return null;

            decimal? aOri, cOri, iOri, aNet, cNet, iNet;
            decimal tOri, tNet;
            var summedItin = new FlightItinerary {
                AdultCount = itins[0].AdultCount,
                ChildCount = itins[0].ChildCount,
                InfantCount = itins[0].InfantCount,
                AdultPricePortion = itins[0].AdultPricePortion,
                ChildPricePortion = itins[0].ChildPricePortion,
                InfantPricePortion = itins[0].InfantPricePortion,
                NetAdultPricePortion = itins[0].NetAdultPricePortion,
                NetChildPricePortion = itins[0].NetChildPricePortion,
                NetInfantPricePortion = itins[0].NetInfantPricePortion,
                Price = new Price
            {
                Supplier = itins.Sum(itin => itin.Price.Supplier),
                SupplierCurrency = itins[0].Price.SupplierCurrency,
                OriginalIdr = itins.Sum(itin => itin.Price.OriginalIdr),
                Rounding = itins.Sum(itin => itin.Price.Rounding),
                MarginNominal = itins.Sum(itin => itin.Price.MarginNominal),
                FinalIdr = itins.Sum(itin => itin.Price.FinalIdr),
                Local = itins.Sum(itin => itin.Price.Local),
                LocalCurrency = itins[0].Price.LocalCurrency
            }};

            CalculateFare(summedItin,
                out tOri, out aOri, out cOri, out iOri, 
                out tNet, out aNet, out cNet, out iNet);

            return new FlightItineraryForDisplay
            {
                AdultCount = itins[0].AdultCount,
                ChildCount = itins[0].ChildCount,
                InfantCount = itins[0].InfantCount,
                RequireBirthDate = itins.Exists(itin => itin.RequireBirthDate),
                RequirePassport = itins.Exists(itin => itin.RequirePassport),
                RequireSameCheckIn = itins.Exists(itin => itin.RequireSameCheckIn),
                RequireNationality = itins.Exists(itin => itin.RequireNationality),
                RequestedCabinClass = itins[0].RequestedCabinClass,
                CanHold = itins.TrueForAll(itin => itin.CanHold),
                Currency = itins[0].Price.LocalCurrency,
                TripType = itins[0].RequestedTripType,
                OriginalTotalFare = tOri,
                OriginalAdultFare = aOri,
                OriginalChildFare = cOri,
                OriginalInfantFare = iOri,
                NetTotalFare = tNet,
                NetAdultFare = aNet,
                NetChildFare = cNet,
                NetInfantFare = iNet,
                Trips = ExtractTripsForDisplay(itins),
                RegisterNumber = 0
            };
        }

        internal FlightItineraryForDisplay ConvertToItineraryForDisplay(FlightItinerary itinerary)
        {
            if (itinerary == null)
                return null;

            decimal? aOri, cOri, iOri, aNet, cNet, iNet;
            decimal tOri, tNet;
            CalculateFare(itinerary,
                out tOri, out aOri, out cOri, out iOri, 
                out tNet, out aNet, out cNet, out iNet);

            return new FlightItineraryForDisplay
            {
                AdultCount = itinerary.AdultCount,
                ChildCount = itinerary.ChildCount,
                InfantCount = itinerary.InfantCount,
                RequireBirthDate = itinerary.RequireBirthDate,
                RequirePassport = itinerary.RequirePassport,
                RequireSameCheckIn = itinerary.RequireSameCheckIn,
                RequireNationality = itinerary.RequireNationality,
                RequestedCabinClass = itinerary.RequestedCabinClass,
                CanHold = itinerary.CanHold,
                Currency = itinerary.Price.LocalCurrency,
                TripType = itinerary.TripType,
                OriginalTotalFare = tOri,
                OriginalAdultFare = aOri,
                OriginalChildFare = cOri,
                OriginalInfantFare = iOri,
                NetTotalFare = tNet,
                NetAdultFare = aNet,
                NetChildFare = cNet,
                NetInfantFare = iNet,
                Trips = itinerary.Trips.Select(ConvertToTripForDisplay).ToList(),
                RegisterNumber = itinerary.RegisterNumber
            };
        }

        private static ComboForDisplay ConvertToComboForDisplay(Combo combo)
        {
            if (combo == null)
                return null;
            return new ComboForDisplay
            {
                Fare = combo.Fare,
                Registers = combo.Registers
            };
        }

        private FlightTripForDisplay ConvertToTripForDisplay(FlightTrip trip)
        {
            return new FlightTripForDisplay
            {
                Segments = MapSegments(trip.Segments),
                OriginAirport = trip.OriginAirport,
                OriginCity = GetAirportCity(trip.OriginAirport),
                OriginAirportName = GetAirportName(trip.OriginAirport),
                DestinationAirport = trip.DestinationAirport,
                DestinationCity = GetAirportCity(trip.DestinationAirport),
                DestinationAirportName = GetAirportName(trip.DestinationAirport),
                DepartureDate = trip.DepartureDate,
                TotalDuration = CalculateTotalDuration(trip).TotalMilliseconds,
                Airlines = GetAirlineList(trip),
                TotalTransit = CalculateTotalTransit(trip),
                Transits = MapTransitDetails(trip)
            };
        }

        private List<FlightTripForDisplay> ExtractTripsForDisplay(List<FlightItinerary> itins)
        {
            var allTrips = new List<FlightTripForDisplay>();
            foreach (var itin in itins)
            {
                var trips = itin.Trips.Select(ConvertToTripForDisplay).ToList();
                var cumulativeOri = 0M;
                var cumulativeLocal = 0M;
                for (var i = trips.Count; i > 0; i--)
                {
                    var trip = trips[i - 1];
                    var price = itin.Price;
                    price.OriginalIdr = (itin.Price.OriginalIdr - cumulativeOri) / i;
                    cumulativeOri += itin.Price.OriginalIdr;
                    var unrounded = (price.Local - cumulativeLocal) / i;
                    var rounded = unrounded - unrounded % price.LocalCurrency.RoundingOrder;
                    price.Local = rounded;
                    cumulativeLocal += price.Local;
                    decimal? aOri, cOri, iOri, aNet, cNet, iNet;
                    decimal tOri, tNet;
                    CalculateFare(itin,
                        out tOri, out aOri, out cOri, out iOri, 
                        out tNet, out aNet, out cNet, out iNet);
                    trip.OriginalTotalFare = tOri;
                    trip.OriginalAdultFare = aOri;
                    trip.OriginalChildFare = cOri;
                    trip.OriginalInfantFare = iOri;
                    trip.NetTotalFare = tNet;
                    trip.NetAdultFare = aNet;
                    trip.NetChildFare = cNet;
                    trip.NetInfantFare = iNet;
                }
                allTrips.AddRange(trips);
            }
            return allTrips.OrderBy(trip => trip.Segments[0].DepartureTime).ToList();
        }

        private List<FlightSegmentForDisplay> MapSegments(IEnumerable<FlightSegment> segments)
        {
            return segments.Select(segment => new FlightSegmentForDisplay
            {
                DepartureAirport = segment.DepartureAirport,
                DepartureCity = GetAirportCity(segment.DepartureAirport),
                DepartureAirportName = GetAirportName(segment.DepartureAirport),
                DepartureTime = segment.DepartureTime,
                DepartureTerminal = segment.DepartureTerminal,
                ArrivalAirport = segment.ArrivalAirport,
                ArrivalCity = GetAirportCity(segment.ArrivalAirport),
                ArrivalAirportName = GetAirportName(segment.ArrivalAirport),
                ArrivalTime = segment.ArrivalTime,
                ArrivalTerminal = segment.ArrivalTerminal,
                Duration = segment.Duration.TotalMilliseconds,
                AirlineCode = segment.AirlineCode,
                AirlineName = GetAirlineName(segment.AirlineCode),
                AirlineLogoUrl = GetAirlineLogoUrl(segment.AirlineCode),
                FlightNumber = segment.FlightNumber,
                OperatingAirlineCode = segment.OperatingAirlineCode,
                OperatingAirlineName = GetAirlineName(segment.OperatingAirlineCode),
                OperatingAirlineLogoUrl = GetAirlineLogoUrl(segment.OperatingAirlineCode),
                AircraftCode = segment.AircraftCode,
                StopQuantity = segment.StopQuantity,
                Stops = MapStops(segment.Stops),
                CabinClass = segment.CabinClass,
                Pnr = segment.Pnr,
                Rbd = segment.Rbd,
                IsMealIncluded = segment.IsMealIncluded,
                IsPscIncluded = segment.IsPscIncluded,
                BaggageCapacity = segment.BaggageCapacity,
                IsBaggageIncluded = segment.IsBaggageIncluded,
                RemainingSeats = segment.RemainingSeats != 0 ? (int?)segment.RemainingSeats : null,
            }).ToList();
        }

        private List<FlightStopForDisplay> MapStops(IEnumerable<FlightStop> stops)
        {
            if (stops == null)
                return null;

            return stops.Select(stop => new FlightStopForDisplay
            {
                Airport = stop.Airport,
                ArrivalTime = stop.ArrivalTime,
                DepartureTime = stop.DepartureTime,
                Duration = stop.Duration.Milliseconds
            }).ToList();
        }

        private static TimeSpan CalculateTotalDuration(FlightTrip trip)
        {
            var segments = trip.Segments;
            var totalFlightDuration = new TimeSpan();
            var totalTransitDuration = new TimeSpan();
            for (var i = 0; i < segments.Count; i++)
            {
                totalFlightDuration += segments[i].Duration;
                if (i != 0)
                    totalTransitDuration += segments[i].DepartureTime - segments[i - 1].ArrivalTime;
            }
            return totalFlightDuration + totalTransitDuration;
        }

        private List<Airline> GetAirlineList(FlightTrip trip)
        {
            var segments = trip.Segments;
            var airlineCodes = segments.Select(segment => segment.AirlineCode);
            var airlines = airlineCodes.Distinct().Select(code => new Airline
            {
                Code = code,
                Name = GetAirlineName(code),
                LogoUrl = GetAirlineLogoUrl(code)
            });
            return airlines.ToList();
        }

        private static List<Transit> MapTransitDetails(FlightTrip trip)
        {
            var segments = trip.Segments;
            var result = new List<Transit>();
            for (var i = 0; i < segments.Count; i++)
            {
                if (i != 0)
                {
                    result.Add(new Transit
                    {
                        Airport = segments[i].DepartureAirport,
                        ArrivalTime = segments[i - 1].ArrivalTime,
                        DepartureTime = segments[i].DepartureTime,
                        Duration = (segments[i].DepartureTime - segments[i - 1].ArrivalTime).TotalMilliseconds
                    });
                }
            }
            return result;
        }

        private static int CalculateTotalTransit(FlightTrip trip)
        {
            var segments = trip.Segments;
            var transit = segments.Count() - 1;
            return transit;
        }

        private void CalculateFare(FlightItinerary itin, out decimal tOri, out decimal? aOri, out decimal? cOri, out decimal? iOri, out decimal tNet, out decimal? aNet, out decimal? cNet, out decimal? iNet)
        {
            aOri = cOri = iOri = aNet = cNet = iNet = null;
            tOri = itin.GetApparentOriginalPrice();
            tNet = itin.Price.Local;
            if (itin.AdultPricePortion == 0M)
                return;

            cOri = iOri = cNet = iNet = 0;
            var aSinglePortion = itin.AdultPricePortion / itin.AdultCount;
            var aNetSinglePortion = itin.NetAdultPricePortion / itin.AdultCount;
            aOri = decimal.Round(aSinglePortion * tOri);
            aNet = decimal.Round(aNetSinglePortion * tNet);
            if (itin.ChildCount > 0)
            {
                if (itin.InfantCount > 0)
                {
                    var cSinglePortion = itin.ChildPricePortion / itin.ChildCount;
                    var cNetSinglePortion = itin.NetChildPricePortion / itin.ChildCount;
                    cOri = decimal.Round(cSinglePortion * tOri);
                    cNet = decimal.Round(cNetSinglePortion * tNet);
                }
                else
                {
                    cOri = decimal.Round((decimal) (tOri - aOri * itin.AdultCount) / itin.ChildCount);
                    cNet = decimal.Round((decimal) (tNet - aNet * itin.AdultCount) / itin.ChildCount);
                }
            }
            if (itin.InfantCount > 0)
            {
                iOri = decimal.Round((decimal)(tOri - aOri * itin.AdultCount - cOri * itin.ChildCount) / itin.InfantCount);
                iNet = decimal.Round((decimal)(tNet - aNet * itin.AdultCount - cNet * itin.ChildCount) / itin.InfantCount);
            }
        }

    }
}
