using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.Interfaces.IAudits
{
    public interface IAuditRenterRepository : IEntityBaseRepository<AuditRenter>
    {
        PagedList<AuditRenter> GetAuditRenters(BaseResourceParameters renterResourceParameters);
    }
}
