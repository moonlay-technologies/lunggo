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
            clauseBuilder.Append("SELECT c.hotels");
            clauseBuilder.Append("FROM c");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE c.searhId = @SearchId");
            if (condition.FilterParam.Area != null)
            { clauseBuilder.Append("AND c.hotels.zoneCd IN @FilterParam.Area"); }
            if (condition.FilterParam.StarRating != null)
            { clauseBuilder.Append("AND c.hotels.starRating IN @FilterParam.StarRating"); }
            if (condition.FilterParam.AccomodationType != null)
            { clauseBuilder.Append("AND c.hotels.accomodationType IN @FilterParam.AccomodationType"); }
            if (condition.FilterParam.Amenities != null)
            { clauseBuilder.Append("AND c.hotels.facilityCd IN @FilterParam.Amenities"); }
            return clauseBuilder.ToString();
        }

        private static string CreateOrderByClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("ORDER BY c.hotelCd ASC");
            if (condition.SortingParam.AscendingPrice == true)
            { clauseBuilder.Append(", c.netFare ASC"); }
            if (condition.SortingParam.DescendingPrice == true)
            { clauseBuilder.Append(", c.netFare DESC"); }
            //if (condition.SortingParam.ByReview == true)
            //{ clauseBuilder.Append(", c.review DESC"); }
            //if (condition.SortingParam.ByPopularity == true)
            //{ clauseBuilder.Append(", c.popularity DESC"); }
            return clauseBuilder.ToString();
        }
    }
}
