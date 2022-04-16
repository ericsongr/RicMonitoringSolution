using System;
using System.Collections.Generic;
using RicEntityFramework.PropertyMappings;
using RicEntityFramework.RicXplorer.Interfaces.IPropertyMappings;
using RicModel.RicXplorer;
using RicModel.RicXplorer.Dtos;

namespace RicEntityFramework.RicXplorer.PropertyMappings
{
    public class BookingTypePropertyMappingService : PropertyMappingService, IBookingTypePropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _bookingTypePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"Name", new PropertyMappingValue(new List<string>() {"Name"}) },
                {"Image", new PropertyMappingValue(new List<string>() {"Image"}) },
                {"Price", new PropertyMappingValue(new List<string>() {"Price"}) },
                {"IsActive", new PropertyMappingValue(new List<string>() {"IsActive"}) },
                {"UtcDateTimeCreated", new PropertyMappingValue(new List<string>() {"UtcDateTimeCreated"}) },
                {"UtcDateTimeUpdated", new PropertyMappingValue(new List<string>() {"UtcDateTimeUpdated"}) },
            };

        public BookingTypePropertyMappingService()
        {
            base.Add(new PropertyMapping<BookingTypeDto, BookingType>(_bookingTypePropertyMapping));
        }

    }

}
