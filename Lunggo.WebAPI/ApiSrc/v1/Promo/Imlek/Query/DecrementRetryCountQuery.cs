using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query
{
    public class DecrementRetryCountQuery : NoReturnQueryBase<DecrementRetryCountQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE PromoImlek SET RetryCount = (RetryCount - 1) WHERE Email = @Email";
        }
    }
}