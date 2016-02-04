using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Query
{
    public class GetUnusedVoucherCodesQuery : QueryBase<GetUnusedVoucherCodesQuery, VoucherRecipientsTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM VoucherRecipients WHERE Email LIKE 'IMLEK2567%'";
        }
    }
}