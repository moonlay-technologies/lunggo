using System.Collections.Generic;
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
        public static HotelAutocompleteApiResponse GetHotelAutocomplete(string prefix, int dest, int zone, int hotelNum)
        {
            var hotel = HotelService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var hotelLocationIds = autocomplete.GetHotelIdsAutocomplete(prefix);
            if (!hotelLocationIds.Any())
            {
                return new HotelAutocompleteApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Count = 0
                };
            }

            var hotelLocations = new List<HotelAutocompleteApi>();
            foreach (var item in hotelLocationIds)
            {
                var hotelDict = hotel._Autocompletes[item];
                var input = new HotelAutocompleteApi();
                input.Name = hotelDict.Name;
                input.Id = hotelDict.Id;
                input.Type = hotelDict.Type.ToString();
                if (hotelDict.Type.ToString() != "Hotel")
                {
                    input.NumOfHotels = 0;
                }
                
                hotelLocations.Add(input);
            }

            var zones =
                hotelLocations.Where(c => c.Type == HotelService.AutocompleteType.Zone.ToString()).Take(zone).ToList();

            var dests = hotelLocations.Where(c => c.Type == HotelService.AutocompleteType.Destination.ToString()).Take(dest).ToList();

            var hotels = hotelLocations.Where(c => c.Type == HotelService.AutocompleteType.Hotel.ToString()).Take(hotelNum).ToList();

            var hotelAutocompleteApis = new List<HotelAutocompleteApi>();
            hotelAutocompleteApis.AddRange(zones);
            hotelAutocompleteApis.AddRange(dests);
            hotelAutocompleteApis.AddRange(hotels);

            return new HotelAutocompleteApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Autocompletes = hotelAutocompleteApis,
                Count = hotelAutocompleteApis.Count
            };
        }
    }
}