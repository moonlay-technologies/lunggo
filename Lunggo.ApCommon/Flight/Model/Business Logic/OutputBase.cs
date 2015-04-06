using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class OutputBase
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
    }
}
