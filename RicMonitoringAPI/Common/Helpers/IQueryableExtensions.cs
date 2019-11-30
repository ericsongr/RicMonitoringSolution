﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using RicMonitoringAPI.Api.Services.PropertyMappings;

namespace RicMonitoringAPI.Api.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                return source;
            }

            // the orderby separated by "," so we split it.
            var orderByAfterSplit = orderBy.Split(',');

            //apply each order clause in reverse order - otherwise the 
            //IQueryable will be ordered in wrong order

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {

                //trim the orderByClause, as it might contain heading
                //or trailing spaces. Can't trim the var in foreach,
                //so use another var.
                var trimmedOrderByClause = orderByClause.Trim();

                //if the sort options end with " desc", we order
                //descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from the orderByClause, so we
                // get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"Key mapping for {propertyName} is missing.");
                }

                //get the property value
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                //Run through the property names in reverse
                //so the orderby clause are apply in the correct order.
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    // revert sort order if necessary
                    if (propertyMappingValue.IsRevert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;

        }
    }
}
