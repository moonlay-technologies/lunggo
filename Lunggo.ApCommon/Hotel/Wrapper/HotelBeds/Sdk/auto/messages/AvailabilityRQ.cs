using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{

    public class AvailabilityRQ : AbstractGenericRequest
    {
        public Stay stay { get; set; }
        public List<Occupancy> occupancies { get; set; }
        public GeoLocation geolocation { get;  set; }
        public Destination destination { get; set; }
        public KeywordsFilter keywords { get; set; }        
        public HotelsFilter hotels { get; set; }
        
        public List<ReviewFilter> reviews { get; set; }
        public Filter filter { get; set; }
        public Boards boards { get; set; }
        public Rooms rooms { get; set; }
        public bool dailyRate { get; set; }
        public Source source { get; set; }
        public string sourceMarket { get; set; }
        [JsonProperty("accommodations", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.AccommodationTypeConverter))]
        public List<SimpleTypes.AccommodationType> accommodations { get; set; }

        public void Validate()
        {
            if (stay == null || String.IsNullOrEmpty(stay.checkIn) || String.IsNullOrEmpty(stay.checkOut))
                throw new ArgumentException("Stay object can't be null");

            if (occupancies == null || occupancies.Count == 0)
                throw new ArgumentException("Occupancies List object can't be null");

            for(int i = 0; i < occupancies.Count; i++)
            {
                if ( !occupancies[i].rooms.HasValue || occupancies[i].rooms.Value < 1)
                    throw new ArgumentException("Rooms must be 1 or greater");
                if ( !occupancies[i].adults.HasValue || occupancies[i].adults.Value < 1)
                    throw new ArgumentException("Adults must be 1 or greater");

                if (occupancies[i].children.HasValue)
                {
                    if (occupancies[i].paxes == null || occupancies[i].paxes.Count == 0)
                        throw new ArgumentException("Pax list can't be null with children's availability");

                    for (int p = 0; p < occupancies[i].paxes.Count; p++)
                    {
                        if( !occupancies[i].paxes[p].age.HasValue )
                            throw new ArgumentException("Age passenger must be 0 or greater");
                    }                    
                }                
            }

            if ( destination != null )
            {
                if (String.IsNullOrEmpty(destination.code) || !destination.zone.HasValue)
                    throw new ArgumentException("If destination object is informed then code and zone must be informed too.");
            }

            if( geolocation != null )
            {
                if (!geolocation.longitude.HasValue || !geolocation.latitude.HasValue || !geolocation.radius.HasValue)
                    throw new ArgumentException("If geolocation object is informed then longitude and latitude and radius must be informed too.");
            }

            if (hotels != null)
            {
                if (hotels.hotel != null && hotels.hotel.Count == 0)
                    throw new ArgumentException("If hotelsFilter object is informed then internal hotel code must be informed too.");
            }

            if(boards != null)
            {
                if(boards.board == null || boards.board.Count == 0)
                    throw new ArgumentException("If boards object is informed then internal board list code must be informed too.");

                for(int b = 0; b < boards.board.Count; b++)
                {
                    if(String.IsNullOrEmpty(boards.board[b]))
                        throw new ArgumentException("If boards object is informed then internal board list code must be informed too.");
                }
            }
        }
    }
}
