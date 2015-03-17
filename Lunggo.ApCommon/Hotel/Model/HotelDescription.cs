using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Hotel.Model
{
    public abstract class HotelDescriptionBase
    {
        public int Line { get; set; }
        public I18NText Description { get; set; }
    }
    
    public class OnMemHotelDescription : HotelDescriptionBase
    {

    }

    public class HotelDescription : HotelDescriptionBase
    {
        
    }

}
