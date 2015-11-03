using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Error;

namespace Lunggo.ApCommon.Travolutionary
{
    public abstract class TravolutionaryResponseBase
    {
        public HashSet<Error> Errors { get; set; }
        public String SessionId { get; set; }
        public bool IsErrorLess()
        {
            return Errors == null || !Errors.Any();
        }
    }
}
