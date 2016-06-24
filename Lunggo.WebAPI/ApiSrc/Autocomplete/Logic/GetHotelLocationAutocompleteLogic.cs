using System.Linq;
using System.Net;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Logic
{
    public partial class AutocompleteLogic
    {
        public static AutocompleteHotelLocationsApiResponse GetHotelLocationAutocomplete(string prefix)
        {
            var flight = FlightService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var hotelLocationIds = autocomplete.GetHotelLocationIdsAutocomplete(prefix);
            var hotelLocations = hotelLocationIds.Select(id =>
            {
                var hotelLocationDict = flight.HotelLocationDict[id];
                return new HotelLocationApi
                {
                    LocationId = hotelLocationDict.LocationId,
                    LocationName = hotelLocationDict.LocationName,
                    RegionName = hotelLocationDict.RegionName,
                    CountryName = hotelLocationDict.CountryName,
                    Priority = hotelLocationDict.Priority
                };
            }).OrderBy(d => d.Priority).ToList();
            return new AutocompleteHotelLocationsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                HotelLocations = hotelLocations,
                Count = hotelLocations.Count
            };
        }
    }
}