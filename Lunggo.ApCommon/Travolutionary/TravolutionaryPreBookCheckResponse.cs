using System.Linq;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryPreBookCheckResponse : TravolutionaryResponseBase
    {
        public bool IsBookable
        {
            get { return Errors == null || !Errors.Any(); }
        }
    }
}
