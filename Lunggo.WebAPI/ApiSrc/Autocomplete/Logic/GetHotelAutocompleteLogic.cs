using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Services.Description;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Hotel.Constant;
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
                var hotelDict = hotel.AutoCompletes.First(h => h.Id == item);
                var input = new HotelAutocompleteApi();
                switch (hotelDict.Type)
                {
                    case 1:
                        input.Name = hotelDict.Destination + ", " + hotelDict.Country;
                        input.NumOfHotels = hotelDict.HotelCount;
                        break;
                    case 2:
                        input.Name = hotelDict.Zone + ", " + hotelDict.Destination + ", " + hotelDict.Country;
                        input.NumOfHotels = hotelDict.HotelCount;
                        break;
                    case 4:
                        input.Name = hotelDict.HotelName + ", " + hotelDict.Destination + ", " + hotelDict.Country;
                        input.NumOfHotels = 0;
                        break;
                }
                
                input.Id = hotelDict.Id;
                input.Type = AutocompleteTypeCd.Mnemonic(hotelDict.Type).ToString();
                hotelLocations.Add(input);
            }

            var zones =
                hotelLocations.Where(c => c.Type == "Zone").Take(zone).ToList();

            var dests = hotelLocations.Where(c => c.Type == "Destination").Take(dest).ToList();

            var hotels = hotelLocations.Where(c => c.Type == "Hotel").Take(hotelNum).ToList();

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