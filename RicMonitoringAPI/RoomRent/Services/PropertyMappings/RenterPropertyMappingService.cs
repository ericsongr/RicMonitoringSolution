using RicMonitoringAPI.Api.Services.PropertyMappings;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace RicMonitoringAPI.RoomRent.Services.PropertyMappings
{
    public class RenterPropertyMappingService : PropertyMappingService, IRenterPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _renterPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                {"AdvanceMonths", new PropertyMappingValue(new List<string>() {"AdvanceMonths"}) },
                {"MonthsUsed", new PropertyMappingValue(new List<string>() {"MonthsUsed"}) },
                {"AdvancePaidDate", new PropertyMappingValue(new List<string>() {"AdvancePaidDate"}) },
                {"StartDate", new PropertyMappingValue(new List<string>() {"StartDate"}) },
                {"DueDate", new PropertyMappingValue(new List<string>() {"DueDate"}) },
                {"NoOfPersons", new PropertyMappingValue(new List<string>() {"NoOfPersons"}) },
                {"IsEndRent", new PropertyMappingValue(new List<string>() {"IsEndRent"}) },
                {"DateEndRent", new PropertyMappingValue(new List<string>() {"DateEndRent"}) },
            };

        public RenterPropertyMappingService()
        {
            base.Add(new PropertyMapping<RenterDto, Renter>(_renterPropertyMapping));
        }

    }

}
