using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RoomRent.Models
{
    public class RentTransactionForCreateDto : RentTransaction
    {
        public int RentArrearId { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal PreviousUnpaidAmount { get; set; }
    }
}
