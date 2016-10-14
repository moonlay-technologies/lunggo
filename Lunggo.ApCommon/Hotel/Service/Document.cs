using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.Framework.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveSearchResultToDocument(SearchHotelResult searchHotelResult)
        {
            var document = DocumentService.GetInstance();
            document.Upsert("SearchResult:" + searchHotelResult.SearchId, searchHotelResult, TimeSpan.FromMinutes(30));
        }

        public SearchHotelResult GetAllSearchHotelResultFromDocument(string searchId)
        {
            var document = DocumentService.GetInstance();
            var id = "SearchResult:" + searchId;
            return document.Retrieve<SearchHotelResult>(id);
        }

        public SearchHotelResult GetSearchHotelResultWithFilter(SearchHotelInput input)
        {
            var param = new
            {
                input.SearchId,
                input.SortingParam.AscendingPrice,
                input.SortingParam.DescendingPrice,
                input.SortingParam.ByPopularity,
                input.SortingParam.ByReview,
                input.FilterParam.MaxPrice,
                input.FilterParam.MinPrice,
                input.FilterParam.StarRating,
                input.FilterParam.Area,
                input.FilterParam.BoardCode,
                input.FilterParam.AccomodationType,
                input.FilterParam.Amenities
            };
            var document = DocumentService.GetInstance();
            var searchResultData = document.Execute<SearchHotelResult>(new GetSearchResultBySearchId(), param, param).SingleOrDefault();
            return searchResultData;
        }
        
        public void SaveHotelDetailToDocument(HotelDetailsBase hotelDetails)
        {
            var document = DocumentService.GetInstance();
            document.Upsert("HotelDetail:" + hotelDetails.HotelCode, hotelDetails);
        }

        public HotelDetailsBase GetHotelDetailsFromDocument(int hotelCode)
        {
            var document = DocumentService.GetInstance();
            var id = "HotelDetail:" + hotelCode;
            return document.Retrieve<HotelDetailsBase>(id);
        }
    }
}
