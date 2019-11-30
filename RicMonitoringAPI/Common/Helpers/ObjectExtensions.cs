using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace RicMonitoringAPI.Api.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(
            this TSource source,
            string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // create an ExpandoObject that will hold the
            //selected properties & values
            var dataShapedObject = new ExpandoObject();

            //all fields
            if (string.IsNullOrWhiteSpace(fields))
            {
                //all public properties should be in ExpandoObject
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Get the value of each property we have to return. for that,
                //we run through the list
                foreach (var propertyInfo in propertyInfos)
                {
                    //GetValue returns the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(source);

                    //add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);

                }
            }
            else
            {
                //selected fields
                //only the public properties that match the fields should be
                //in the ExpandoObjects

                //the field are separated by ",", so we split it
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    //trim each field, as it might contain leading
                    //or trailing spaces. Can't trim the var in foreach,
                    //so use another var
                    var propertyName = field.Trim();

                    //use reflection to get the property on the source object
                    //we need to include public and instance, b/c specifying a binding flag overwrites the
                    //already-existing binding flags.
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    }

                    //GetValue returns the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(source);

                    //add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
            }


            // return the list
            return dataShapedObject;
        }
    }
}
