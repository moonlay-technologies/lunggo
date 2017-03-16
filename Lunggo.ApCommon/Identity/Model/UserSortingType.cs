using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;

namespace Lunggo.ApCommon.Identity.Model
{
    public enum UserSortingType
    {
        Undefined = 0,
        AscendingName = 1,
        DescendingName = 2,
        AscendingPosition = 3,
        DescendingPosition = 4,
        AscendingDepartment = 5,
        DescendingDepartment = 6,
        AscendingBranch = 7,
        DescendingBranch = 8,
        AscendingEmail = 9,
        DescendingEmail = 10
    }

    internal class UserSortingTypeCd
    {
       
        internal static UserSortingType Mnemonic(string sortingType)
        {
            switch (sortingType)
            {
                case "ascendingname":
                    return UserSortingType.AscendingName;
                case "descendingname":
                    return UserSortingType.DescendingName;
                case "ascendingposition":
                    return UserSortingType.AscendingPosition;
                case "descendingposition":
                    return UserSortingType.DescendingPosition;
                case "ascendingdepartment":
                    return UserSortingType.AscendingDepartment;
                case "descendingdepartment":
                    return UserSortingType.DescendingDepartment;
                case "ascendingbranch":
                    return UserSortingType.AscendingBranch;
                case "descendingbranch":
                    return UserSortingType.DescendingBranch;
                case "ascendingemail":
                    return UserSortingType.AscendingEmail;
                case "descendingemail":
                    return UserSortingType.DescendingEmail;
                default:
                    return UserSortingType.Undefined;
            }
        }
    }
}
