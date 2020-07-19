using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IAuditRenterRepository : IEntityBaseRepository<AuditRenter>
    {
        PagedList<AuditRenter> GetRenters(BaseResourceParameters renterResourceParameters);
    }
}
