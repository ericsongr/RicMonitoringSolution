using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.PropertyMappings
{
    public class AccountPropertyMappingService : PropertyMappingService, IAccountPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _accountPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                {"Timezone", new PropertyMappingValue(new List<string>() {"Timezone"}) },
                {"IsActive", new PropertyMappingValue(new List<string>() {"IsActive"}) },
                {"Street", new PropertyMappingValue(new List<string>() {"Street"}) },
                {"SubUrb", new PropertyMappingValue(new List<string>() {"SubUrb"}) },
                {"State", new PropertyMappingValue(new List<string>() {"State"}) },
                {"PostalCode", new PropertyMappingValue(new List<string>() {"PostalCode"}) },
                {"Email", new PropertyMappingValue(new List<string>() {"Email"}) },
                {"PhoneNumber", new PropertyMappingValue(new List<string>() {"PhoneNumber"}) },
                {"WebsiteUrl", new PropertyMappingValue(new List<string>() {"WebsiteUrl"}) },
                {"FacebookUrl", new PropertyMappingValue(new List<string>() {"FacebookUrl"}) },
                {"AddressLine1", new PropertyMappingValue(new List<string>() {"AddressLine1"}) },
                {"City", new PropertyMappingValue(new List<string>() {"City"}) },
                {"DialingCode", new PropertyMappingValue(new List<string>() {"DialingCode"}) },
                {"BusinessOwnerName", new PropertyMappingValue(new List<string>() {"BusinessOwnerName"}) },
                {"BusinessOwnerPhoneNumber", new PropertyMappingValue(new List<string>() {"BusinessOwnerPhoneNumber"}) },
                {"BusinessOwnerEmail", new PropertyMappingValue(new List<string>() {"BusinessOwnerEmail"}) },
                {"GeoCoordinates", new PropertyMappingValue(new List<string>() {"GeoCoordinates"}) },
                {"CompanyFeeFailedPaymentCount", new PropertyMappingValue(new List<string>() {"CompanyFeeFailedPaymentCount"}) },
                {"PaymentIssueSuspensionDate", new PropertyMappingValue(new List<string>() {"PaymentIssueSuspensionDate"}) },
                {"IsSelected", new PropertyMappingValue(new List<string>() {"IsSelected"}) },
            };

        public AccountPropertyMappingService()
        {
            base.Add(new PropertyMapping<AccountDto, Account>(_accountPropertyMapping));
        }

    }

}
