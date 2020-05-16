using System;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class MonthlyRentBatch : IEntityBase
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime ProcessStartDateTime { get; set; }
        public DateTime? ProcesssEndDateTime { get; set; }
    }
}
