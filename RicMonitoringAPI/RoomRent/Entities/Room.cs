using RicMonitoringAPI.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class Room : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public decimal Price { get; set; }

        public ICollection<Renter> Renters { get; set; }
        public ICollection<RentTransaction> RentTransactions { get; set; }
    }
}
