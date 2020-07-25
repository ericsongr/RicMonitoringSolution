using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos.Audits;

namespace RicEntityFramework.RoomRent.PropertyMappings.Audits
{
    public class AuditRoomPropertyMappingService : PropertyMappingService, IAuditRoomPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _auditRoomPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"AuditRoomId", new PropertyMappingValue(new List<string>() {"AuditRoomId"}) },
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                {"Frequency", new PropertyMappingValue(new List<string>() {"Frequency"}) },
                {"Price", new PropertyMappingValue(new List<string>() {"Price"}) },
                {"AuditDateTimeString", new PropertyMappingValue(new List<string>() {"AuditDateTimeString"}) },
                {"Username", new PropertyMappingValue(new List<string>() {"Username"}) },
                {"AuditAction", new PropertyMappingValue(new List<string>() {"AuditAction"}) },
            };

        public AuditRoomPropertyMappingService()
        {
            base.Add(new PropertyMapping<AuditRoomDto, AuditRoom>(_auditRoomPropertyMapping));
        }

    }

}
