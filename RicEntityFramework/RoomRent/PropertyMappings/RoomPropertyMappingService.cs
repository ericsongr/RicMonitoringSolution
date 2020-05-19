using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
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
                {"IsOccupied", new PropertyMappingValue(new List<string>() {"IsOccupied"}) },
            };

        public RoomPropertyMappingService()
        {
            base.Add(new PropertyMapping<RoomDto, Room>(_roomPropertyMapping));
        }

    }

}
