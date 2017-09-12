using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Pattern;

namespace Lunggo.ApCommon.Product.Service
{
    public abstract class ProductServiceBase<T> : SingletonBase<T> where T : SingletonBase<T>
    {
        internal abstract void Issue(string rsvNo);

        private static Dictionary<ProductType, Type> _serviceList;

        //protected ProductServiceBase()
        //{
        //    _serviceList =
        //        Assembly.GetAssembly(typeof(ProductServiceBase<>)).GetTypes()
        //            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof (ProductServiceBase<T>)))
        //            .ToDictionary(
        //                type => (ProductType) type.GetProperty("Type").GetConstantValue(),
        //                type => type);
        //}

        internal static Type GetService(ProductType type)
        {
            return _serviceList[type];
        }

        public List<PaxForDisplay> ConvertToPaxForDisplay(List<Pax> pax)
        {
            return pax != null
                ? pax.Select(ConvertToPaxForDisplay).ToList()
                : null;
        }

        public PaxForDisplay ConvertToPaxForDisplay(Pax pax)
        {
            if (pax == null)
                return null;

            var name = pax.FirstName == pax.LastName 
                ? pax.LastName 
                : pax.FirstName + " " + pax.LastName;

            return new PaxForDisplay
            {
                Type = pax.Type,
                Title = pax.Title,
                Name = name,
                DateOfBirth = pax.DateOfBirth,
                Nationality = pax.Nationality,
                PassportNumber = pax.PassportNumber,
                PassportExpiryDate = pax.PassportExpiryDate,
                PassportCountry = pax.PassportCountry
            };
        }

        public List<Pax> ConvertToPax(List<PaxForDisplay> pax)
        {
            return pax != null
                ? pax.Select(ConvertToPax).ToList()
                : null;
        }
        
        public Pax ConvertToPax(PaxForDisplay pax)
        {
            if (pax == null)
                return null;

            string first, last;
            if (pax.Name == null)
            {
                first = null;
                last = null;
            }
            else
            {
                var splittedName = pax.Name.Trim().Split(' ');
                if (splittedName.Length == 1)
                {
                    first = pax.Name;
                    last = pax.Name;
                }
                else
                {
                    first = pax.Name.Substring(0, pax.Name.LastIndexOf(' '));
                    last = splittedName[splittedName.Length - 1];
                }
            }

            return new Pax
            {
                Type = pax.Type.GetValueOrDefault(),
                Title = pax.Title.GetValueOrDefault(),
                FirstName = first,
                LastName = last,
                Gender = pax.Title.GetValueOrDefault() == Title.Mister ? Gender.Male : Gender.Female,
                DateOfBirth = pax.DateOfBirth,
                Nationality = pax.Nationality,
                PassportNumber = pax.PassportNumber,
                PassportIssueDate = pax.PassportIssueDate,
                PassportExpiryDate = pax.PassportExpiryDate,
                PassportCountry = pax.PassportCountry
            };
        }
    }
}
