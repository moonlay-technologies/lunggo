using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;

namespace Lunggo.ApCommon.Hotel.Query
{
    internal class GetSearchResultBySearchId : DocQueryBase
    {
        public override string GetQueryString(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            //queryBuilder.Append()
            return queryBuilder.ToString();
            //return "SELECT c.hotels FROM c WHERE c.searchId = @SearchId AND c.hotelCd = @HotelCode";
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT h AS hotels ");
            clauseBuilder.Append("FROM c ");
            clauseBuilder.Append("JOIN h in c.hotels ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE c.id = CONCAT('SearchResult:' , @SearchId) ");
            if (condition.Area != null)
            { clauseBuilder.Append("AND h.zoneCd IN @Area "); }
            if (condition.MaxPrice != null && condition.MinPrice != null)
            {
                { clauseBuilder.Append("AND h.originalFare BETWEEN @MaxPrice AND @MinPrice "); }
            }
            if (condition.StarRating != null)
            { clauseBuilder.Append("AND h.starRating IN @StarRating "); }
            if (condition.AccomodationType != null)
            { clauseBuilder.Append("AND h.accomodationType IN @AccomodationType "); }
            if (condition.Amenities != null)
            { clauseBuilder.Append("AND h.facilityCd IN @Amenities "); }
            return clauseBuilder.ToString();
        }

        private static string CreateOrderByClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("ORDER BY c.hotelCd ASC");
            if (condition.AscendingPrice == true)
            { clauseBuilder.Append(", c.netFare ASC"); }
            if (condition.DescendingPrice == true)
            { clauseBuilder.Append(", c.netFare DESC"); }
            //if (condition.SortingParam.ByReview == true)
            //{ clauseBuilder.Append(", c.review DESC"); }
            //if (condition.SortingParam.ByPopularity == true)
            //{ clauseBuilder.Append(", c.popularity DESC"); }
            return clauseBuilder.ToString();
        }
    }
}
