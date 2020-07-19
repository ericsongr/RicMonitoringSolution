using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos.Audits;

namespace RicEntityFramework.RoomRent.PropertyMappings.Audits
{
    public class AuditRenterPropertyMappingService : PropertyMappingService, IAuditRenterPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _renterPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                {"AdvancePaidDateString", new PropertyMappingValue(new List<string>() {"AdvancePaidDateString"}) },
                {"StartDateString", new PropertyMappingValue(new List<string>() {"StartDateString"}) },
                {"DueDayString", new PropertyMappingValue(new List<string>() {"DueDayString"}) },
                {"NoOfPersons", new PropertyMappingValue(new List<string>() {"NoOfPersons"}) },
                {"RoomName", new PropertyMappingValue(new List<string>() {"RoomName"}) },
                {"AdvanceMonths", new PropertyMappingValue(new List<string>() {"AdvanceMonths"}) },
                {"MonthsUsed", new PropertyMappingValue(new List<string>() {"MonthsUsed"}) },
                {"DateEndString", new PropertyMappingValue(new List<string>() {"DateEndString"}) },
                {"IsEndRent", new PropertyMappingValue(new List<string>() {"IsEndRent"}) },
                {"BalanceAmount", new PropertyMappingValue(new List<string>() {"BalanceAmount"}) },
                {"BalancePaidDateString", new PropertyMappingValue(new List<string>() {"BalancePaidDateString"}) },
                {"TotalPaidAmount", new PropertyMappingValue(new List<string>() {"TotalPaidAmount"}) },
                {"NextDueDateString", new PropertyMappingValue(new List<string>() {"NextDueDateString"}) },
                {"PreviousDueDateString", new PropertyMappingValue(new List<string>() {"PreviousDueDateString"}) },
                {"AuditDateTimeString", new PropertyMappingValue(new List<string>() {"AuditDateTimeString"}) },
                {"Username", new PropertyMappingValue(new List<string>() {"Username"}) },
                {"AuditAction", new PropertyMappingValue(new List<string>() {"AuditAction"}) },
            };

        public AuditRenterPropertyMappingService()
        {
            base.Add(new PropertyMapping<AuditRenterDto, AuditRenter>(_renterPropertyMapping));
        }

    }

}
