using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;

namespace Lunggo.ApCommon.Hotel.Model
{
    public  abstract class ResultBase
    {
        public List<HotelError> Errors { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsSuccess { get; set; }
    }
}
