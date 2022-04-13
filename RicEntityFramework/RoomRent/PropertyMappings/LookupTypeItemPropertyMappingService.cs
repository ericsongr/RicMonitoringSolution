using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
{
    public class LookupTypeItemPropertyMappingService : PropertyMappingService, ILookupTypeItemPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _lookupTypeItemPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Description", new PropertyMappingValue(new List<string>() {"Description"}) },
                {"IsActive", new PropertyMappingValue(new List<string>() {"IsActive"}) },
                {"LookupTypeId", new PropertyMappingValue(new List<string>() {"LookupTypeId"}) },
            };

        public LookupTypeItemPropertyMappingService()
        {
            base.Add(new PropertyMapping<LookupTypeItemDto, LookupTypeItem>(_lookupTypeItemPropertyMapping));
        }

    }

}
