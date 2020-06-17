using System;

namespace RicModel.RoomRent.Audits
{
    public interface IAudit
    {
        DateTime AuditDateTime { get; set; }
        string Username { get; set; }
        string AuditAction { get; set; }
    }
}
