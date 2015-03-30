using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    public partial class MystiflyWrapper
    {
        private static Lunggo.Framework.Error.Error MapError(Error error)
        {
            return new Framework.Error.Error
            {
                Code = error.Code,
                Message = error.Message
            };
        }
    }
}
