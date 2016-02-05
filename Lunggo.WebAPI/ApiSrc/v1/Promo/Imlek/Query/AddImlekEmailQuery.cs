using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query
{
    public class AddImlekEmailQuery : NoReturnQueryBase<AddImlekEmailQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "INSERT INTO PromoImlek VALUES (@Email, @RetryCount, GETDATE())";
        }
    }
}