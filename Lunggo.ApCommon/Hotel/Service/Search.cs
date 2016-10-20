using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CsQuery.Engine.PseudoClassSelectors;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Documents;
using Lunggo.Framework.SharedModel;

namespace Lunggo.ApCommon.Hotel.Service
{

    public partial class HotelService
    {
        public SearchHotelOutput Search(SearchHotelInput input)
        {
            bool isByDestination = false;
            if (input.SearchId != null)
            {
                //Take search data from Redis
                var searchResult = GetSearchHotelResultFromCache(input.SearchId);

                var hotels = searchResult.HotelDetails;
                
                //Filtering
                if (hotels != null && input.FilterParam != null)
                {
                    var listStar = GetStarFilter(input.FilterParam.StarFilter);
                    
                    hotels = searchResult.HotelDetails.Where(p =>
                    (input.FilterParam.AreaFilter == null || input.FilterParam.AreaFilter.Areas.Contains(p.ZoneCode)) &&
                    (listStar == null || listStar.Contains(p.StarRating)) &&
                    (input.FilterParam.PriceFilter == null|| (p.OriginalFare >= input.FilterParam.PriceFilter.MinPrice && p.OriginalFare <= input.FilterParam.PriceFilter.MaxPrice))
                    ).Select(p => new HotelDetail
                    {
                        HotelCode = p.HotelCode,
                        HotelName = p.HotelName,
                        Review = p.Review,
                        Address = p.Address,
                        PostalCode = p.PostalCode,
                        Chain = p.Chain,
                        Pois = p.Pois,
                        StarRating = p.StarRating,
                        Terminals = p.Terminals,
                        Latitude = p.Latitude,
                        Longitude = p.Longitude,
                        PhonesNumbers = p.PhonesNumbers,
                        OriginalFare = p.OriginalFare,
                        ImageUrl = p.ImageUrl,
                        ZoneCode = p.ZoneCode,
                        SpecialRequest = p.SpecialRequest,
                        Email = p.Email,
                        Facilities = p.Facilities,
                        Segment = p.Segment,
                        Description = p.Description,
                        City = p.City,
                        CountryCode = p.CountryCode,
                        NightCount = p.NightCount,
                        Rooms = p.Rooms,
                        NetFare = p.NetFare,
                        DestinationCode = p.DestinationCode,
                        AccomodationType = p.AccomodationType,
                        Discount = p.Discount,
                    }).ToList();    
                }
                

                //Sorting
                if (hotels != null && input.SortingParam != null)
                {
                    if (input.SortingParam.LowestPrice)
                    {
                        hotels = hotels.OrderBy(p => p.OriginalFare).ToList();
                    }

                    if (input.SortingParam.HighestPrice)
                    {
                        hotels = hotels.OrderByDescending(p => p.OriginalFare).ToList();
                    }
                }

                List<HotelDetail> hotelList;
                if (input.StartPage != 0 && input.EndPage != 0)
                {
                    hotelList = hotels.Skip(input.StartPage).Take(input.EndPage).ToList();
                }
                else
                {
                    hotelList = hotels.Take(100).ToList();
                }

                hotelList = AddHotelDetail(hotelList);

                return new SearchHotelOutput
                {
                    SearchId = searchResult.SearchId,
                    HotelDetailLists = ConvertToHotelDetailForDisplay(hotelList),
                    StartPage = input.StartPage,
                    EndPage = input.EndPage,
                    TotalDisplayHotel = hotelList.Count,
                    TotalActualHotel = searchResult.HotelDetails.Count,
                    HotelFilterDisplayInfo = searchResult.HotelFilterDisplayInfo
                    
                };
            }
            else
            {
                //Save data to DocDB
                var hotelBedsClient = new HotelBedsSearchHotel();
                var detailDestination = GetLocationById(input.Location);
                var request = new SearchHotelCondition
                {
                    CheckIn = input.CheckIn,
                    Checkout = input.Checkout,
                    HotelCode = input.HotelCode,
                    AdultCount = input.AdultCount,
                    ChildCount = input.ChildCount,
                    Nights = input.Nights,
                    Rooms = input.Rooms
                };
                switch (detailDestination.Type)
                {
                    case AutocompleteType.Zone :
                        var splittedZone = detailDestination.Code.Split('-');
                        request.Zone = int.Parse(splittedZone[1].Trim());
                        request.Destination = splittedZone[0].Trim();
                        break;
                    case AutocompleteType.Destination:
                        request.Destination = detailDestination.Code;
                        isByDestination = true;
                        break;
                };

                var result = hotelBedsClient.SearchHotel(request);

                //remember to add searchId
                Guid generatedSearchId = Guid.NewGuid();
                result.SearchId = generatedSearchId.ToString();
                Debug.Print("Search Id : "+ result.SearchId);

                if (result.HotelDetails != null)
                {
                    //TODO: MARGIN
                    result.HotelDetails.ForEach(h => h.Rooms.ForEach(r => r.Rates.ForEach(t =>
                    {
                        t.Price.LocalCurrency = new Currency("IDR");
                        t.Price.SetMargin(new Margin
                        {
                            Constant = 0,
                            Currency = new Currency("IDR"),
                            Description = "HOTELBEDS",
                            Id = 10,
                            IsActive = false,
                            IsFlat = false,
                            Name = "htbd",
                            Percentage = 5,
                            RuleId = 3
                        });
                    }
                        )));


                    if (isByDestination)
                    {
                        result.HotelFilterDisplayInfo = SetHotelFilterDisplayInfo(result.HotelDetails);
                    }

                    SaveSearchResultintoDatabaseToCache(result.SearchId, result);

                    var firstPageHotelDetails = result.HotelDetails.Take(100).ToList(); 
                    //Add HotelDetail Here
                    firstPageHotelDetails = AddHotelDetail(firstPageHotelDetails);

                    //return only 100 data for the first page
                    return new SearchHotelOutput
                    {
                        SearchId = result.SearchId,
                        HotelDetailLists = ConvertToHotelDetailForDisplay(firstPageHotelDetails),
                        StartPage = 1,
                        EndPage = 100,
                        TotalDisplayHotel = firstPageHotelDetails.Count,
                        TotalActualHotel = result.HotelDetails.Count,
                        HotelFilterDisplayInfo = result.HotelFilterDisplayInfo
                    };
                }
                else
                {
                    Console.WriteLine("Search result is empty");
                    return new SearchHotelOutput();
                }
            }
        }

