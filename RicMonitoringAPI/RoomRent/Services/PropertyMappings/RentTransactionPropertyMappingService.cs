using RicMonitoringAPI.Api.Services.PropertyMappings;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace RicMonitoringAPI.RoomRent.Services.PropertyMappings
{
    public class RentTransactionPropertyMappingService : PropertyMappingService, IRentTransactionPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _renterPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"RenterId", new PropertyMappingValue(new List<string>() {"RenterId"}) },
                {"Renter", new PropertyMappingValue(new List<string>() {"Renter"}) },
                {"RoomId", new PropertyMappingValue(new List<string>() {"RoomId"}) },
                {"Room", new PropertyMappingValue(new List<string>() {"Room"}) },
                {"PaidDate", new PropertyMappingValue(new List<string>() {"PaidDate"}) },
                {"Amount", new PropertyMappingValue(new List<string>() {"Amount"}) },
                {"Balance", new PropertyMappingValue(new List<string>() {"Balance"}) },
                {"BalanceDateToBePaid", new PropertyMappingValue(new List<string>() {"BalanceDateToBePaid"}) },
                {"IsDepositUsed", new PropertyMappingValue(new List<string>() {"IsDepositUsed"}) },
                {"Note", new PropertyMappingValue(new List<string>() {"Note"}) },
                {"DueDate", new PropertyMappingValue(new List<string>() {"DueDate"}) },
            };

        public RentTransactionPropertyMappingService()
        {
            base.Add(new PropertyMapping<RentTransaction2Dto, RentTransaction2>(_renterPropertyMapping));
        }

    }

}
