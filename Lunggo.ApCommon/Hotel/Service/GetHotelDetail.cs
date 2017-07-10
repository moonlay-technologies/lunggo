using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket;
using Lunggo.Framework.Documents;
using Microsoft.Azure.Documents;
using Supplier = Lunggo.ApCommon.Hotel.Constant.Supplier;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public GetHotelDetailOutput GetHotelDetail(GetHotelDetailInput input)
        {
            var hotelSearchResult = GetSearchHotelResultFromCache(input.SearchId);
            //SetHotelFullFacilityCode(hotelDetail);
            if (hotelSearchResult == null)
                return new GetHotelDetailOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                    ErrorMessages = new List<string> { "SearchID no longer valid" }
                };

            //Find Hotel Uri based on HotelCode
            var hotel = hotelSearchResult.HotelDetails.SingleOrDefault(x => x.HotelCode == input.HotelCode);
            if (hotel == null)
                return new GetHotelDetailOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                    ErrorMessages = new List<string> { "SearchID no longer valid" }
                };

            //Call Api Get Hotel Detail
            var detailClient = new TiketHotelDetail();
            var resultDetail = detailClient.GetHotelDetail(hotel.HotelUri);
            if (resultDetail == null)
                return new GetHotelDetailOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                    ErrorMessages = new List<string> { "SearchID no longer valid" }
                };

            var hotelDetail = new HotelDetailsBase
            {
                SearchId = input.SearchId,
                DestinationName = hotelSearchResult.DestinationName,
                StarCode = resultDetail.Breadcrumb == null ? 0 : resultDetail.Breadcrumb.StarRating,
                Supplier = Supplier.Tiket,
                Longitude = resultDetail.General == null ? 0 : resultDetail.General.Longitude,
                Latitude = resultDetail.General == null ? 0 : resultDetail.General.Latitude,
                Address = resultDetail.General == null ? null : resultDetail.General.Address,
                HotelCode = resultDetail.Breadcrumb == null ? 0 : int.Parse(resultDetail.Breadcrumb.BusinessId),
                HotelName = resultDetail.Breadcrumb == null ? null : resultDetail.Breadcrumb.BusinessName,
                City = resultDetail.Breadcrumb == null ? null : resultDetail.Breadcrumb.CityName,
                ZoneCode = resultDetail.Breadcrumb == null ? null : (resultDetail.Breadcrumb.KecamatanName ?? resultDetail.Breadcrumb.KelurahanName),
                ImageUrl = resultDetail.Photos == null ? null : resultDetail.Photos.Photo.Select(x=> new Image
                {
                    Path = x.FileName,
                    Type = x.PhotoType
                }).ToList(),
                PrimaryPhoto = resultDetail.PrimaryPhotos,
                CheckInDate = hotelSearchResult.CheckIn,
                CheckOutDate = hotelSearchResult.CheckOut,
                CountryCode = resultDetail.Breadcrumb == null ? null : resultDetail.Breadcrumb.CountryName,
                Facilities = resultDetail.AvailFacilities == null ? null : resultDetail.AvailFacilities.AvailFacility.Select(x=> new HotelFacility
                {
                    FullFacilityCode = x.FacilityType,
                    FacilityName = x.FacilityName
                }).ToList(),
            };
            hotelDetail.Description = new List<HotelDescriptions>();
            hotelDetail.Description.Add(new HotelDescriptions
            {
                Description = resultDetail.General == null ? null : resultDetail.General.Description,
                languageCode = "IND"
            });
            
            return new GetHotelDetailOutput
            {
                IsSuccess = true,
                HotelDetail = ConvertToTiketHotelDetailOnlyForDisplay(hotelDetail)
            };
        }

        public HotelDetailsBase GetHotelDetailFromDb (int hotelCode)
        {
            return GetHotelDetailFromTableStorage(hotelCode);
        }

        public bool SetDetailFromSearchResult(ref HotelDetailsBase hotel ,string searchId, out decimal originalPrice, out decimal netFare)
        {
            originalPrice = 0;
            netFare = 0;
            var hotelTemp = hotel;

            var searchResultData = GetSearchHotelResultFromCache(searchId);
            
            if (searchResultData == null) return false;

            var searchResulthotel = searchResultData.HotelDetails.SingleOrDefault(p => p.HotelCode == hotelTemp.HotelCode);
            if (searchResulthotel == null)
                hotel = null;
            hotel.Rooms = searchResulthotel.Rooms;

            if (hotel.ImageUrl != null)
            {
                foreach (var room in hotel.Rooms)
                {
                    room.Images = hotel.ImageUrl.Where(x => x.Type == "HAB").Select(x => x.Path).ToList();
                }
            }
           
            SetRegIdsAndTnc(hotel.Rooms, searchResultData.CheckIn, hotel.HotelCode);
            originalPrice = searchResulthotel.OriginalTotalFare;
            netFare = searchResulthotel.NetTotalFare;

            return true;
        }

        public void SetHotelFullFacilityCode(HotelDetailsBase hotel)
        {
            if (hotel.Facilities != null && hotel.Facilities.Count != 0)
            {
                foreach (var data in hotel.Facilities)
                {
                    data.FullFacilityCode = ((data.FacilityGroupCode*1000) + data.FacilityCode).ToString();
                }    
            }
        }
    }
}
