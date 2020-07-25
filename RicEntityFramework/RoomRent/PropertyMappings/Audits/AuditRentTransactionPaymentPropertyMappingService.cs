using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos.Audits;

namespace RicEntityFramework.RoomRent.PropertyMappings.Audits
{
    public class AuditRentTransactionPaymentPropertyMappingService : PropertyMappingService, IAuditRentTransactionPaymentPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _roomPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"AuditRentTransactionPaymentId", new PropertyMappingValue(new List<string>() {"AuditRentTransactionPaymentId"}) },
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Amount", new PropertyMappingValue(new List<string>() {"Amount"}) },
                {"DatePaidString", new PropertyMappingValue(new List<string>() {"DatePaidString"}) },
                {"PaymentTransactionType", new PropertyMappingValue(new List<string>() {"PaymentTransactionType"}) },
                {"AuditDateTimeString", new PropertyMappingValue(new List<string>() {"AuditDateTimeString"}) },
                {"Username", new PropertyMappingValue(new List<string>() {"Username"}) },
                {"AuditAction", new PropertyMappingValue(new List<string>() {"AuditAction"}) },
                {"IsDeleted", new PropertyMappingValue(new List<string>() {"IsDeleted"}) },
            };

        public AuditRentTransactionPaymentPropertyMappingService()
        {
            base.Add(new PropertyMapping<AuditRentTransactionPaymentDto, AuditRentTransactionPayment>(_roomPropertyMapping));
        }

    }

}
