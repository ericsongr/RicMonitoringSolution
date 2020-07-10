using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
{
    public class RentTransactionHistoryPropertyMappingService: PropertyMappingService, IRentTransactionHistoryPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _historyPropertyMapping = 
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"DueDate", new PropertyMappingValue(new List<string>() {"DueDate"}, isRevert:true) },
                {"DueDateString", new PropertyMappingValue(new List<string>() {"DueDateString"}) },
                {"PaidOrUsedDepositDateString", new PropertyMappingValue(new List<string>() {"PaidOrUsedDepositDateString"}) },
                {"Period", new PropertyMappingValue(new List<string>() {"Period"}) },
                {"PaidAmount", new PropertyMappingValue(new List<string>() {"PaidAmount"}) },
                {"BalanceDateToBePaidString", new PropertyMappingValue(new List<string>() {"BalanceDateToBePaidString"}) },
                {"MonthlyRent", new PropertyMappingValue(new List<string>() {"MonthlyRent"}) },
                {"PreviousBalance", new PropertyMappingValue(new List<string>() {"PreviousBalance"}) },
                {"CurrentBalance", new PropertyMappingValue(new List<string>() {"CurrentBalance"}) },
                {"TotalAmountDue", new PropertyMappingValue(new List<string>() {"TotalAmountDue"}) },
                {"IsDepositUsed", new PropertyMappingValue(new List<string>() {"IsDepositUsed"}) },
                {"Note", new PropertyMappingValue(new List<string>() {"Note"}) },
                {"TransactionType", new PropertyMappingValue(new List<string>() {"TransactionType"}) },
            };

        public RentTransactionHistoryPropertyMappingService()
        {
            base.Add(new PropertyMapping<RentTransactionHistoryDto, RentTransaction>(_historyPropertyMapping));
        }
    }
}
