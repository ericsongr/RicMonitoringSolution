using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public bool IsOccupied { get; set; }

        public virtual ICollection<Renter> Renters { get; set; }
        public virtual ICollection<RentTransaction> RentTransactions { get; set; }

        //audit tables
        public virtual ICollection<AuditRenter> AuditRenters { get; set; }
        public virtual ICollection<AuditRoom> AuditRooms { get; set; }
        public virtual ICollection<AuditRentTransaction> AuditRentTransactions { get; set; }


    }
}
