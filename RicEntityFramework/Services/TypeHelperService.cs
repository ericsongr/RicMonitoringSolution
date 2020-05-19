﻿using System.Reflection;
using RicEntityFramework.Interfaces;

namespace RicEntityFramework.Services
{
    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            //the field are separated by ",", so we split it
            var fieldAfterSplit = fields.Split(',');

            //check if the requestd field exist on source
            foreach (var field in fieldAfterSplit)
            {
                //trim each field, as it might contain leading
                //or trailing spaces. can't trim the var in foreach,
                //so use another var
                var propertyName = field.Trim();

                //use reflection to check if the property can be
                //found on T.
                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                //it can't be found, return false
                if (propertyInfo == null)
                {
                    return false;
                }
            }
            //all checks out, return true
            return true;
        }
    }
}
