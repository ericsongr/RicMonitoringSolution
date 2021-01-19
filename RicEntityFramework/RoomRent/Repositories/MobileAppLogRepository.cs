using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class MobileAppLogRepository : EntityBaseRepository<MobileAppLog>, IMobileAppLogRepository
    {
        public MobileAppLogRepository(RicDbContext context) : base(context)
        {
        }
    }
}
