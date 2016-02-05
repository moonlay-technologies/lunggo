using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query
{
    public class ResetRetryCountQuery : NoReturnQueryBase<ResetRetryCountQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE PromoImlek SET RetryCount = @RetryCount, LastTryDate = GETDATE() WHERE Email = @Email";
        }
    }
}