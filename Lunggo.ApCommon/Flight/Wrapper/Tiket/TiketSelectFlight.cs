using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override bool SelectFlight(FlightItinerary itin)
        {
            return Client.SelectFlight(itin);
        }

        private partial class TiketClientHandler
        {
            internal bool SelectFlight(FlightItinerary itin)
            {
                var flightId = itin.FareId;
                var date = itin.Trips[0].DepartureDate;

                GetToken();
                var client = CreateTiketClient();
                var url = "/flight_api/get_flight_data?flight_id=" + flightId + "&token=" + _token + "&date=" + date.ToString("yyyy-MM-dd") + "&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var flightData = response.Content.Deserialize<GetFlightDataResponse>();
                if (flightData.Diagnostic.Status != "200")
                    return false;
                
                if (flightData.Required != null)
                {
                    var requireData = flightData.Required;
                    if (requireData.Birthdatea1 != null && requireData.Birthdatea1.Mandatory == 1)
                    {
                        itin.RequireBirthDate = true;
                    }
                    if (requireData.Passportnoa1 != null && requireData.Passportnoa1.Mandatory == 1)
                    {
                        itin.RequirePassport = true;
                        if (requireData.Passportissueddatea1 != null && requireData.Passportissueddatea1.Mandatory == 1)
                        {
                            itin.RequireIssueDatePassport = true;
                        }
                    }
                    else if (requireData.Passportnationalitya1 != null && requireData.Passportnationalitya1.Mandatory == 1)
                    {
                        itin.RequireNationality = true;
                    }

                    if (requireData.DeptCheckinBaggage != null)
                    {
                        if (requireData.DeptCheckinBaggage.Resource != null &&
                            requireData.DeptCheckinBaggage.Resource.Count != 0)
                        {
                            var capacityData =
                                requireData.DeptCheckinBaggage.Resource.FirstOrDefault(x => x.Name.Contains("(+ IDR 0,00)"));
                            if (capacityData != null)
                            {
                                itin.Trips[0].Segments[0].BaggageCapacity = capacityData.Id;
                            }

                        }
                    }
                    itin.Token = flightData.Token;
                }
                return true;
            }
        }
    }

}
