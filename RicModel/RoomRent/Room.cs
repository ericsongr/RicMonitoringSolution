using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RicModel.RoomRent
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public bool IsOccupied { get; set; }

        public ICollection<Renter> Renters { get; set; }
        public ICollection<RentTransaction> RentTransactions { get; set; }
    }
}
