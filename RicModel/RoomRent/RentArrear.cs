using System;
using System.Collections.Generic;

namespace RicModel.RoomRent
{
    public class RentArrear 
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public int RentTransactionId { get; set; }
        public decimal UnpaidAmount { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime? ProcessedDateTime { get; set; }
        public string Note { get; set; }
        public bool IsManualEntry { get; set; }
        public DateTime? ManualEntryDateTime { get; set; }
        
        public virtual RentTransaction RentTransaction { get; set; }
        public virtual Renter Renter { get; set; }

        public ICollection<RentTransactionDetail> RentTransactionDetails { get; set; }
    }
}
