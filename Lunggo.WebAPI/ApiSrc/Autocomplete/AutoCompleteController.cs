using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
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
        public ApiResponseBase HotelLocations(string prefix)
        {
            try
            {
                return AutocompleteLogic.GetHotelAutocomplete(prefix);
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
