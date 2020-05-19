using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;

namespace RicEntityFramework.Interfaces.PropertyMappings
{
    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
