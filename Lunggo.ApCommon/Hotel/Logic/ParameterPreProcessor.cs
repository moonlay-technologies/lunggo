using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.Framework.Util;

namespace Lunggo.ApCommon.Hotel.Logic
{
    public class ParameterPreProcessor
    {
        public static HotelsSearchServiceRequest InitializeHotelsSearchServiceRequest(HotelSearchRequestBase request)
        {
            var searchServiceRequest = new HotelsSearchServiceRequest
            {
                LocationId = request.LocationId,
                SearchId = request.SearchId,
                SearchFilter = new HotelsSearchFilter(),
            };
            return searchServiceRequest;
        }

        public static HotelRoomsSearchServiceRequest InitializeHotelRoomsSearchServiceRequest(HotelRoomsSearchRequestBase request)
        {
            var searchServiceRequest = new HotelRoomsSearchServiceRequest
            {
                HotelId = Int32.Parse(request.HotelId),
                SearchId = request.SearchId,
            };
            return searchServiceRequest;
        }

        public static void PreProcessStayLengthParam(HotelSearchServiceRequestBase searchServiceRequest, HotelRequestBase request)
        {
            //TODO Do not hardcode use configuration file
            searchServiceRequest.StayLength = 1;
            
            if (!String.IsNullOrEmpty(request.StayLength))
            {
                searchServiceRequest.StayLength = Int32.Parse(request.StayLength);
            }
        }

        public static void PreProcessStayDateParam(HotelSearchServiceRequestBase searchServiceRequest, HotelRequestBase request)
        {
            //TODO Do not hardcode use configuration file
            searchServiceRequest.StayDate =  GetDefaultStayDate();

            if (!String.IsNullOrEmpty(request.StayDate))
            {
                //TODO create string constant untuk datetime format
                searchServiceRequest.StayDate = DateTime.ParseExact(request.StayDate, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public static void PreProcessLangParam(HotelSearchServiceRequestBase searchServiceRequest,
            HotelRequestBase request)
        {
            //TODO Do not hardcode use configuration file
            searchServiceRequest.Lang = "id";
            if (!String.IsNullOrEmpty(request.Lang))
            {
                searchServiceRequest.Lang = request.Lang;
            }
        }

        private static DateTime GetDefaultStayDate()
        {
            const int dayOffset = 1;
            return DateTimeUtil.GetJakartaDateTime().AddDays(dayOffset);
        }

        public static void PreProcessPagingParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchRequestBase request)
        {
            //TODO Do not hardcode use configuration file
            searchServiceRequest.StartIndex = 0;
            searchServiceRequest.ResultCount = 10;

            if (!String.IsNullOrEmpty(request.StartIndex))
            {
                searchServiceRequest.StartIndex = Convert.ToInt32(request.StartIndex);
            }

            if (!String.IsNullOrEmpty(request.ResultCount))
            {
                searchServiceRequest.ResultCount = Convert.ToInt32(request.ResultCount);
            }
        }

        public static void PreProcessSortParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchRequestBase request)
        {
            searchServiceRequest.SortBy = HotelsSearchSortType.Default;
            if (!String.IsNullOrEmpty(request.SortBy))
            {
                searchServiceRequest.SortBy =
                    (HotelsSearchSortType)Enum.Parse(typeof(HotelsSearchSortType), request.SortBy);
            }
        }

        public static void PreProcessFilterParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchRequestBase request)
        {
            PreProcessPriceFilterParam(searchServiceRequest, request);
            PreProcessStarRatingFilterParam(searchServiceRequest, request);
        }

        public static void PreProcessPriceFilterParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchRequestBase request)
        {
            searchServiceRequest.SearchFilter.MinPrice = -99999999L;
            searchServiceRequest.SearchFilter.MaxPrice = Int64.MaxValue;

            if (!String.IsNullOrEmpty(request.MinPrice))
            {
                searchServiceRequest.SearchFilter.MinPrice = Convert.ToInt64(request.MinPrice);
            }

            if (!String.IsNullOrEmpty(request.MaxPrice))
            {
                searchServiceRequest.SearchFilter.MaxPrice = Convert.ToInt64(request.MaxPrice);
            }
        }

        public static void PreProcessStarRatingFilterParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchRequestBase request)
        {
            if (!String.IsNullOrEmpty(request.StarRating))
            {
                searchServiceRequest.SearchFilter.StarRatingsToDisplay = request.StarRating.Split(',').ToList().Select(Int32.Parse);
            }
        }

        public static void PreProcessRoomCountParam(HotelSearchServiceRequestBase searchServiceRequest, HotelRequestBase request)
        {
            //TODO Use Configuration File Do not HardCode
            var defaultRoomCount = 1;
            var requestedRoomCount = request.RoomCount;
            if (requestedRoomCount <=0)
            {
                requestedRoomCount = defaultRoomCount;
            }
            var roomOccupantList = new List<RoomOccupant>();
            for (var i = 0; i < requestedRoomCount; i++)
            {
                roomOccupantList.Add(new RoomOccupant
                {
                    AdultCount = 2, //TODO Do not hardcode use configuration file
                    ChildrenAges = null
                });
            }
            searchServiceRequest.RoomOccupants = roomOccupantList;
        }
    }
}