        public List<HotelDetail> AddHotelDetail(List<HotelDetail> result)
        {
            //Adding Additional Hotel Information
                foreach (var hotel in result)
                {
                   var detail = GetHotelDetailFromDb(hotel.HotelCode);
                    hotel.Address = detail.Address;
                    hotel.City = detail.City;
                    hotel.Chain = detail.Chain;
                    hotel.AccomodationType = detail.AccomodationType;
                    //facilities hotel;
                    hotel.Review = detail.Review;
                    hotel.ImageUrl = detail.ImageUrl;
                }
            return result;
        }

        public HotelFilterDisplayInfo SetHotelFilterDisplayInfo(List<HotelDetail> hotels)
        {
            var filter = new HotelFilterDisplayInfo();
            var zoneDict = new Dictionary<int, ZoneFilter>();
            try
            {
                foreach (var hotelDetail in hotels)
                {
                    if (!(zoneDict.ContainsKey(hotelDetail.ZoneCode)))
                    {
                        zoneDict.Add(hotelDetail.ZoneCode, new ZoneFilter
                        {
                            Code = hotelDetail.ZoneCode,
                            Count = 1,
                            Name = GetHotelZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode)
                        });
                    }
                    else
                    {
                        zoneDict[hotelDetail.ZoneCode].Count += 1;
                    }
                }
                filter.ZoneFilter = new List<ZoneFilter>();

                foreach (var zone in zoneDict.Keys)
                {
                    filter.ZoneFilter.Add(new ZoneFilter
                    {
                        Code = zoneDict[zone].Code,
                        Count = zoneDict[zone].Count,
                        Name = zoneDict[zone].Name,
                    });
                }
            }
            catch(Exception e)
            {
                Debug.Print(e.Message);
            }
            
            return filter;
        }

        public List<string> GetStarFilter(List<bool> starFilter)
        {
            if (starFilter != null)
            {
                int count = 0;
                var completedList = new List<string> { "1EST", "2EST", "3EST", "4EST", "5EST" };
                var listStar = new List<string>();
                foreach (var star in starFilter)
                {
                    if (star)
                    {
                        listStar.Add(completedList[count]);
                    }
                    count++;
                }
                return listStar;
            }
            else
            {
                return null;
            }
            
        } 
    }
}
