using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class KeywordsFilter
    {           
        public List<int> keyword { get; set; }
        public bool allIncluded { get; set; }

        public KeywordsFilter(List<int> keywords, bool allIncluded)
        {
            this.keyword = keywords;
            this.allIncluded = allIncluded;
        }
    }
}
