using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.WebAPI.ApiSrc.v1.Hotels;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Object;

namespace Lunggo.WebAPI
{
    public class DummyLogic
    {
        public static IEnumerable<HotelExcerpt> GetHotels(HotelSearchApiRequest request)
        {
            var completeHotelList = DummyData.GetHotelCompleteList();
            var filteredList = FilterList(completeHotelList, request);
            var sortedList = SortList(filteredList, request);
            var pagedList = PageList(sortedList, request);
            var hotelExcerptList = ConvertToHotelExcerptList(pagedList);
            return hotelExcerptList;
        }

        private static IEnumerable<HotelDetailComplete> FilterByPrice(IEnumerable<HotelDetailComplete> hotelList, HotelSearchApiRequest request)
        {
            var minValue = -99999999L;
            var maxValue = Int64.MaxValue;
            if (!String.IsNullOrEmpty(request.MinPrice))
            {
                minValue = Convert.ToInt64(request.MinPrice);
            }

            if (!String.IsNullOrEmpty(request.MaxPrice))
            {
                maxValue = Convert.ToInt64(request.MaxPrice);
            }

            return hotelList.Where(p => p.LowestPrice.Value >= minValue && p.LowestPrice.Value <= maxValue);
        }

        private static IEnumerable<HotelDetailComplete> FilterByStarRating(IEnumerable<HotelDetailComplete> hotelList, HotelSearchApiRequest request)
        {
            if (String.IsNullOrEmpty(request.StarRating))
            {
                return hotelList;
            }
            else
            {
                var starArray = request.StarRating.Split(',');
                return hotelList.Where(p => starArray.Contains(p.StarRating.ToString(CultureInfo.InvariantCulture)));
            }
        }

        private static IEnumerable<HotelExcerpt> ConvertToHotelExcerptList(IEnumerable<HotelDetailComplete> sourceList)
        {
            return sourceList.Select(
                hotel => new HotelExcerpt
            {
                Address = hotel.Address, 
                Area = hotel.Area, 
                Country = hotel.Country, 
                HotelId = hotel.HotelId, 
                HotelName = hotel.HotelName, 
                ImageUrlList = hotel.ImageUrlList, 
                Latitude = hotel.Latitude, 
                Longitude = hotel.Longitude, 
                LowestPrice = hotel.LowestPrice, 
                Province = hotel.Province, 
                StarRating = hotel.StarRating
            });
        }

        private static IEnumerable<HotelDetailComplete> PageList(IEnumerable<HotelDetailComplete> hotelList,
            HotelSearchApiRequest request)
        {
            var startIndex = 0;
            var count = 10;

            if (!String.IsNullOrEmpty(request.StartIndex))
            {
                startIndex = Convert.ToInt32(request.StartIndex);
            }

            if (!String.IsNullOrEmpty(request.ResultCount))
            {
                count = Convert.ToInt32(request.ResultCount);
            }

            return hotelList.Skip(startIndex).Take(count);
        }

        private static IEnumerable<HotelDetailComplete> SortList(IEnumerable<HotelDetailComplete> hotelList,
            HotelSearchApiRequest request)
        {
            var sortBy = 0;
            if (!String.IsNullOrEmpty(request.SortBy))
            {
                sortBy = Convert.ToInt32(request.SortBy);
            }
            var comparer = GetHotelDetailComparer(sortBy);
            return hotelList.OrderBy(p=>p,comparer);
        }

        private static IComparer<HotelDetailComplete> GetHotelDetailComparer(int sortBy)
        {
            switch (sortBy)
            {
                case (int)HotelSearchSortType.AlphanumericAscending:
                    return new HotelDetailSortNameAscending();
                case (int)HotelSearchSortType.AlphanumericDescending:
                    return new HotelDetailSortNameDescending();
                case (int)HotelSearchSortType.PriceAscending:
                    return new HotelDetailSortPriceAscending();
                case (int)HotelSearchSortType.PriceDescending:
                    return new HotelDetailSortPriceDescending();
                case (int)HotelSearchSortType.StarRatingAscending:
                    return new HotelDetailSortStarAscending();
                case (int)HotelSearchSortType.StarRatingDescending:
                    return new HotelDetailSortStarDescending();
                default:
                    return new HotelDetailSortPriceAscending();
            }
        }

        

    


}