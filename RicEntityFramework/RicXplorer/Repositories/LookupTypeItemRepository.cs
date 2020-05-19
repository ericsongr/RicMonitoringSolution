using RicEntityFramework;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent;

namespace RicMonitoringAPI.RicXplorer.Repository
{
    public class LookupTypeItemRepository : EntityBaseRepository<LookupType>, ILookupTypeItemRepository
    {
        public LookupTypeItemRepository(RicDbContext context) : base(context)
        {
        }
    }
}
