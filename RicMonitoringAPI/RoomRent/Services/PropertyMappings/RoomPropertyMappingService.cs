using RicMonitoringAPI.Api.Services.PropertyMappings;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace RicMonitoringAPI.RoomRent.Services.PropertyMappings
{
    public class RoomPropertyMappingService : PropertyMappingService, IRoomPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _roomPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                {"Frequency", new PropertyMappingValue(new List<string>() {"Frequency"}) },
                {"Price", new PropertyMappingValue(new List<string>() {"Price"}) },
            };

        public RoomPropertyMappingService()
        {
            base.Add(new PropertyMapping<RoomDto, Room>(_roomPropertyMapping));
        }

    }

}
