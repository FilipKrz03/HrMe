using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IPropertyMapping { }
    public class PropertyMapping<TSource , TDestination> : IPropertyMapping
    {
        public Dictionary<string , PropertyMappingValue> MapingDictionary {  get; set; }

        public PropertyMapping(Dictionary<string , PropertyMappingValue> mappingDictionary)
        {
            MapingDictionary = mappingDictionary;
        }
    }
}
