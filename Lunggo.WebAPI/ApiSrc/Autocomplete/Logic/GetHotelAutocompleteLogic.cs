using System.Linq;
using System.Net;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Logic
{
    public partial class AutocompleteLogic
    {
        public static HotelAutocompleteApiResponse GetHotelAutocomplete(string prefix)
        {
            var hotel = HotelService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var hotelLocationIds = autocomplete.GetHotelIdsAutocomplete(prefix);
            var hotelLocations = hotelLocationIds.Select(code =>
            {
                var hotelDict = hotel._Autocompletes[code];
                return new HotelAutocompleteApi
                {
                    Name = hotelDict.Name,
                    Id = hotelDict.Id,
                    Type = hotelDict.Type.ToString()
                    //TODO: NUM OF HOTELS
                };
            }).OrderBy(d => d.Name).ToList();
            return new HotelAutocompleteApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Autocompletes = hotelLocations,
                Count = hotelLocations.Count
            };
        }
    }
}