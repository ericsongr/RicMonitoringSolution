using RicMonitoringAPI.RoomRent.Entities;
using System;

namespace RicMonitoringAPI.RoomRent.Helpers.Extensions
{
    public static class RentTransaction2Extensions
    {
        public static DateTime GetDueDate(this RentTransaction2 rentTransaction2) {
            if (rentTransaction2 == null)
            {
                throw new ArgumentNullException("source");
            }

            var day = rentTransaction2.DueDay;
            var dueDate = new DateTime(rentTransaction2.Year, rentTransaction2.Month, day);

            return dueDate;
        }

        public static string GetPeriod(this RentTransaction2 rentTransaction2)
        {
            if (rentTransaction2 == null)
            {
                throw new ArgumentNullException("source");
            }

            var day = rentTransaction2.DueDay;
            var dateFrom = new DateTime(rentTransaction2.Year, rentTransaction2.Month, day).AddDays(1);
            var dateTo = dateFrom.AddMonths(1).AddDays(-1);

            return $"{dateFrom.ToString("dd-MMM")} to {dateTo.ToString("dd-MMM-yyyy")}";
        }
    }
}
