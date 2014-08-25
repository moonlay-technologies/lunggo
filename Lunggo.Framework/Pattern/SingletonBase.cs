using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Pattern
{
    public abstract class SingletonBase<T> where T: SingletonBase<T>
    {
        private static readonly T Instance = CreateInstance();

        private static T CreateInstance()
        {
            var constructor = typeof (T).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Type.EmptyTypes,
                null
            );

            return (T) constructor.Invoke(null);
        }

        public static T  GetInstance()
        {
            return Instance;
        }
    }
}
