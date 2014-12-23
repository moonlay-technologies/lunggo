using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Logic
{
    public class HotelsSortComparer
    {
        public static IComparer<HotelDetail> GetComparer(HotelsSearchSortType sortType)
        {
            switch (sortType)
            {
                case HotelsSearchSortType.AlphanumericAscending:
                    return new HotelDetailSortNameAscending();
                case HotelsSearchSortType.AlphanumericDescending:
                    return new HotelDetailSortNameDescending();
                case HotelsSearchSortType.PriceAscending:
                    return new HotelDetailSortPriceAscending();
                case HotelsSearchSortType.PriceDescending:
                    return new HotelDetailSortPriceDescending();
                case HotelsSearchSortType.StarRatingAscending:
                    return new HotelDetailSortStarAscending();
                case HotelsSearchSortType.StarRatingDescending:
                    return new HotelDetailSortStarDescending();
                case HotelsSearchSortType.Default:
                    return new HotelDetailSortPriceAscending();
                default:
                    return new HotelDetailSortPriceAscending();
            }
        }
    }

    public class HotelDetailSortPriceAscending : IComparer<HotelDetail>
    {
        public int Compare(HotelDetail x, HotelDetail y)
        {
            if (x.LowestPrice.Value < y.LowestPrice.Value)
            {
                return -1;
            }
            else if (x.LowestPrice.Value > y.LowestPrice.Value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class HotelDetailSortPriceDescending : IComparer<HotelDetail>
    {
        public int Compare(HotelDetail x, HotelDetail y)
        {
            if (x.LowestPrice.Value < y.LowestPrice.Value)
            {
                return 1;
            }
            else if (x.LowestPrice.Value > y.LowestPrice.Value)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class HotelDetailSortStarDescending : IComparer<HotelDetail>
    {
        public int Compare(HotelDetail x, HotelDetail y)
        {
            if (x.StarRating < y.StarRating)
            {
                return 1;
            }
            else if (x.StarRating > y.StarRating)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class HotelDetailSortStarAscending : IComparer<HotelDetail>
    {
        public int Compare(HotelDetail x, HotelDetail y)
        {
            if (x.StarRating < y.StarRating)
            {
                return -1;
            }
            else if (x.StarRating > y.StarRating)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class HotelDetailSortNameAscending : IComparer<HotelDetail>
    {
        public int Compare(HotelDetail x, HotelDetail y)
        {
            return System.String.Compare(x.HotelName, y.HotelName, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    public class HotelDetailSortNameDescending : IComparer<HotelDetail>
    {
        public int Compare(HotelDetail x, HotelDetail y)
        {
            var comparisonResult = System.String.Compare(x.HotelName, y.HotelName,
                System.StringComparison.OrdinalIgnoreCase);
            switch (comparisonResult)
            {
                case -1:
                    return 1;
                case 1:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}
