using Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class IQueraybleExtensions
    {
        public static IQueryable<T> ApplySort<T>
            (this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> dictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            string orderByString = string.Empty;

            var trimmedOredBy = orderBy.Split(",");

            foreach (var orderByClause in trimmedOredBy)
            {
                string trimmedOredByCaluse = orderByClause.Trim();

                bool isDescending = trimmedOredByCaluse.EndsWith(" desc");

                int indexOfFirstWhiteSpace = trimmedOredByCaluse.IndexOf(" ");

                var propertyName = indexOfFirstWhiteSpace == -1 ? trimmedOredByCaluse
                    : trimmedOredByCaluse.Remove(indexOfFirstWhiteSpace);


                var dictionaryValues = dictionary[propertyName];

                if (dictionaryValues == null)
                {
                    throw new Exception($"Could not find property {propertyName}");
                }

                foreach (var value in dictionaryValues.DestinationProperties)
                {
                    orderByString = 
                        orderByString + 
                        (string.IsNullOrEmpty(orderByString) ? string.Empty : ", ") +
                        value +
                        (isDescending ? " descending" : " ascending");
                }
            }
            return source.OrderBy(orderByString);
        }
    }
}
