using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces.IAudits;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.Repositories.Audits
{
    public class AuditAccountRepository : EntityBaseRepository<AuditAccount>, IAuditAccountRepository
    {
        public AuditAccountRepository(RicDbContext context) : base(context)
        { }
    }
}
