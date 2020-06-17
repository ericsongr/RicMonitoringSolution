
using System;
using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent
{
    public class AuditRoom : IAudit
    {
        public int AuditRoomId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public decimal Price { get; set; }

        public DateTime AuditDateTime { get; set; }
        public string Username { get; set; }
        public string AuditAction { get; set; }

        public virtual Room Room { get; set; }
    }
}
