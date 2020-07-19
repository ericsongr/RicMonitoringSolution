using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicModel.RoomRent.Audits;


namespace RicEntityFramework.RoomRent.Interfaces.IAudits
{
    public interface IAuditRoomRepository : IEntityBaseRepository<AuditRoom>
    {
        PagedList<AuditRoom> GetAuditRooms(BaseResourceParameters auditRoomResourceParameters);
    }
}
