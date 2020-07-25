using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent.Dtos.Audits
{
    public class AuditRoomDto : AuditRoom
    {
        public string AuditDateTimeString => AuditDateTime.ToString("dd-MMM-yyyy hh:mm tt");
    }
}
