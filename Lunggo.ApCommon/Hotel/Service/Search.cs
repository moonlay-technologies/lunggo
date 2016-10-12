using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.Framework.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{

    public partial class HotelService
    {
        public SearchHotelOutput Search(SearchHotelInput input)
        {
            if (input.SearchId != null)
            {
                //Take data from SearchResult
                var searchResult = GetAllSearchHotelResultFromDocument(input.SearchId);
                //do return
                List<HotelDetail> hotelList;
                if (input.StartPage != 0 && input.EndPage != 0)
                {
                    hotelList = searchResult.HotelDetails.Skip(input.StartPage).Take(input.EndPage).ToList();
                }
                else
                {
                    hotelList = searchResult.HotelDetails.Take(100).ToList();
                }
                return new SearchHotelOutput
                {
                    SearchId = searchResult.SearchId,
                    HotelDetailLists = ConvertToHotelDetailForDisplay(hotelList)
                };
            }
            else
            {
                //Do Call Availability
                //Save data to DocDB
                var hotelBedsClient = new HotelBedsSearchHotel();
                var result = hotelBedsClient.SearchHotel(new SearchHotelCondition
                {
                    CheckIn = input.CheckIn,
                    Checkout = input.Checkout,
                    Location = input.Location,
                    Zone = input.Zone,
                    AdultCount = input.AdultCount,
                    ChildCount = input.ChildCount,
                    Nights = input.Nights,
                    Rooms = input.Rooms
                });

                //remember to add searchId
                Guid generatedSearchId = Guid.NewGuid();
                result.SearchId = generatedSearchId.ToString();
                Debug.Print("Search Id : "+ result.SearchId);

                if (result.HotelDetails != null)
                {
                    //save data to docDB
                    SaveSearchResultToDocument(result);
                    //return only 100 data for the first page
                    return new SearchHotelOutput
                    {
                        SearchId = result.SearchId,
                        HotelDetailLists = ConvertToHotelDetailForDisplay(result.HotelDetails).Take(100).ToList(),
                        StartPage = 1,
                        EndPage = 100,
                    };
                }
                else
                {
                    Console.WriteLine("Search result is empty");
                    return new SearchHotelOutput();
                }
            }
        }
        
    }
}
