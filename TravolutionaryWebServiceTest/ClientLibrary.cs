using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravolutionaryWebServiceTest.Travolutionary.WebService;

namespace TravolutionaryWebServiceTest
{
    class ClientLibrary
    {
        public static HotelsServiceSearchRequest CreateMockHotelsSearchRequest(SeedHotelsSearchRequest request)
        {
            var searchRqst = new HotelsServiceSearchRequest()
            {
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                ClientIP = null,
                //ContractIds = new int[] { 5 }, //Search by contract id with hotel supplier. (Can be retrieved from admin panel: Admin>Contracts)
                DesiredResultCurrency = request.ResultCurrency, //Currency ISO (Example: "USD", "EUR", "ILS")
                DetailLevel = ToTravolutionaryHotelSearchDetail(request.SearchDetail),
                /**
                    Default - includes all the details. Not Recommended because of Large package size 
                    Low - includes supplier name on the package
                    Minimal - includes only rooms types and prices
                    NoPackages - doesn't return any package information, only the lowest price.
                    Meta - doesn't return any package information, only lowest prices from each supplier. Recommended way!
                **/
                ExcludeHotelDetails = false,
                //HotelIds = new int[] { }, //*Mandatory*. Search by hotel id. Limited to 150 id's. Can be downloaded
                HotelLocation = request.Location, //*Mandatory*. Search by hotel location. Can be downloaded here.
                IncludeCityTax = false,
                /*
                    False - default value.
                    True - might be useful for some USA cities.
                */
                Nights = request.Nights, //Mandatory. Number of nights for hotel stay (Example: 2). Cannot be “0” or “NULL”!
                Residency = request.Residency, //Mandatory. Lead pax residency, ISO Country Code (Example: US, CZ, IL)
                //ResponseLanguage = null, // No description in API documentation
                Rooms = new HotelRoomRequest[]
                    {
                        new HotelRoomRequest
                        {
                            AdultsCount  = 2, //Mandatory. Number of adults to stay in room. Up to 4 in a room. Cannot be “0” or “NULL”!
                            //KidsAges = new int[] {8,8}, //Up to 2 in a room.
                            SeperatedBeds = false
                            /*
                                False - default value. No separated beds in the room.
                                True - Separated beds.
                            */
                        }
                    },
        
                //SupplierIds = new int[] { } //For search from specific suppliers.
            };
            return searchRqst;
        }

        private static SearchDetailLevel ToTravolutionaryHotelSearchDetail(HotelSearchDetailLevel detail)
        {
            if (detail == HotelSearchDetailLevel.Default)
            {
                return SearchDetailLevel.Default;
            }
            else if (detail == HotelSearchDetailLevel.Low)
            {
                return SearchDetailLevel.Low;
            }
            else if (detail == HotelSearchDetailLevel.Meta)
            {
                return SearchDetailLevel.Meta;
            }
            else if (detail == HotelSearchDetailLevel.Minimal)
            {
                return SearchDetailLevel.Minimal;
            }
            else if (detail == HotelSearchDetailLevel.NoPackages)
            {
                return SearchDetailLevel.NoPackages;
            }
            else
            {
                throw new Exception("Invald value for HotelSearchDetailLevel parameter");
            }
        }
    }

    class SeedHotelsSearchRequest
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public HotelSearchDetailLevel SearchDetail { get; set; }
        public String ResultCurrency { get; set; }
        public int Location { get; set; }
        public String Residency { get; set; }
        public int Nights { get; set; }
    }

    enum HotelSearchDetailLevel
    {
        Meta,
        Default,
        Low,
        Minimal,
        NoPackages
    }
}
