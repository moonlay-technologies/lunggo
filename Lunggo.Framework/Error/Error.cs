using System;
using System.Collections.Generic;

namespace Lunggo.Framework.Error
{
    public class Error
    {
        public String Code { get; set; }
        public String Message { get; set; }
    }

    public class ErrorComparer : IEqualityComparer<Error>
    {
        public bool Equals(Error x, Error y)
        {
            if (x == y)
            {
                return true;
            }
            else
            {
                if (x == null || y == null)
                {
                    return false;
                }
                else
                {
                    var firstErrorCode = x.Code;
                    var secondErrorCode = y.Code;

                    return String.Equals(firstErrorCode, secondErrorCode, StringComparison.InvariantCultureIgnoreCase);
                }    
            }
        }

        public int GetHashCode(Error obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}
