using System;

namespace RicModel.RoomRent
{
    public class MonthlyRentBatch
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime ProcessStartDateTime { get; set; }
        public DateTime? ProcesssEndDateTime { get; set; }
    }
}
