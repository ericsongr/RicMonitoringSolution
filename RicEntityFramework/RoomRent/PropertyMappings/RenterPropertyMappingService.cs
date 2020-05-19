using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
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
                {"DueDay", new PropertyMappingValue(new List<string>() {"DueDay"}) },
                {"DueDayString", new PropertyMappingValue(new List<string>() {"DueDayString"}) },
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
