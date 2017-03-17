using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Query
{
    internal class GetPaymentDisabilityStatusQuery : DbQueryBase<GetPaymentDisabilityStatusQuery, bool?>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT IsPaymentDisabled FROM Company WHERE Id = @CompanyId";
        }
    }
}
