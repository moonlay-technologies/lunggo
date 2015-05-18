using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Core;
using Lunggo.Framework.Error;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryErrorMapper
    {
        public static Framework.Error.Error MapErrorCode(Lunggo.ApCommon.Travolutionary.WebService.Hotel.Error error)
        {
            return new Lunggo.Framework.Error.Error
            {
                Code = error.ErrorCode,
                Message = error.Message
            };
        }
    }
}
