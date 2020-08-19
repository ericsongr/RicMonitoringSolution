using System;

namespace RicModel.RoomRent
{
    public class MonthlyRentBatch
    {
        public int Id { get; set; }
        public DateTime ProcessStartDateTime { get; set; }
        public DateTime? ProcesssEndDateTime { get; set; }
    }
}
