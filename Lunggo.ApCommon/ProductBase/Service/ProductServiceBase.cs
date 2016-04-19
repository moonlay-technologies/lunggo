using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.ProductBase.Model;

namespace Lunggo.ApCommon.ProductBase.Service
{
    internal abstract partial class ProductServiceBase<T, TRsv>
        where T : ProductServiceBase<T, TRsv>, new()
        where TRsv : ReservationBase<TRsv>, new()
    {
        private static readonly T Instance = new T();

        private ProductServiceBase()
        {

        }

        public static T GetInstance()
        {
            return Instance;
        }

        public abstract void Init();
    }
}
