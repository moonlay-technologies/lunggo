using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Logic;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete
{
    public class AutocompleteController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/autocomplete/airlines/{prefix}")]
        public ApiResponseBase Airlines(string prefix)
        {
            try
            {
                return AutocompleteLogic.GetAirlineAutocomplete(prefix);
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/autocomplete/airports")]
        [Route("v1/autocomplete/airports/{prefix}")]
        public ApiResponseBase Airports(string prefix = null)
        {
            try
            {
                return AutocompleteLogic.GetAirportAutocomplete(prefix);
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/hotel/{prefix}")]
        [Route("v1/autocomplete/hotel/{prefix}/{dest}/{zone}/{area}/{hotel}")]
        public ApiResponseBase HotelLocations(string prefix, int dest = 10, int zone = 10,int area=10, int hotel = 10)
        {
            try
            {
                return AutocompleteLogic.GetHotelAutocomplete(prefix, dest, zone,area, hotel);
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
