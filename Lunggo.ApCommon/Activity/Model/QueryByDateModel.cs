using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model
{
    public class QueryByDateModel
    {
        public int ActivityTypeId { get; set; }
        public long ActivityId { get; set; }
        public string ActivityTypeName { get; set; }
        public string ActivityName { get; set; }
        public string UrlImage { get; set; }
        public string LangCd { get; set; }

    }
}
