using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Util;
using Lunggo.CustomerWeb.WebSrc.Common.Constant;
using Lunggo.CustomerWeb.WebSrc.UW200.Model;
using Lunggo.CustomerWeb.WebSrc.UW200.Object;
using Lunggo.Framework.Context;

namespace Lunggo.CustomerWeb.WebSrc.UW200.Logic
{
    public class HotelDetailLogic
    {
        public static Uw200HotelDetailResponse GetHotelDetail(Uw200HotelDetailRequest request)
        {
            var searchServiceRequest = PreProcessHotelDetailRequest(request);
            var hotelDetail = HotelsSearchService.GetHotelDetail(searchServiceRequest.HotelId);
            var response = AssembleResponse(hotelDetail, searchServiceRequest);
            return response;
        }

        private static Uw200HotelDetailResponse AssembleResponse(HotelDetail hotelDetail, HotelRoomsSearchServiceRequest searchServiceRequest)
        {
            var response = new Uw200HotelDetailResponse
            {
                SearchId = searchServiceRequest.SearchId,
                Lang = searchServiceRequest.Lang,
                StayDate = searchServiceRequest.StayDate,
                StayLength = searchServiceRequest.StayLength,
                HotelDetail = hotelDetail != null ? ToUw200HotelDetail(hotelDetail) : null
            };
            return response;
        }

        private static Uw200HotelDetail ToUw200HotelDetail(HotelDetail hotelDetail)
        {
            var retVal = new Uw200HotelDetail
            {
                Address = hotelDetail.Address,
                Area = hotelDetail.Area,
                Country = hotelDetail.Country,
                HotelId = Int32.Parse(hotelDetail.HotelId),
                HotelName = hotelDetail.HotelName,
                IsLatLongSet = hotelDetail.IsLatLongSet,
                Latitude = hotelDetail.Latitude,
                Longitude = hotelDetail.Longitude,
                LowestPrice = hotelDetail.LowestPrice,
                Province = hotelDetail.Province,
                StarRating = hotelDetail.StarRating,
                HotelDescription = GetHotelDescription(hotelDetail) 
            };
            SetFullImageUrl(retVal);
            SetFacilitiesName(retVal);
            return retVal;
        }

        private static IEnumerable<HotelDescription> GetHotelDescription(HotelDetail hotelDetail)
        {
            if (hotelDetail.HotelDescriptions == null)
            {
                return null;
            }
            else
            {
                var activeLanguage = OnlineContext.GetActiveLanguageCode();
                var hotelDescriptionsByLangPreference =
                    hotelDetail.HotelDescriptions.Where(p => p.Description.Lang == activeLanguage);
                var descriptionsByLangPreference = hotelDescriptionsByLangPreference as IList<HotelDescription> ?? hotelDescriptionsByLangPreference.ToList();
                if (descriptionsByLangPreference.Any())
                {
                    return descriptionsByLangPreference;
                }
                else
                {
                    return
                        hotelDetail.HotelDescriptions.Where(
                            p => p.Description.Lang == CustomerWebConstant.DefaultLangForDescription);
                }
            }
        }

        private static void SetFullImageUrl(Uw200HotelDetail uw200HotelDetail)
        {
            if (uw200HotelDetail.ImageUrlList == null) return;
            var hotelId = uw200HotelDetail.HotelId.ToString(CultureInfo.InvariantCulture);
            var isHttps = HttpContext.Current.Request.IsSecureConnection;
            foreach (var hotelImage in uw200HotelDetail.ImageUrlList)
            {
                
                if (!String.IsNullOrEmpty(hotelImage.FullSizeUrl))
                {
                    hotelImage.FullSizeUrl =
                        UrlUtil.CreateFullImageUrlForHotel(hotelId,
                            hotelImage.FullSizeUrl, isHttps );
                }

                if (!String.IsNullOrEmpty(hotelImage.ThumbSizeUrl))
                {
                    hotelImage.ThumbSizeUrl = UrlUtil.CreateFullImageUrlForHotel(hotelId, hotelImage.ThumbSizeUrl,
                        isHttps);
                }
            }
        }

        private static void SetFacilitiesName(Uw200HotelDetail uw200HotelDetail)
        {
            if (uw200HotelDetail.Facilities == null) return;
            var activeLanguageCode = OnlineContext.GetActiveLanguageCode();
            foreach (var facility in  uw200HotelDetail.Facilities)
            {
                facility.FacilityName = HotelFacilityUtil.GetFacilityName(facility.FacilityId, activeLanguageCode);
            }
        }

        private static HotelRoomsSearchServiceRequest PreProcessHotelDetailRequest(Uw200HotelDetailRequest request)
        {
            var searchServiceRequest = ParameterPreProcessor.InitializeHotelRoomsSearchServiceRequest(request);
            ParameterPreProcessor.PreProcessLangParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessStayLengthParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessStayDateParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessRoomCountParam(searchServiceRequest, request);
            return searchServiceRequest;
        }
    }
}