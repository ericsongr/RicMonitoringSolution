using System;
using System.Collections.Generic;
using System.Linq;
using RicMonitoringAPI.Api.Services.Interfaces.PropertyMappings;

namespace RicMonitoringAPI.Api.Services.PropertyMappings
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public void Add(IPropertyMapping propertyMapping)
        {
            propertyMappings.Add(propertyMapping);
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            //get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

             throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");

        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }

            // the string is separated by "," , so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the field clauses
            foreach (var field in fieldsAfterSplit)
            {
                //trim
                var trimmedField = field.Trim();

                //remove everything after the firts " " - if the fields
                //are coming from an orderBy string, this part musb be 
                //ignore
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedField
                    : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            return true;

        }
    }

}
