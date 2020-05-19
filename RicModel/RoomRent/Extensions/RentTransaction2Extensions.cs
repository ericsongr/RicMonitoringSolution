using System;
using RicModel.RoomRent;

namespace RicMonitoringAPI.RoomRent.Helpers.Extensions
{
    public static class RentTransaction2Extensions
    {
        public static DateTime GetDueDate(this RentTransaction2 rentTransaction2) {
            if (rentTransaction2 == null)
            {
                throw new ArgumentNullException("source");
            }
            var day = 0;
            var year = rentTransaction2.Year;
            var month = rentTransaction2.Month;

            var lastDayInMonth = DateTime.DaysInMonth(year, month);
            //check if the payDay is less than or equal the current month day to avoid error
            if (rentTransaction2.DueDay > lastDayInMonth)
                day = lastDayInMonth;
            else
                day = rentTransaction2.DueDay;

            var dueDate = new DateTime(rentTransaction2.Year, rentTransaction2.Month, day);

            return dueDate;
        }

        public static string GetPeriod(this RentTransaction2 rentTransaction2)
        {
            if (rentTransaction2 == null)
            {
                throw new ArgumentNullException("source");
            }

            var day = 0;
            var year = rentTransaction2.Year;
            var month = rentTransaction2.Month;

            var lastDayInMonth = DateTime.DaysInMonth(year, month);
            //check if the payDay is less than or equal the current month day to avoid error
            if (rentTransaction2.DueDay > lastDayInMonth)
                day = lastDayInMonth;
            else
                day = rentTransaction2.DueDay;

            var dateFrom = new DateTime(rentTransaction2.Year, rentTransaction2.Month, day).AddDays(1);
            var dateTo = dateFrom.AddMonths(1).AddDays(-1);

            return $"{dateFrom.ToString("dd-MMM")} to {dateTo.ToString("dd-MMM-yyyy")}";
        }
    }
}
