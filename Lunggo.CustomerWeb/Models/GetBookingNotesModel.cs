using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.CustomerWeb.Models
{
    public class GetBookingNotesModel
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