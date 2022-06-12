using System;

namespace RicModel.RicXplorer.Dtos
{
    public class GuestBookedSchedule
    {
        public int TotalGuestsBooked { get; set; }
        public DateTime BookedDate { get; set; }
        public bool IsFullyBooked { get; set; }
    }
}
