using RicMonitoringAPI.Api.Services.PropertyMappings;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace RicMonitoringAPI.RoomRent.Services.PropertyMappings
{
    public class LookupTypePropertyMappingService : PropertyMappingService, ILookupTypePropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _LookupTypePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
            };

        public LookupTypePropertyMappingService()
        {
            base.Add(new PropertyMapping<LookupTypeDto, LookupType>(_LookupTypePropertyMapping));
        }

    }

}
