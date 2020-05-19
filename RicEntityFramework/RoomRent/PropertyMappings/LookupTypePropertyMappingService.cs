using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
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
