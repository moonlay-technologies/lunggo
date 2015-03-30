using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    public partial class MystiflyWrapper : IRevalidateFare
    {
        public RevalidateFareResult RevalidateFare(FlightFareItinerary flightFareItinerary)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirRevalidateRQ
                {
                    FareSourceCode = flightFareItinerary.FareId,
                    SessionId = client.SessionId,
                    Target = client.Target,
                    ExtensionData = null
                };
                
                var result = new RevalidateFareResult();
                var retry = 0;
                var done = false;
                while (retry < 3 && !done)
                {
                    done = true;
                    var response = client.AirRevalidate(request);
                    if (!response.Errors.Any())
                    {
                        result = MapResult(response, flightFareItinerary);
                        result.Success = true;
                    }
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERREV002")
                            {
                                result.Errors.Clear();
                                client.CreateSession();
                                request.SessionId = client.SessionId;
                                retry++;
                                done = false;
                                break;
                            }
                            result.Errors.Add(MapError(error));
                            result.Success = false;
                        }
                    }
                }
                return result;
            }
        }

        private static RevalidateFareResult MapResult(AirRevalidateRS response, FlightFareItinerary flightFareItinerary)
        {
            var result = new RevalidateFareResult();
            CheckFareValidity(response, result, flightFareItinerary);
            return result;
        }

        private static void CheckFareValidity(
            AirRevalidateRS response,
            RevalidateFareResult result,
            FlightFareItinerary flightFareItinerary)
        {
            var newFlightItinerary = MapFlightFareItinerary(response.PricedItineraries.FirstOrDefault());
            if (response.IsValid)
            {
                result.IsValid = true;
                result.FlightFareItinerary = newFlightItinerary;
            }
            else
            {
                if (newFlightItinerary == null)
                {
                    result.IsValid = false;
                    result.IsHigherFareAvailable = false;
                }
                else
                {
                    result.IsValid = false;
                    result.IsHigherFareAvailable = true;
                    result.FlightFareItinerary = newFlightItinerary;
                    CabinTypeCheck(flightFareItinerary, result);
                    RBDCheck(flightFareItinerary, result);
                }
            }
        }

        private static void CabinTypeCheck(FlightFareItinerary flightFareItinerary, RevalidateFareResult result)
        {
            for (var i = 0; i < flightFareItinerary.FlightTrips.Count; i++)
            {
                var oldFlightTrips = flightFareItinerary.FlightTrips;
                var newFlightTrips = result.FlightFareItinerary.FlightTrips;
                if (oldFlightTrips[i].CabinClass != newFlightTrips[i].CabinClass)
                {
                    result.IsCabinTypeChanged = true;
                }
            }
        }

        private static void RBDCheck(FlightFareItinerary flightFareItinerary, RevalidateFareResult result)
        {
            for (var i = 0; i < flightFareItinerary.FlightTrips.Count; i++)
            {
                var oldFlightTrips = flightFareItinerary.FlightTrips;
                var newFlightTrips = result.FlightFareItinerary.FlightTrips;
                if (oldFlightTrips[i].RBD != newFlightTrips[i].RBD)
                {
                    result.IsRBDChanged = true;
                }
            }
        }
    }
}
