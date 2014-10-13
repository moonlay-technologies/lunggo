using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Hotel.Search.Object;

namespace Lunggo.ApCommon.Model
{
    public class HotelDetailViewModel : SearchBase
    {
        public long HotelId { get; set; }

        public SearchServiceRequest ToSearchServiceRequest()
        {
            SearchServiceRequest serviceRequest =  new SearchServiceRequest();
            try
            {
                //process serviceRequest
            }
            catch (Exception)
            {
                
                throw;
            }
            return serviceRequest;
        }
    }
}
