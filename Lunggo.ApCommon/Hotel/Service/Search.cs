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
                    (input.FilterParam.AccommodationTypeFilter == null || input.FilterParam.AccommodationTypeFilter.Accomodations.Contains(p.AccomodationType)) &&
                    (listStar == null || listStar.Contains(p.StarCode)) &&
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
                        StarCode = p.StarCode,
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
                var allCurrency = Currency.GetAllCurrencies();
                Guid generatedSearchId = Guid.NewGuid();
                SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency );

                //Do Call Availability
                //Save data to DocDB
                var hotelBedsClient = new HotelBedsSearchHotel();
                var request = new SearchHotelCondition();
                if (input.HotelCode != 0)
                {
                    if (input.Occupancies == null)
                    {
                        request.AdultCount = input.AdultCount;
                        request.Rooms = input.Rooms;
                    }
                    else
                    {
                        request.Occupancies = input.Occupancies;
                    }
                    request.HotelCode = input.HotelCode;
                    request.CheckIn = input.CheckIn;
                    request.Checkout = input.Checkout;
                    request.SearchId = generatedSearchId.ToString();
                }
                else
                {
                    var detailDestination = GetLocationById(input.Location);
                    if (input.Occupancies == null)
                    {
                        request = new SearchHotelCondition
                        {
                            CheckIn = input.CheckIn,
                            Checkout = input.Checkout,
                            AdultCount = input.AdultCount,
                            ChildCount = input.ChildCount,
                            Nights = input.Nights,
                            Rooms = input.Rooms,
                            SearchId = generatedSearchId.ToString()
                        };
                    }
                    else
                    {
                        request = new SearchHotelCondition
                        {
                            CheckIn = input.CheckIn,
                            Checkout = input.Checkout,
                            Nights = input.Nights,
                            Occupancies = input.Occupancies,
                            SearchId = generatedSearchId.ToString()
                        };
                    }
                    
                    switch (detailDestination.Type)
                    {
                        case AutocompleteType.Zone:
                            request.Zone = detailDestination.Code;
                            break;
                        case AutocompleteType.Destination:
                            request.Destination = detailDestination.Code;
                            isByDestination = true;
                            break;
                        case AutocompleteType.Area:
                            request.Area = detailDestination.Code;
                            break;
                        case AutocompleteType.Hotel:
                            request.HotelCode = int.Parse(detailDestination.Code);
                            break;
                    };
                }
                

                var result = hotelBedsClient.SearchHotel(request);
                result.SearchId = generatedSearchId.ToString();
                Debug.Print("Search Id : " + result.SearchId);
                //remember to add searchId


                if (result.HotelDetails != null)
                {
                    AddPriceMargin(result.HotelDetails);
                    result.HotelDetails = AddFilteringInfo(result.HotelDetails);
                    result.HotelFilterDisplayInfo = SetHotelFilterDisplayInfo(result.HotelDetails, isByDestination);
              
                    SaveSearchResultintoDatabaseToCache(result.SearchId, result);

                    var firstPageHotelDetails = result.HotelDetails.Take(100).ToList(); 
                    firstPageHotelDetails = AddHotelDetail(firstPageHotelDetails);

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
                    hotel.CountryCode = detail.CountryCode;
                    hotel.Review = detail.Review;
                    hotel.ImageUrl = detail.ImageUrl;
                }
            return result;
        }

        public List<HotelDetail> AddFilteringInfo(List<HotelDetail> result )
        {
            //Adding Additional Hotel Information
            foreach (var hotel in result)
            {
                var detail = GetHotelDetailFromDb(hotel.HotelCode);
                hotel.AccomodationType = detail.AccomodationType;
                hotel.Facilities = detail.Facilities;
                hotel.StarCode = GetSimpleCodeByCategoryCode(hotel.StarRating);
            }
            return result;
        }

        public HotelFilterDisplayInfo SetHotelFilterDisplayInfo(List<HotelDetail> hotels, bool isByDestination)
        {
            var filter = new HotelFilterDisplayInfo();
            var zoneDict = new Dictionary<string, ZoneFilter>();
            var accDict = new Dictionary<string, AccomodationFilter>();
            try
            {
                foreach (var hotelDetail in hotels)
                {
                    //For Zone
                    if (isByDestination)
                    {
                        if (!(zoneDict.ContainsKey(hotelDetail.ZoneCode)))
                        {
                            zoneDict.Add(hotelDetail.ZoneCode, new ZoneFilter
                            {
                                Code = hotelDetail.ZoneCode,
                                Count = 1,
                                Name = GetZoneNameFromDict(hotelDetail.ZoneCode)
                            });
                        }
                        else
                        {
                            zoneDict[hotelDetail.ZoneCode].Count += 1;
                        }    
                    }

                    //ForAccomodation
                    if (!(accDict.ContainsKey(hotelDetail.AccomodationType)))
                    {
                        accDict.Add(hotelDetail.AccomodationType, new AccomodationFilter
                        {
                            Code = hotelDetail.AccomodationType,
                            Count = 1,
                            Name = GetHotelAccomodationMultiDesc(hotelDetail.AccomodationType)
                        });
                    }
                    else
                    {
                        accDict[hotelDetail.AccomodationType].Count += 1;
                    }

                    
                }

                filter.ZoneFilter = new List<ZoneFilter>();
                filter.AccomodationFilter = new List<AccomodationFilter>();


                if (isByDestination)
                {
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

                foreach (var accomodation in accDict.Keys)
                {
                    filter.AccomodationFilter.Add(new AccomodationFilter
                    {
                        Code = accDict[accomodation].Code,
                        Count = accDict[accomodation].Count,
                        Name = accDict[accomodation].Name
                    });
                }
                
            }
            catch(Exception e)
            {
                Debug.Print(e.Message);
            }
            
            return filter;
        }

        public List<int> GetStarFilter(List<bool> starFilter)
        {
            if (starFilter != null)
            {
                int count = 0;
                var completedList = new List<int> { 1, 2, 3, 4, 5};
                var listStar = new List<int>();
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
