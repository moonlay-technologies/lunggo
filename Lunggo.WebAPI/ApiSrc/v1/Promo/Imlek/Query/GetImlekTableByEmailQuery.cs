using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query
{
    public class GetImlekTableByEmailQuery : QueryBase<GetImlekTableByEmailQuery, ImlekTable>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM PromoImlek WHERE Email = @Email";
        }
    }
}