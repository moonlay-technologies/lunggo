using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override SearchFlightResult SpecificSearchFlight(SpecificSearchConditions conditions)
        {
            var request = new IntelliBestBuyRQ
            {
                IntelliFareInformations = MapItineraryInformations(conditions.FlightSegments),
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
                        result.FlightItineraries = null;
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

        private static PassengerTypeQuantity[] MapPassengerTypes(SpecificSearchConditions conditions)
        {
            var passengerTypeQuantities = new List<PassengerTypeQuantity>
            {
                new PassengerTypeQuantity()
                {
                    Code = PassengerType.ADT,
                    Quantity = conditions.AdultCount
                }
            };
            if (conditions.ChildCount > 0)
                passengerTypeQuantities.Add(
                    new PassengerTypeQuantity()
                    {
                        Code = PassengerType.CHD,
                        Quantity = conditions.ChildCount
                    });
            if (conditions.InfantCount > 0)
                passengerTypeQuantities.Add(
                    new PassengerTypeQuantity()
                    {
                        Code = PassengerType.INF,
                        Quantity = conditions.InfantCount
                    });
            return passengerTypeQuantities.ToArray();
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
                DepartureDateTime = item.DepartureTime.ToUniversalTime(),
                ArrivalDateTime = item.ArrivalTime.ToUniversalTime(),
                AirlineCode = item.AirlineCode,
                FlightNumber = item.FlightNumber,
                BookingClass = item.Rbd,
                ExtensionData = null
            }).ToArray();
        }
    }
}
