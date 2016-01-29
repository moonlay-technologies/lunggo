using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Dictionary;
using Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Logic
{
    public partial class AutocompleteLogic
    {
        public static AutocompleteHotelLocationsApiResponse GetHotelLocationAutocomplete(string prefix)
        {
            var dict = DictionaryService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var hotelLocationIds = autocomplete.GetHotelLocationIdsAutocomplete(prefix);
            var hotelLocations = hotelLocationIds.Select(id =>
            {
                var hotelLocationDict = dict.HotelLocationDict[id];
                return new HotelLocationApi
                {
                    LocationId = hotelLocationDict.LocationId,
                    LocationName = hotelLocationDict.LocationName,
                    RegionName = hotelLocationDict.RegionName,
                    CountryName = hotelLocationDict.CountryName,
                    Priority = hotelLocationDict.Priority
                };
            }).OrderBy(d => d.Priority);
            return new AutocompleteHotelLocationsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Success.",
                HotelLocations = hotelLocations
            };
        }
    }
}