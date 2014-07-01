using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Hotel.Object
{
    public enum SortOrder
    {
        PriceAscending,
        PriceDescending,
        StarAscending,
        StarDescending,
        RankAscending,
        RankDescending
    }

    [Flags]
    public enum MealType
    {
        WithoutMeal = 0,
        Breakfast = 1,
        Dinner = 2
    }

    [Flags]
    public enum BedType
    {
        NotSpecified = 0,
        Single = 1,
        Twin = 2,
        Double = 4,
        Triple = 8,
        King = 16,
        Queen = 32
    }

    [Flags]
    public enum HotelRating
    {
        NotSpecified = 0,
        WithoutStar = 1,
        OneStar = 2,
        TwoStars = 4,
        ThreeStars = 8,
        FourStars = 16,
        FiveStars = 32
    }

    
}
