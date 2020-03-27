using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RentTransactionDetail: IEntityBase
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public int? RentArrearId { get; set; }

        public virtual RentTransaction RentTransaction { get; set; }
        public virtual RentArrear RentArrear { get; set; }

    }
}
