using RicEntityFramework;
using RicEntityFramework.BaseRepository;
using RicModel.RoomRent;
using RicMonitoringAPI.RicXplorer.Services.Interfaces;

namespace RicMonitoringAPI.RicXplorer.Services
{
    public class LookupTypeItemRepository : EntityBaseRepository<LookupType>, ILookupTypeItemRepository
    {
        public LookupTypeItemRepository(RicDbContext context) : base(context)
        {
        }
    }
}
