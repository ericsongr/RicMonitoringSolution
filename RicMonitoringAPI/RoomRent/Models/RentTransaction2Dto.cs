using System;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Containers;

namespace RicMonitoringAPI.RoomRent.Models
{
    public class RentTransaction2Dto : RentTransaction2
    {
        public DateTime DueDate { get; set; }
        public string DueDateString
        {
            get { return DueDate.ToString("dd-MMM-yyyy"); }
        }

        public string DatePaidString {
            get { return PaidDate?.ToString("dd-MMM-yyyy"); }

        }
        public string Period { get; set; }

        public BillingStatement BillingStatement =>
            new BillingStatement
            {
                RenterName = RenterName,
                RoomName = RoomName,
                Period = Period,
                DueDate = DueDateString,
                MonthlyRent = MonthlyRent.ToString("#,###"),
                HasPreviousBalance = PreviousUnpaidAmount > 0,
                PreviousUnpaidAmount = PreviousUnpaidAmount.ToString("#,###"),
                TotalAmountDue = TotalAmountDue.ToString("#,###")
            };

    }
}
