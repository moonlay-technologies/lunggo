using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Util
{
    public class GenerateTokenUtil
    {
        public static string GenerateTokenByRsvNo(string rsvNo)
        {
            var result = "";
            if (rsvNo.Length > 6)
            {
                rsvNo = rsvNo.Substring(rsvNo.Length - 6);
            }
            foreach (var ch in rsvNo)
            {
                result += (ch * 4294967295).ToString();
            }
            return result;
        }
    }
}
