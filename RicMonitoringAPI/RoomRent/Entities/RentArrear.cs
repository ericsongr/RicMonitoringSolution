using System.Collections.Generic;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RentArrear : IEntityBase
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public int RentTransactionId { get; set; }
        public decimal UnpaidAmount { get; set; }
        public bool IsProcessed { get; set; }

        public virtual RentTransaction RentTransaction { get; set; }
        public virtual Renter Renter { get; set; }

        public ICollection<RentTransactionDetail> RentTransactionDetails { get; set; }
    }
}
