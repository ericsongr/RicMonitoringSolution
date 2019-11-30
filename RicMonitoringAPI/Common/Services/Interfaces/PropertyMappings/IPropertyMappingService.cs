using RicMonitoringAPI.Api.Services.PropertyMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.Api.Services.Interfaces.PropertyMappings
{
    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
