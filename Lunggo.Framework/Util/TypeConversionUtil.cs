using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Reflection;

namespace Lunggo.Framework.Util
{
    public static class TypeConversionUtil
    {
        public static dynamic ToAnonymousType(Object source)
        {
            return CreateExpandableObject(source); 
        }

        private static ExpandoObject CreateExpandableObject(Object source)
        {
            ExpandoObject expandableObject = new ExpandoObject();
            FillExpandableObject(expandableObject, source);
            return expandableObject;
        }

        private static void FillExpandableObject(ExpandoObject expandableObject,Object source)
        {
            IDictionary<String, Object> expandableObjectAsDictionary = expandableObject;
            foreach (var property in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                expandableObjectAsDictionary.Add(property.Name, property.GetValue(source));
            }
        }
    }
}
