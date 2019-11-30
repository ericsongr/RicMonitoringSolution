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

            var day = rentTransaction2.DueDate.Day;
            var lastDateOfTheMonth = (new DateTime(rentTransaction2.Year, rentTransaction2.Month, 1)).AddMonths(1).AddDays(-1).Day;
            //if day is greater than the last date of the month it should get the last day of the month
            if (day > lastDateOfTheMonth)
            {
                day = lastDateOfTheMonth;
            }

            var dueDate = new DateTime(rentTransaction2.Year, rentTransaction2.Month, day);

            return dueDate;
        }
    }
}
