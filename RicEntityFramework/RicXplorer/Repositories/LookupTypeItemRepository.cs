using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class LookupTypeItemRepository : EntityBaseRepository<LookupType>, ILookupTypeItemRepository
    {
        public LookupTypeItemRepository(RicDbContext context) : base(context)
        {
        }
    }
}
