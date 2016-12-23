using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Documents;
using Lunggo.Framework.Extension;
using Lunggo.Framework.SharedModel;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public SearchHotelOutput Search(SearchHotelInput input)
        {
            var hotelResult = new SearchHotelOutput();

            switch (input.SearchHotelType)
            {
                case SearchHotelType.Location:
                    hotelResult = DoSearchByLocation(input);
                    break;

                case SearchHotelType.SearchID:
                    hotelResult = DoSearchById(input);
                    break;

                case SearchHotelType.HotelCode:
                    hotelResult = DoSearchByHotelCode(input);
                    break;
            }

            return hotelResult;
        }

        public SearchHotelOutput DoSearchByLocation(SearchHotelInput input)
        {
            Guid generatedSearchId = Guid.NewGuid();
            var hotelBedsClient = new HotelBedsSearchHotel();
            var allCurrency = Currency.GetAllCurrencies();
            SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);
            var request = new SearchHotelCondition();
            var detailDestination = GetLocationById(input.Location);

            if (input.HotelCode != 0)
            {
                request.Occupancies = input.Occupancies;
                request.HotelCode = input.HotelCode;
                request.CheckIn = input.CheckIn;
                request.Nights = input.Nights;
                request.Checkout = input.CheckIn.AddDays(input.Nights);
                request.SearchId = generatedSearchId.ToString();
            }
            else
            {
                request.CheckIn = input.CheckIn;
                request.Checkout = input.CheckIn.AddDays(input.Nights);
                request.Nights = input.Nights;
                request.Occupancies = input.Occupancies;
                request.SearchId = generatedSearchId.ToString();

                switch (AutocompleteTypeCd.Mnemonic(detailDestination.Type))
                {
                    case AutocompleteType.Zone:
                        request.Zone = detailDestination.Code;
                        break;
                    case AutocompleteType.Destination:
                        request.Destination = detailDestination.Code;
                        break;
                    case AutocompleteType.Area:
                        request.Area = detailDestination.Code;
                        break;
                    case AutocompleteType.Hotel:
                        request.HotelCode = int.Parse(detailDestination.Code);
                        break;

                };
            }

            var swAv = Stopwatch.StartNew();
            var realOccupancies = request.Occupancies;
            request.Occupancies = PreProcessOccupancies(request.Occupancies);
            var result = hotelBedsClient.SearchHotel(request);
            result.Occupancies = realOccupancies;
            swAv.Stop();
            Debug.Print("AVAIALABILITY:" + swAv.Elapsed.ToString());
            result.SearchId = generatedSearchId.ToString();


            if (result.HotelDetails == null || result.HotelDetails.Count == 0)
                return new SearchHotelOutput()
                    {
                        IsSuccess = true,
                        DestinationName = result.DestinationName,
                        ReturnedHotelCount = 0,
                        TotalHotelCount = 0,
                        FilteredHotelCount = 0
                    };
            //result.HotelDetails = FilterSearchRoomByCapacity(result.HotelDetails, input.Occupancies);
            if (result.HotelDetails.Count == 0)
                return new SearchHotelOutput()
                {
                    IsSuccess = true,
                    DestinationName = result.DestinationName,
                    ReturnedHotelCount = 0,
                    TotalHotelCount = 0,
                    FilteredHotelCount = 0
                };
            var swPr = Stopwatch.StartNew();
            AddPriceMargin(result.HotelDetails);
            swPr.Stop();
            Debug.Print("PRIMARGIN:" + swPr.Elapsed.ToString());
            var dict = new Dictionary<int, HotelDetailsBase>();
            var details = new HotelDetailsBase();
            //GetHotelDetailByLocation(request.Destination);
            switch (AutocompleteTypeCd.Mnemonic(detailDestination.Type))
            {
                case AutocompleteType.Zone:
                    dict = GetHotelDetailByLocation(request.Zone);
                    result.HotelDetails = ApplyHotelDetails(dict, result.HotelDetails);
                    result.DestinationName = detailDestination.Zone + ", " + detailDestination.Destination + ", " + detailDestination.Country;
                    break;
                case AutocompleteType.Destination:
                    dict = GetHotelDetailByLocation(request.Destination);
                    result.HotelDetails = ApplyHotelDetails(dict, result.HotelDetails);
                    result.DestinationName = detailDestination.Destination + ", " + detailDestination.Country;
                    break;
                case AutocompleteType.Area:
                    dict = GetHotelDetailByLocation(request.Area);
                    result.HotelDetails = ApplyHotelDetails(dict, result.HotelDetails);
                    result.DestinationName = detailDestination.Destination + ", " + detailDestination.Country;
                    break;
                case AutocompleteType.Hotel:
                    details = GetHotelDetailFromDb(request.HotelCode);
                    result.HotelDetails = ApplyHotelDetails(details, result.HotelDetails);
                    result.DestinationName = details.HotelName + ", " +detailDestination.Destination + ", " + detailDestination.Country;
                    break;
            };
            
            result.HotelDetails = AddDetailInfoForSearchResult(result.HotelDetails);
            result.HotelFilterDisplayInfo = SetHotelFilterDisplayInfo(result.HotelDetails, AutocompleteTypeCd.Mnemonic(detailDestination.Type));
            var sortedHotel = result.HotelDetails.OrderByDescending(x => x.NetFare);
            result.MaxPrice = sortedHotel.Select(x => x.NetFare).FirstOrDefault();
            result.MinPrice = sortedHotel.Select(x => x.NetFare).LastOrDefault();
            //result.DestinationName = detailDestination.Destination;

            //REMEMBER TO UNCOMMENT THIS
            Task.Run(() => SaveSearchResultintoDatabaseToCache(result.SearchId, result));

            List<HotelDetail> firstPageHotelDetails = result.HotelDetails;

            //Sorting
            if (firstPageHotelDetails != null) firstPageHotelDetails = SortHotel(firstPageHotelDetails, input.SortingParam);
            input.Page = input.Page != 0 ? input.Page : 1;
            input.PerPage = input.PerPage != 0 ? input.PerPage : 100;
            int pageCount = (int)Math.Ceiling((decimal)result.HotelDetails.Count / input.PerPage);
            firstPageHotelDetails = SetPagination(firstPageHotelDetails, input.Page, input.PerPage);
            //AddDetailInfoForDisplayHotel(firstPageHotelDetails);
            var searchType = detailDestination.Type.ToString();
            return new SearchHotelOutput
            {
                IsSuccess = true,
                SearchId = result.SearchId,
                DestinationName = result.DestinationName,
                FilteredHotelCount = result.HotelDetails.Count,
                HotelDetailLists = ConvertToHotelDetailForDisplay(firstPageHotelDetails),
                Page = input.Page,
                PerPage = input.PerPage,
                PageCount = pageCount,
                ReturnedHotelCount = firstPageHotelDetails.Count,
                TotalHotelCount = result.HotelDetails.Count,
                HotelFilterDisplayInfo = result.HotelFilterDisplayInfo,
                MaxPrice = result.MaxPrice,
                MinPrice = result.MinPrice,
                IsSpecificHotel = searchType.Equals("Hotel"),
                HotelCode = searchType.Equals("Hotel") ? (int?)firstPageHotelDetails.Select(x => x.HotelCode).FirstOrDefault() : null,
                //ExpiryTime = GetSearchHotelResultExpiry(result.SearchId)
            };
        }

        public SearchHotelOutput DoSearchById(SearchHotelInput input)
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

            //Filtering
            hotels = FilterHotel(hotels, input.FilterParam);

            //Sorting
            if (hotels != null) hotels = SortHotel(hotels, input.SortingParam);

            if (hotels == null || hotels.Count == 0)
                return new SearchHotelOutput()
                {
                    IsSuccess = true,
                    SearchId = input.SearchId,
                    DestinationName = searchResult.DestinationName,
                    Page = input.Page,
                    PerPage = input.PerPage,
                    ReturnedHotelCount = 0,
                    TotalHotelCount = 0,
                    FilteredHotelCount = 0,
                    HotelFilterDisplayInfo = searchResult.HotelFilterDisplayInfo,
                    MaxPrice = searchResult.MaxPrice,
                    MinPrice = searchResult.MinPrice,
                    //ExpiryTime = GetSearchHotelResultExpiry(input.SearchId)
                };
            int pageCount = 0;
            input.Page = input.Page != 0 ? input.Page : 1;
            input.PerPage = input.PerPage != 0 ? input.PerPage : 100;
            pageCount = (int)Math.Ceiling((decimal)hotels.Count / input.PerPage);
            var displayHotel = SetPagination(hotels, input.Page, input.PerPage);

            //AddDetailInfoForDisplayHotel(hotels);

            return new SearchHotelOutput
            {
                SearchId = searchResult.SearchId,
                DestinationName = searchResult.DestinationName,
                HotelDetailLists = hotels.Count > 0 ? ConvertToHotelDetailForDisplay(displayHotel) : null,
                Page = input.Page,
                PerPage = input.PerPage,
                PageCount = pageCount,
                ReturnedHotelCount = displayHotel.Count,
                TotalHotelCount = searchResult.HotelDetails.Count,
                HotelFilterDisplayInfo = searchResult.HotelFilterDisplayInfo,
                FilteredHotelCount = hotels.Count,
                MaxPrice = searchResult.MaxPrice,
                MinPrice = searchResult.MinPrice,
                IsSuccess = true,
                //ExpiryTime = GetSearchHotelResultExpiry(input.SearchId)
            };
        }

        public SearchHotelOutput DoSearchByHotelCode(SearchHotelInput input)
        {
            Guid generatedSearchId = Guid.NewGuid();
            var hotelBedsClient = new HotelBedsSearchHotel();
            var allCurrency = Currency.GetAllCurrencies();
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

            if (results.HotelDetails == null || results.HotelDetails.Count == 0 || results.HotelDetails.Any(hotel => hotel.Rooms == null || hotel.Rooms.Count == 0))
                return new SearchHotelOutput
                            {
                                Errors = new List<HotelError> { HotelError.RateKeyNotFound },
                                IsSuccess = false,
                                ErrorMessages = new List<string> { "Rate Key Not Found!" }
                            };

            AddPriceMargin(results.HotelDetails);

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
                        }
                    }
                }
            }

            if (!isRateFound)
                return new SearchHotelOutput
                            {
                                Errors = new List<HotelError> { HotelError.RateKeyNotFound },
                                IsSuccess = false,
                                ErrorMessages = new List<string> { "Rate Key Not Found!" }
                            };

            return new SearchHotelOutput
                        {
                            IsSuccess = true,
                            HotelRoom = ConvertToSingleHotelRoomForDisplay(roomHotel),
                            ReturnedHotelCount = 1,
                            TotalHotelCount = 1
                        };
        }

        public List<HotelDetail> ApplyHotelDetails(Dictionary<int, HotelDetailsBase> dict, List<HotelDetail> hotels)
        {
            foreach (var hotel in hotels)
            {
                var value = new HotelDetailsBase();
                var found = dict.TryGetValue(hotel.HotelCode, out value);
                if (found)
                {
                    hotel.AccomodationType = value.AccomodationType;
                    hotel.Address = value.Address;
                    hotel.Facilities = value.Facilities;
                    hotel.ZoneCode = value.ZoneCode;
                    hotel.AreaCode = value.AreaCode;
                    hotel.PhonesNumbers = value.PhonesNumbers;
                    hotel.HotelName = value.HotelName;
                    hotel.City = value.City;
                    hotel.Chain = value.Chain;
                    hotel.Email = value.Email;
                    hotel.ImageUrl = value.ImageUrl;
                    hotel.Pois = value.Pois;
                    hotel.Segment = value.Segment;
                    hotel.PostalCode = value.PostalCode;
                }
                else
                {
                    continue;
                }
            }
            return hotels;
        }

        public List<HotelDetail> ApplyHotelDetails(HotelDetailsBase detail, List<HotelDetail> hotels)
        {
            foreach (var hotel in hotels)
            {
                hotel.AccomodationType = detail.AccomodationType;
                hotel.Address = detail.Address;
                hotel.Facilities = detail.Facilities;
                hotel.AreaCode = detail.AreaCode;
                hotel.ZoneCode = detail.ZoneCode;
                hotel.PhonesNumbers = detail.PhonesNumbers;
                hotel.HotelName = detail.HotelName;
                hotel.City = detail.City;
                hotel.Chain = detail.Chain;
                hotel.Email = detail.Email;
                hotel.ImageUrl = detail.ImageUrl;
                hotel.Pois = detail.Pois;
                hotel.Segment = detail.Segment;
                hotel.PostalCode = detail.PostalCode;
            }
            return hotels;
        }
        public List<HotelDetail> AddDetailInfoForDisplayHotel(List<HotelDetail> result)
        {
            foreach (var hotel in result)
            {
                var detail = GetTruncatedHotelDetailFromTableStorage(hotel.HotelCode);
                if (detail != null)
                {
                    hotel.City = detail.City;
                    hotel.ImageUrl = detail.ImageUrl;
                    hotel.IsRestaurantAvailable = detail.IsRestaurantAvailable;
                    hotel.WifiAccess = detail.WifiAccess;
                }
            }
            return result;
        }

        public List<HotelDetail> AddDetailInfoForSearchResult(List<HotelDetail> result)
        {
            var shortlistHotel = new List<HotelDetail>();
            foreach (var hotel in result)
            {
                //var detail = GetHotelDetailFromDb(hotel.HotelCode);
                //var detail = GetHotelAmenitiesAndAccomodationTypeFromTableStorage(hotel.HotelCode);
                //hotel.AccomodationType = detail.AccomodationType;
                if (hotel.AccomodationType == "HOTEL")
                {
                    //hotel.Facilities = detail.Facilities == null
                    //? null
                    //: detail.Facilities.Select(x => new HotelFacility
                    //{
                    //    FacilityCode = x.FacilityCode,
                    //    FacilityGroupCode = x.FacilityGroupCode,
                    //    FullFacilityCode = x.FacilityGroupCode + "" + x.FacilityCode
                    //}).ToList();
                    hotel.StarCode = GetSimpleCodeByCategoryCode(hotel.StarRating);
                    CalculatePriceHotel(hotel);
                    shortlistHotel.Add(hotel);
                }
            }
            return shortlistHotel;
        }

        public List<Occupancy> PreProcessOccupancies(List<Occupancy> paxData)
        {
            var maxAdult = 0;
            var maxChild = 0;
            List<int> childAges = new List<int>();
            var occupancies = new List<Occupancy>();
            foreach (var data in paxData)
            {
                if (data.AdultCount > maxAdult)
                    maxAdult = data.AdultCount;
                if (data.ChildCount > maxChild)
                {
                    maxChild = data.ChildCount;
                    foreach (var age in data.ChildrenAges)
                    {
                        childAges.Add(age);
                    }
                }  
            }
            var pax = new Occupancy
            {
                RoomCount = paxData.Count,
                AdultCount = maxAdult,
                ChildCount = maxChild,
                ChildrenAges = childAges,
            };
            occupancies.Add(pax);
            return occupancies;
        }
    }
}
