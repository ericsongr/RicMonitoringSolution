using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
{
    public class RentTransactionPropertyMappingService : PropertyMappingService, IRentTransactionPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _renterPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"RenterId", new PropertyMappingValue(new List<string>() {"RenterId"}) },
                {"RenterName", new PropertyMappingValue(new List<string>() {"RenterName"}) },
                {"RoomId", new PropertyMappingValue(new List<string>() {"RoomId"}) },
                {"RoomName", new PropertyMappingValue(new List<string>() {"RoomName"}) },
                {"MonthlyRent", new PropertyMappingValue(new List<string>() {"MonthlyRent"}) },
                {"PaidDate", new PropertyMappingValue(new List<string>() {"PaidDate"}) },
                {"DatePaidString", new PropertyMappingValue(new List<string>() {"DatePaidString"}) },
                {"PaidAmount", new PropertyMappingValue(new List<string>() {"PaidAmount"}) },
                {"Balance", new PropertyMappingValue(new List<string>() {"Balance"}) },
                {"BalanceDateToBePaid", new PropertyMappingValue(new List<string>() {"BalanceDateToBePaid"}) },
                {"TotalAmountDue", new PropertyMappingValue(new List<string>() {"TotalAmountDue"}) },
                {"PreviousUnpaidAmount", new PropertyMappingValue(new List<string>() {"PreviousUnpaidAmount"}) },
                {"RentArrearId", new PropertyMappingValue(new List<string>() {"RentArrearId"}) },
                {"IsDepositUsed", new PropertyMappingValue(new List<string>() {"IsDepositUsed"}) },
                {"Note", new PropertyMappingValue(new List<string>() {"Note"}) },
                {"DueDate", new PropertyMappingValue(new List<string>() {"DueDate"}) },
                {"DueDateString", new PropertyMappingValue(new List<string>() {"DueDateString"}) },
                {"DueDay", new PropertyMappingValue(new List<string>() {"DueDay"}) },
                {"Period", new PropertyMappingValue(new List<string>() {"Period"}) },
                {"TransactionType", new PropertyMappingValue(new List<string>() {"TransactionType"}) },
                {"IsNoAdvanceDepositLeft", new PropertyMappingValue(new List<string>() {"IsNoAdvanceDepositLeft"}) },
                {"IsProcessed", new PropertyMappingValue(new List<string>() {"IsProcessed"}) },
                {"IsBalanceEditable", new PropertyMappingValue(new List<string>() {"IsBalanceEditable"}) },
                {"BillingStatement", new PropertyMappingValue(new List<string>() {"BillingStatement"}) },
            };

        public RentTransactionPropertyMappingService()
        {
            base.Add(new PropertyMapping<RentTransaction2Dto, RentTransaction2>(_renterPropertyMapping));
        }

    }

}
