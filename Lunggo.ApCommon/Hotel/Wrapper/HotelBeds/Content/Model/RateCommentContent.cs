using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class RateCommentContent
    {
        public int incoming { get; set; }
        public int hotel { get; set; }
        public string code { get; set; }
        public List<CommentsByRates> commentsByRates { get; set; }
    }

    public class CommentsByRates
    {
        public List<int> rateCodes { get; set; }
        public List<Comment> comments { get; set; }
    }

    public class Comment
    {
        public string dateEnd { get; set; }
        public string dateStart { get; set; }
        public string description{ get; set; }
    }
}
