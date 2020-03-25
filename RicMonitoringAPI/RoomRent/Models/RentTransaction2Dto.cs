using System;
using RicMonitoringAPI.RoomRent.Entities;

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
    }
}
