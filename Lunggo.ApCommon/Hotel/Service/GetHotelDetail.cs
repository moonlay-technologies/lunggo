using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.Framework.Documents;
using Microsoft.Azure.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {

        public GetHotelDetailOutput GetHotelDetail(GetHotelDetailInput input)
        {
            var hotelDetail = GetHotelDetailFromDb(input.HotelCode);
            return new GetHotelDetailOutput
            {
                HotelDetail = ConvertToHotelDetailsBaseForDisplay(hotelDetail)
            };
        }

        public HotelDetailsBase GetHotelDetailFromDb (int hotelCode)
        {
            var document = DocumentService.GetInstance();
            return document.Retrieve<HotelDetailsBase>("HotelDetail:"+hotelCode);
        }

    }
}
