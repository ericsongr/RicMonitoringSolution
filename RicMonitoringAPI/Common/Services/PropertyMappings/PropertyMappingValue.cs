using System.Collections.Generic;

namespace RicMonitoringAPI.Api.Services.PropertyMappings
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool IsRevert { get; private set; }

        public PropertyMappingValue(
            IEnumerable<string> destinationProperties,
            bool isRevert = false)
        {
            DestinationProperties = destinationProperties;
            IsRevert = isRevert;
        }
    }
}
