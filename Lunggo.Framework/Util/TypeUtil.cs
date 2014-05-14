using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Reflection;

namespace Lunggo.Framework.Util
{
    public static class TypeUtil
    {
        public static dynamic ToAnonymousType(Object source)
        {
            return CreateExpandableObject(source); 
        }

        public static dynamic ToAnonymousType(Object source, Func<PropertyInfo,bool> typeFilter)
        {
            return CreateExpandableObject(source, typeFilter);
        }

        public static dynamic ToAnonymousType(Object source, IEnumerable<String> propertyNames)
        {
            return CreateExpandableObject(source, propertyNames);
        }

        private static ExpandoObject CreateExpandableObject(Object source, Func<PropertyInfo, bool> typeFilter = null)
        {
            ExpandoObject expandableObject = new ExpandoObject();
            FillExpandableObject(expandableObject, source,typeFilter);
            return expandableObject;
        }

        private static ExpandoObject CreateExpandableObject(Object source, IEnumerable<String> propertyNames)
        {
            ExpandoObject expandableObject = new ExpandoObject();
            FillExpandableObject(expandableObject, source, propertyNames);
            return expandableObject;
        }

        private static void FillExpandableObject(ExpandoObject expandableObject, Object source, IEnumerable<String> propertyNames)
        {
            IDictionary<String, Object> expandableObjectAsDictionary = expandableObject;
            foreach(var propertyName in propertyNames)
            {
                var property = source.GetType().GetProperty(propertyName);
                expandableObjectAsDictionary.Add(property.Name, property.GetValue(source));
            }
        }

        private static void FillExpandableObject(ExpandoObject expandableObject, Object source, Func<PropertyInfo, bool> typeFilter)
        {
            IDictionary<String, Object> expandableObjectAsDictionary = expandableObject;
            foreach (var property in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if(typeFilter != null)
                {
                    if(typeFilter(property))
                    {
                        expandableObjectAsDictionary.Add(property.Name, property.GetValue(source));
                    }
                }
                else
                {
                    expandableObjectAsDictionary.Add(property.Name, property.GetValue(source));
                }
            }
        }
    }
}
