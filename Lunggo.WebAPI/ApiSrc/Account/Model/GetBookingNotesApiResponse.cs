using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetBookingNotesApiResponse : ApiResponseBase
    {
        [JsonProperty("bookingNotes")]
        public List<BookingNotes> BookingNotes { get; set; }       
    }

    public class BookingNotes
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}