using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.GoogleMaps.Geocoding
{
    public class GeocodingClient
    {
        private readonly string apiKey;
        private readonly string basePath;
        private readonly string endpointTemplate;
        private readonly string completeUrl;

        public GeocodingClient() { }
        public GeocodingClient(string apiKey)
        {
            this.apiKey = apiKey;
            basePath = "https://maps.googleapis.com/maps/api/geocode/";
            endpointTemplate = "json?latlng=${lat},${lng}&key=${apiKey}";
        }

        public string GetEndpoint(decimal? latitude, decimal? longitude, string liveKey)
        {
            return endpointTemplate.Replace("${lat}", latitude.ToString()).Replace("${lng}", longitude.ToString()).Replace("${apiKey}", liveKey);
        }

        public List<string> GetLocationByGeoCode(decimal? latitude, decimal? longitude)
        {
            if (latitude == null || longitude == null || latitude == 0 || longitude == 0)
                return null;
            var client = new RestClient(basePath);
            var geoLocationList= new List<string>();
            RestRequest request = new RestRequest(GetEndpoint(latitude, longitude, apiKey), Method.GET);
            IRestResponse<GeocodeResponse> response = client.Execute<GeocodeResponse>(request);
            var content = response.Content;
            var geoLocationResponse = content.Deserialize<GeocodeResponse>();
            if (geoLocationResponse.status.Equals("OVER_QUERY_LIMIT"))
            {
                geoLocationList.Add("OVERLIMIT");
                return geoLocationList;
            }
            
            if (geoLocationResponse.results.Count == 0)
                return null;
            var zone = geoLocationResponse.results[0].address_components.Where(
                x => x.types.Contains("administrative_area_level_2")).Select(x => x.long_name).FirstOrDefault();
            var area = geoLocationResponse.results[0].address_components.Where(
                x => x.types.Contains("administrative_area_level_3")).Select(x => x.long_name).FirstOrDefault();
            geoLocationList.Add(zone);
            geoLocationList.Add(area);
            Thread.Sleep(1000);
            return geoLocationList;
        }

    }
}
