
using System;
using RicModel.RoomRent.Constants;

namespace RicModel.RoomRent.Extensions
{
    public static class SettingExtensions
    {
        public static string GetDataType(this Setting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException("source");
            }

            var isInt = int.TryParse(setting.Value, out int numericValue);
            if (isInt) return DataTypeConstant.Numeric;
            
            var isBool = bool.TryParse(setting.Value, out bool boolValue);
            if (isBool) return DataTypeConstant.Boolean;

            var isDateTime = DateTime.TryParse(setting.Value, out DateTime dateTimeValue);
            if (isDateTime) return DataTypeConstant.Datetime;

            return DataTypeConstant.Varchar;
        }

        public static object GetRealValue(this Setting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException("source");
            }

            var isInt = int.TryParse(setting.Value, out int numericValue);
            if (isInt) return numericValue;

            var isBool = bool.TryParse(setting.Value, out bool boolValue);
            if (isBool) return boolValue;

            var isDateTime = DateTime.TryParse(setting.Value, out DateTime dateTimeValue);
            if (isDateTime) return dateTimeValue;

            return setting.Value;
        }
    }
}
