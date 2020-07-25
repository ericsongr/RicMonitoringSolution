using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Dtos.Audits;

namespace RicEntityFramework.RoomRent.PropertyMappings.Audits
{
    public class AuditRentTransactionPropertyMappingService : PropertyMappingService, IAuditRentTransactionPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _renterPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"AuditRentTransactionId", new PropertyMappingValue(new List<string>() {"AuditRentTransactionId"}) },
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"PaidDateString", new PropertyMappingValue(new List<string>() {"PaidDateString"}) },
                {"PaidAmount", new PropertyMappingValue(new List<string>() {"PaidAmount"}) },
                {"Balance", new PropertyMappingValue(new List<string>() {"Balance"}) },
                {"BalanceDateToBePaidString", new PropertyMappingValue(new List<string>() {"BalanceDateToBePaidString"}) },
                {"IsDepositUsed", new PropertyMappingValue(new List<string>() {"IsDepositUsed"}) },
                {"Note", new PropertyMappingValue(new List<string>() {"Note"}) },
                {"RoomName", new PropertyMappingValue(new List<string>() {"RoomName"}) },
                {"RenterName", new PropertyMappingValue(new List<string>() {"RenterName"}) },
                {"DueDateString", new PropertyMappingValue(new List<string>() {"DueDateString"}) },
                {"Period", new PropertyMappingValue(new List<string>() {"Period"}) },
                {"TransactionType", new PropertyMappingValue(new List<string>() {"TransactionType"}) },
                {"IsSystemProcessed", new PropertyMappingValue(new List<string>() {"IsSystemProcessed"}) },
                {"SystemDateTimeProcessedString", new PropertyMappingValue(new List<string>() {"SystemDateTimeProcessedString"}) },
                {"TotalAmountDue", new PropertyMappingValue(new List<string>() {"TotalAmountDue"}) },
                {"IsProcessed", new PropertyMappingValue(new List<string>() {"IsProcessed"}) },
                {"ExcessPaidAmount", new PropertyMappingValue(new List<string>() {"ExcessPaidAmount"}) },
                {"AuditDateTimeString", new PropertyMappingValue(new List<string>() {"AuditDateTimeString"}) },
                {"Username", new PropertyMappingValue(new List<string>() {"Username"}) },
                {"AuditAction", new PropertyMappingValue(new List<string>() {"AuditAction"}) }
            };

        public AuditRentTransactionPropertyMappingService()
        {
            base.Add(new PropertyMapping<AuditRentTransactionDto, AuditRentTransaction>(_renterPropertyMapping));
        }

    }

}
