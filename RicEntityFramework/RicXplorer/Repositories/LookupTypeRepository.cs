using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class LookupTypeRepository : EntityBaseRepository<LookupType>, ILookupTypeRepository
    {
        public LookupTypeRepository(RicDbContext context) : base(context)
        {
        }
    }
}
