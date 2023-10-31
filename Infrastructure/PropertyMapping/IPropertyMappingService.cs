using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PropertyMapping
{
    public interface IPropertyMappingService
    {
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
