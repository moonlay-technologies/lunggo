using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Services.Description;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Logic
{
    public partial class AutocompleteLogic
    {
        public static HotelAutocompleteApiResponse GetHotelAutocomplete(string prefix, int dest, int zone, int area, int hotelNum)
        {
            var hotel = HotelService.GetInstance();
            if(!IsValidRequest(prefix, dest,zone, area, hotelNum))
                return new HotelAutocompleteApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Count = 0
                };
            var hotelLocations = new List<HotelAutocompleteApi>();
            TiketHotelWrapper tiket = new TiketHotelWrapper();
            var response = tiket.SearchAutoComplete(prefix);
            if (response == null)
            {
                return new HotelAutocompleteApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Count = 0
                };
            }

            foreach (var result in response)
            {
                result.CountLocation = result.CountLocation.Replace("Hotel", "").Trim();
                var numHotel = int.Parse(result.CountLocation);
                var singleLocation = new HotelAutocompleteApi
                {
                    Id = result.Value,//result.Id,
                    Name = result.LabelLocation,
                    Country = result.CountryId,
                    Type = result.Category,
                    NumOfHotels = numHotel,
                    Value = result.Value
                };
                hotelLocations.Add(singleLocation);
            }

            return new HotelAutocompleteApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Autocompletes = hotelLocations,
                Count = hotelLocations.Count
            };
        }

        public static bool IsValidRequest(string prefix, int dest, int zone, int area, int hotelNum)
        {
            return (!string.IsNullOrEmpty(prefix)) ||
                   zone > 0 || area > 0 || hotelNum > 0;

        }


        public static HotelAutocompleteApiResponse GetHotelAutocompleteTemp(string prefix, int dest, int zone, int area, int hotelNum)
        {
            var hotel = HotelService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var hotelLocationIds = autocomplete.GetHotelIdsAutocomplete(prefix).ToList();
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
                var hotelDict = HotelService.AutoCompletes.First(h => h.Id == item);
                var input = new HotelAutocompleteApi();
                switch (hotelDict.Type)
                {
                    case 1:
                        input.Name = hotelDict.Destination + ", " + hotelDict.Country;
                        input.NumOfHotels = hotelDict.HotelCount;
                        input.Destination = hotelDict.Destination;
                        input.Country = hotelDict.Country;
                        break;
                    case 2:
                        input.Name = hotelDict.Zone + ", " + hotelDict.Destination + ", " + hotelDict.Country;
                        input.NumOfHotels = hotelDict.HotelCount;
                        input.Zone = hotelDict.Zone;
                        input.Destination = hotelDict.Destination;
                        input.Country = hotelDict.Country;
                        break;
                    case 3:
                        input.Name = hotelDict.Area + ", " + hotelDict.Destination + ", " + hotelDict.Country;
                        input.NumOfHotels = hotelDict.HotelCount;
                        input.Area = hotelDict.Area;
                        input.Zone = hotelDict.Zone;
                        input.Destination = hotelDict.Destination;
                        input.Country = hotelDict.Country;
                        break;
                    case 4:
                        input.Name = hotelDict.HotelName + ", " + hotelDict.Destination + ", " + hotelDict.Country;
                        input.Country = hotelDict.Country;
                        input.Destination = hotelDict.Destination;
                        input.NumOfHotels = 0;
                        break;
                }

                input.Id = hotelDict.Id;
                input.Type = AutocompleteTypeCd.Mnemonic(hotelDict.Type).ToString();
                hotelLocations.Add(input);
            }

            var zones =
                hotelLocations.Where(c => c.Type == "Zone").Take(zone).ToList().OrderByDescending(c => c.NumOfHotels).ToList();
            var areas =
                hotelLocations.Where(c => c.Type == "Area").Take(area).ToList().OrderByDescending(c => c.NumOfHotels).ToList();

            var dests = hotelLocations.Where(c => c.Type == "Destination").Take(dest).ToList().OrderByDescending(c => c.NumOfHotels).ToList();

            var hotels = hotelLocations.Where(c => c.Type == "Hotel").Take(hotelNum).ToList();

            var hotelAutocompleteApis = new List<HotelAutocompleteApi>();

            hotelAutocompleteApis.AddRange(dests);
            hotelAutocompleteApis.AddRange(zones);
            hotelAutocompleteApis.AddRange(areas);
            //hotelAutocompleteApis.AddRange(hotels);

            return new HotelAutocompleteApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Autocompletes = hotelAutocompleteApis,
                Count = hotelAutocompleteApis.Count
            };
        }
    }
}