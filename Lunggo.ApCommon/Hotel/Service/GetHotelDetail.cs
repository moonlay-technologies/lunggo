using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Documents;
using Lunggo.Hotel.ViewModels;
using Microsoft.Azure.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public HotelDetailsBase GetHotelDetail (int hotelCode)
        {
            var document = DocumentService.GetInstance();
            return document.Retrieve<HotelDetailsBase>("HotelDetail:"+hotelCode);
        }
    }
}
