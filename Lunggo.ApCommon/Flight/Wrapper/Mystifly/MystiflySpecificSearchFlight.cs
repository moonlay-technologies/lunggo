using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal SearchFlightResult SpecificSearchFlight(SearchFlightConditions conditions)
        {
            var request = new IntelliBestBuyRQ
            {
                IntelliFareInformations = MapItineraryInformations(conditions.Trips.SelectMany(trip => trip.Segments)),
                CabinPreference = MapCabinType(conditions.CabinClass),
                BookingClassPreference = BookingClassPreference.Any,
                PassengerTypeQuantities = MapPassengerTypes(conditions),
                SessionId = Client.SessionId,
                Target = Client.Target,
                ExtensionData = null
            };

            var result = new SearchFlightResult();
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.IntelliBestBuy(request);
                done = true;
                if (!response.Errors.Any())
                {
                    result = MapResult(response, conditions);
                    result.IsSuccess = true;
                    result.Errors = null;
                    result.ErrorMessages = null;
                }
                else
                {
                    result.Errors = new List<FlightError>();
                    result.ErrorMessages = new List<string>();
                    if (response.Errors.Length <= 3 &&
                        (response.Errors.First().Code == "ERIFS012" ||
                        response.Errors.First().Code == "ERIFS013" ||
                        response.Errors.First().Code == "ERIFS014"))
                    {
                        result.Errors = null;
                        result.ErrorMessages = null;
                        result.IsSuccess = true;
                        result.Itineraries = null;
                    }
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERIFS001" || error.Code == "ERIFS002")
                            {
                                Client.CreateSession();
                                request.SessionId = Client.SessionId;
                                retry++;
                                if (retry <= 3)
                                {
                                    done = false;
                                    break;
                                }
                            }
                            result.IsSuccess = false;
                        }
                        if (done)
                            MapError(response, result);
                    }
                }

            }
            return result;
        }

        private static CabinType MapCabinType(Flight.Constant.CabinClass cabinClass)
        {
            switch (cabinClass)
            {
                case Flight.Constant.CabinClass.Economy :
                    return CabinType.Y;
                case Flight.Constant.CabinClass.Business :
                    return CabinType.C;
                case Flight.Constant.CabinClass.First :
                    return CabinType.F;
                default :
                    return CabinType.Default;
            }
        }

        private static IntelliBestBuyInformation[] MapItineraryInformations(IEnumerable<FlightSegment> source)
        {
            return source.Select(item => new IntelliBestBuyInformation
            {
                OriginLocationCode = item.DepartureAirport,
                DestinationLocationCode = item.ArrivalAirport,
                DepartureDateTime = DateTime.SpecifyKind(item.DepartureTime,DateTimeKind.Utc),
                ArrivalDateTime = DateTime.SpecifyKind(item.ArrivalTime,DateTimeKind.Utc),
                AirlineCode = item.AirlineCode,
                FlightNumber = item.FlightNumber,
                BookingClass = item.Rbd,
                ExtensionData = null
            }).ToArray();
        }
    }
}
