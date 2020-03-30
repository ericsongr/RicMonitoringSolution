using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RoomRent.Models
{
    public class RenterForUpdateDto : Renter
    {
        public string StartDateInput { get; set; }
        public string AdvancePaidDateInput { get; set; }
    }
}
