using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using CsQuery.Engine.PseudoClassSelectors;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
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
            var hotelResult = new SearchHotelOutput();
            Guid generatedSearchId = Guid.NewGuid();
            var hotelBedsClient = new HotelBedsSearchHotel();
            var allCurrency = Currency.GetAllCurrencies();

            switch (input.SearchHotelType)
            {
                case SearchHotelType.SearchID:
                    if (input.SearchId != null)
                    {
                        var searchResult = GetSearchHotelResultFromCache(input.SearchId);

                        if (searchResult == null)
                            return new SearchHotelOutput
                            {
                                Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                                IsSuccess = false,
                                ErrorMessages = new List<string> { "Error while getting search result by searchId" }
                            };

                        var hotels = searchResult.HotelDetails;
                        var facilityData = new List<string>();
                        if (input.FilterParam != null && input.FilterParam.FacilityFilter != null && input.FilterParam.FacilityFilter.Facilities != null)
                        {
                            foreach (var facilities in input.FilterParam.FacilityFilter.Facilities.Select(param => HotelFacilityFilters[param].FacilityCode))
                            {
                                facilityData.AddRange(facilities);
                            }
                        }

                        //Filtering
                        if (hotels != null && input.FilterParam != null && (input.FilterParam.FacilityFilter.Facilities != null || input.FilterParam.StarFilter.Stars != null || input.FilterParam.ZoneFilter.Zones != null || input.FilterParam.PriceFilter.MinPrice > 0 || input.FilterParam.PriceFilter.MaxPrice > 0))
                        {
                               hotels = searchResult.HotelDetails.Where(p =>
                            (input.FilterParam.ZoneFilter == null || input.FilterParam.ZoneFilter.Zones == null || input.FilterParam.ZoneFilter.Zones.Contains(p.ZoneCode)) && 
                            (input.FilterParam.AccommodationTypeFilter == null || input.FilterParam.AccommodationTypeFilter.Accomodations == null || input.FilterParam.AccommodationTypeFilter.Accomodations.Contains(p.AccomodationType)) &&
                                    (facilityData.Count == 0 || facilityData.Any(e => p.Facilities.Select(x => x.FullFacilityCode).ToList().Contains(e))) &&
                            (input.FilterParam.StarFilter == null || input.FilterParam.StarFilter.Stars == null || input.FilterParam.StarFilter.Stars.Contains(p.StarCode)) &&
                            (input.FilterParam.PriceFilter == null || (p.NetFare >= input.FilterParam.PriceFilter.MinPrice && p.NetFare <= input.FilterParam.PriceFilter.MaxPrice))
                            ).ToList();
                        }

                        //Sorting
                        switch (SortingTypeCd.Mnemonic(input.SortingParam))
                        {
                            case SortingType.AscendingPrice:
                                if (hotels != null) hotels = hotels.OrderBy(p => p.NetFare).ToList();
                                break;
                            case SortingType.DescendingPrice:
                                if (hotels != null) hotels = hotels.OrderByDescending(p => p.NetFare).ToList();
                                break;
                            case SortingType.AscendingStar:
                                if (hotels != null) hotels = hotels.OrderBy(p => p.StarCode).ToList();
                                break;
                            case SortingType.DescendingStar:
                                if (hotels != null) hotels = hotels.OrderByDescending(p => p.StarCode).ToList();
                                break;
                            default:
                                hotels = hotels.OrderBy(p => p.NetFare).ToList();
                                break;
                        }
                        var sortedHotel = searchResult.HotelDetails.OrderByDescending(x => x.NetFare);
                        if (hotels == null)
                        {
                            Console.WriteLine("Search result is empty");
                            return new SearchHotelOutput()
                            {
                                IsSuccess = true,
                                ReturnedHotelCount = 0,
                                TotalHotelCount = 0,
                                FilteredHotelCount = 0,
                                HotelFilterDisplayInfo = searchResult.HotelFilterDisplayInfo,
                                MaxPrice = sortedHotel.Select(x => x.NetFare).FirstOrDefault(),
                                MinPrice = sortedHotel.Select(x => x.NetFare).LastOrDefault(),
                            };
                        }

                        List<HotelDetail> hotelList;
                        int pageCount = 0;

                        //input.Page = input.Page != 0 ? input.Page : 1;
                        //input.PerPage = input.PerPage != 0 ? input.PerPage : 100;

                        //hotelList = hotels.Skip((input.Page - 1) * input.PerPage).Take(input.PerPage).ToList();
                        //pageCount = (int)Math.Ceiling((double)hotels.Count / input.PerPage);

                        //paging
                        if (input.Page != 0 && input.PerPage != 0)
                        {
                            if (input.Page == 1)
                                hotelList = hotels.Take(input.PerPage).ToList();
                            else
                                hotelList = hotels.Skip((input.Page - 1)*input.PerPage).Take(input.PerPage).ToList();
                            pageCount = (int)Math.Ceiling((double)hotels.Count / input.PerPage);
                        }
                        else
                        {
                            //harusnya return error
                            hotelList = hotels.Take(100).ToList();
                            pageCount = (int)Math.Ceiling((double)hotels.Count / 100);
                        }

                        hotelList = AddHotelDetail(hotelList);
                        

                        hotelResult = new SearchHotelOutput
                        {
                            SearchId = searchResult.SearchId,
                            HotelDetailLists = hotelList.Count > 0 ? ConvertToHotelDetailForDisplay(hotelList) : null,
                            Page = input.Page,
                            PerPage = input.PerPage,
                            PageCount = pageCount,
                            ReturnedHotelCount = hotelList.Count,
                            TotalHotelCount = searchResult.HotelDetails.Count,
                            HotelFilterDisplayInfo = searchResult.HotelFilterDisplayInfo,
                            FilteredHotelCount = hotels != null ? hotels.Count : 0,
                            MaxPrice = sortedHotel.Select(x => x.NetFare).FirstOrDefault(),
                            MinPrice = sortedHotel.Select(x => x.NetFare).LastOrDefault(),
                            IsSuccess = true
                        };
                    }
                    break;

                case SearchHotelType.Location:
                    var isByDestination = false;
                    SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);


                    var request = new SearchHotelCondition();
                    var detailDestination = GetLocationById(input.Location);

                    if (input.HotelCode != 0)
                    {
                        request.Occupancies = input.Occupancies;
                        request.HotelCode = input.HotelCode;
                        request.CheckIn = input.CheckIn;
                        request.Checkout = input.CheckIn.AddDays(input.Nights);
                        request.SearchId = generatedSearchId.ToString();
                    }
                    else
                    {
                        request = new SearchHotelCondition
                        {
                            CheckIn = input.CheckIn,
                            Checkout = input.CheckIn.AddDays(input.Nights),
                            Nights = input.Nights,
                            Occupancies = input.Occupancies,
                            SearchId = generatedSearchId.ToString()
                        };

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


                    if (result.HotelDetails != null)
                    {
                        AddPriceMargin(result.HotelDetails);
                        result.HotelDetails = AddDetailInfo(result.HotelDetails);
                        result.HotelFilterDisplayInfo = SetHotelFilterDisplayInfo(result.HotelDetails, isByDestination);

                        //REMEMBER TO UNCOMMENT THIS
                        SaveSearchResultintoDatabaseToCache(result.SearchId, result);
                        int pageCount = 0;

                        switch (SortingTypeCd.Mnemonic(input.SortingParam))
                        {
                            case SortingType.AscendingPrice:
                                if (result.HotelDetails != null) result.HotelDetails = result.HotelDetails.OrderBy(p => p.NetFare).ToList();
                                break;
                            case SortingType.DescendingPrice:
                                if (result.HotelDetails != null) result.HotelDetails = result.HotelDetails.OrderByDescending(p => p.NetFare).ToList();
                                break;
                            case SortingType.AscendingStar:
                                if (result.HotelDetails != null) result.HotelDetails = result.HotelDetails.OrderBy(p => p.StarCode).ToList();
                                break;
                            case SortingType.DescendingStar:
                                if (result.HotelDetails != null) result.HotelDetails = result.HotelDetails.OrderByDescending(p => p.StarCode).ToList();
                                break;
                            default:
                                result.HotelDetails = result.HotelDetails.OrderBy(p => p.NetFare).ToList();
                                break;
                        }

                        List<HotelDetail> firstPageHotelDetails;
                        if (input.Page != 0 && input.PerPage != 0)
                        {
                            if (input.Page == 1)
                                firstPageHotelDetails = result.HotelDetails.Take(input.PerPage).ToList();
                            else
                                firstPageHotelDetails = result.HotelDetails.Skip((input.Page - 1) * input.PerPage).Take(input.PerPage).ToList();
                            pageCount = (int)Math.Ceiling((double)result.HotelDetails.Count / input.PerPage);
                        }
                        else
                        {
                            //harusnya return error
                            firstPageHotelDetails = result.HotelDetails.Take(100).ToList();
                            pageCount = (int)Math.Ceiling((double)result.HotelDetails.Count / 100);
                        }

                        firstPageHotelDetails = AddHotelDetail(firstPageHotelDetails);
                        var searchType = detailDestination.Type.ToString();
                        var sortedHotel = result.HotelDetails.OrderByDescending(x => x.NetFare);
                        hotelResult = new SearchHotelOutput
                        {
                            IsSuccess = true,
                            SearchId = result.SearchId,
                            FilteredHotelCount = result.HotelDetails.Count,
                            HotelDetailLists = ConvertToHotelDetailForDisplay(firstPageHotelDetails),
                            Page = input.Page,
                            PerPage = input.PerPage,
                            PageCount = pageCount,
                            ReturnedHotelCount = firstPageHotelDetails.Count,
                            TotalHotelCount = result.HotelDetails.Count,
                            HotelFilterDisplayInfo = result.HotelFilterDisplayInfo,
                            MaxPrice = sortedHotel.Select(x => x.NetFare).FirstOrDefault(),
                            MinPrice = sortedHotel.Select(x => x.NetFare).LastOrDefault(),
                            IsSpecificHotel = searchType.Equals("Hotel"),
                            HotelCode = searchType.Equals("Hotel") ? (int?)firstPageHotelDetails.Select(x => x.HotelCode).FirstOrDefault() : null

                        };
                    }
                    else
                    {
                        Console.WriteLine("Search result is empty");
                        return new SearchHotelOutput()
                        {
                            IsSuccess = true,
                            ReturnedHotelCount = 0,
                            TotalHotelCount = 0,
                            FilteredHotelCount = 0
                        };
                    }
                    break;
                case SearchHotelType.HotelCode:

                    SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);
                    var splittedRegsId = input.RegsId.Split(',');
                    var hotelCd = splittedRegsId[0];
                    var rateKey = splittedRegsId[2];
                    var splittedRateKey = rateKey.Split('|');
                    var checkin = splittedRateKey[0];
                    var checkout = splittedRateKey[1];
                    var roomCd = splittedRateKey[5];
                    var someData = splittedRateKey[6];
                    var board = splittedRateKey[7];

                    var checkinDateTime = new DateTime(Convert.ToInt32(checkin.Substring(0, 4)),
                        Convert.ToInt32(checkin.Substring(4, 2)),
                        Convert.ToInt32(checkin.Substring(6, 2)));

                    var results = hotelBedsClient.SearchHotel(new SearchHotelCondition
                    {
                        HotelCode = input.HotelCode,
                        Occupancies = input.Occupancies,
                        CheckIn = new DateTime(Convert.ToInt32(checkin.Substring(0, 4)), Convert.ToInt32(checkin.Substring(4, 2)),
                            Convert.ToInt32(checkin.Substring(6, 2))),
                        Checkout = new DateTime(Convert.ToInt32(checkout.Substring(0, 4)), Convert.ToInt32(checkout.Substring(4, 2)),
                            Convert.ToInt32(checkout.Substring(6, 2))),
                        SearchId = generatedSearchId.ToString()

                    });

                    if (results.HotelDetails != null || results.HotelDetails.Count != 0)
                    {
                        if (results.HotelDetails.Any(hotel => hotel.Rooms == null || hotel.Rooms.Count == 0))
                        {
                            hotelResult = new SearchHotelOutput
                            {
                                IsSuccess = false
                            };
                        }

                        AddPriceMargin(results.HotelDetails);
                        List<HotelRate> rateList = new List<HotelRate>();
                        HotelRoom roomHotel = new HotelRoom();
                        var isRateFound = false;
                        var counter = 0;
                        foreach (var hotel in results.HotelDetails)
                        {
                            foreach (var room in hotel.Rooms)
                            {
                                foreach (var ratea in room.Rates)
                                {
                                    var ratekey = ratea.RateKey.Split('|');
                                    if (Convert.ToInt32(ratekey[4]) == input.HotelCode && ratekey[5] == roomCd &&
                                        ratekey[6] == someData && ratekey[7] == board)
                                    {
                                        isRateFound = true;
                                        roomHotel.Facilities = room.Facilities;
                                        roomHotel.Images = room.Images;
                                        roomHotel.RoomCode = room.RoomCode;
                                        roomHotel.RoomName = room.RoomName;
                                        roomHotel.Type = room.Type;
                                        roomHotel.TypeName = room.TypeName;
                                        roomHotel.characteristicCd = room.characteristicCd;
                                        roomHotel.SingleRate = ratea;
                                        roomHotel.SingleRate.RegsId = EncryptRegsId(hotel.HotelCode, room.RoomCode, roomHotel.SingleRate.RateKey);
                                        hotelResult = new SearchHotelOutput
                                        {
                                            IsSuccess = true,
                                            HotelRoom = ConvertToSingleHotelRoomForDisplay(roomHotel),
                                            ReturnedHotelCount = 1,
                                            TotalHotelCount = 1
                                        };
                                        Debug.Print("----->FOUND" + counter);
                                        counter++;
                                    }
                                }
                            }
                        }

                        if (!isRateFound)
                        {
                            hotelResult = new SearchHotelOutput
                            {
                                Errors = new List<HotelError> { HotelError.RateKeyNotFound },
                                IsSuccess = false,
                                ErrorMessages = new List<string> { "Rate Key Not Found!" }
                            };
                        }

                    }
                    else
                    {
                        hotelResult = new SearchHotelOutput
                        {
                            IsSuccess = false,
                            Errors = new List<HotelError> { HotelError.RateKeyNotFound },
                            ErrorMessages = new List<string> { "Search Result is null" }
                        };
                    }

                    break;
            }

            return hotelResult;
        }

        private bool zoneFilterContains(string p)
        {
            throw new NotImplementedException();
        }

        public List<HotelDetail> AddHotelDetail(List<HotelDetail> result)
        {
            foreach (var hotel in result)
            {
                var detail2 = GetTruncatedHotelDetailFromTableStorage(hotel.HotelCode);
                if (detail2 != null)
                {
                    hotel.City = detail2.City;
                    hotel.ImageUrl = detail2.ImageUrl;
                    hotel.IsRestaurantAvailable = detail2.IsRestaurantAvailable;
                    hotel.WifiAccess = detail2.WifiAccess;
                }
            }
            return result;
        }


        public List<HotelDetail> AddDetailInfo(List<HotelDetail> result)
        {
            var shortlistHotel = new List<HotelDetail>();
            foreach (var hotel in result)
            {
                //var detail = GetHotelDetailFromDb(hotel.HotelCode);
                var detail = GetHotelAmenitiesAndAccomodationTypeFromTableStorage(hotel.HotelCode);
                hotel.AccomodationType = detail.AccomodationType;
                if (hotel.AccomodationType == "HOTEL")
                {
                    hotel.Facilities = detail.Facilities == null
                    ? null
                    : detail.Facilities.Select(x => new HotelFacility
                    {
                        FacilityCode = x.FacilityCode,
                        FacilityGroupCode = x.FacilityGroupCode,
                        FullFacilityCode = x.FacilityGroupCode + "" + x.FacilityCode
                    }).ToList();
                    hotel.StarCode = GetSimpleCodeByCategoryCode(hotel.StarRating);
                    CalculatePriceHotel(hotel);
                    shortlistHotel.Add(hotel);
                }
            }
            return shortlistHotel;
        }

        public HotelFilterDisplayInfo SetHotelFilterDisplayInfo(List<HotelDetail> hotels, bool isByDestination)
        {
            var filter = new HotelFilterDisplayInfo();
            var zoneDict = new Dictionary<string, ZoneFilterInfo>();
            var starDict = new Dictionary<int, StarFilterInfo>();
            var accDict = new Dictionary<string, AccomodationFilterInfo>();
            var facilityDict = HotelFacilityFilters.Keys.ToDictionary(key => key, key => new FacilitiesFilterInfo
            {
                Name = key,
                Code = key
            });
            try
            {
                foreach (var hotelDetail in hotels)
                {
                    //Zone
                    if (isByDestination)
                    {
                        if (!(zoneDict.ContainsKey(hotelDetail.ZoneCode)))
                        {
                            zoneDict.Add(hotelDetail.ZoneCode, new ZoneFilterInfo
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

                    //Accomodation
                    if (!(accDict.ContainsKey(hotelDetail.AccomodationType)))
                    {
                        accDict.Add(hotelDetail.AccomodationType, new AccomodationFilterInfo
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


                    if (!(starDict.ContainsKey(hotelDetail.StarCode)))
                    {
                        starDict.Add(hotelDetail.StarCode, new StarFilterInfo
                        {
                            Code = hotelDetail.StarCode,
                            Count = 1,
                        });
                    }
                    else
                    {
                        starDict[hotelDetail.StarCode].Count += 1;
                    }

                    //Facilities
                    var keys = facilityDict.Keys;
                    var hotelFacilityDict = new Dictionary<string, bool>();
                    foreach (var facility in hotelDetail.Facilities)
                    {
                        var concatedFacility = facility.FacilityGroupCode + "" + facility.FacilityCode;

                        foreach (var key in keys)
                        {
                            if (!hotelFacilityDict.ContainsKey(key))
                                hotelFacilityDict[key] = false;
                            if (HotelFacilityFilters[key].FacilityCode.Contains(concatedFacility))
                            {
                                hotelFacilityDict[key] = true;
                            }
                        }

                    }
                    foreach (var key in keys)
                    {
                        facilityDict[key].Count += hotelFacilityDict[key] ? 1 : 0;
                    }


                }
                filter.ZoneFilter = new List<ZoneFilterInfo>();
                filter.AccomodationFilter = new List<AccomodationFilterInfo>();
                filter.FacilityFilter = new List<FacilitiesFilterInfo>();
                filter.StarFilter = new List<StarFilterInfo>();

                if (isByDestination)
                {
                    foreach (var zone in zoneDict.Keys)
                    {
                        filter.ZoneFilter.Add(new ZoneFilterInfo
                        {
                            Code = zoneDict[zone].Code,
                            Count = zoneDict[zone].Count,
                            Name = zoneDict[zone].Name,
                        });
                    }
                }

                foreach (var accomodation in accDict.Keys)
                {
                    filter.AccomodationFilter.Add(new AccomodationFilterInfo
                    {
                        Code = accDict[accomodation].Code,
                        Count = accDict[accomodation].Count,
                        Name = accDict[accomodation].Name
                    });
                }

                foreach (var key in facilityDict.Keys)
                {
                    filter.FacilityFilter.Add(new FacilitiesFilterInfo
                    {
                        Code = facilityDict[key].Code,
                        Count = facilityDict[key].Count,
                        Name = facilityDict[key].Name
                    });
                }

                foreach (var key in starDict.Keys)
                {
                    filter.StarFilter.Add(new StarFilterInfo
                    {
                        Code = starDict[key].Code,
                        Count = starDict[key].Count
                    });
                }

            }
            catch (Exception e)
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
                var completedList = new List<int> { 1, 2, 3, 4, 5 };
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
