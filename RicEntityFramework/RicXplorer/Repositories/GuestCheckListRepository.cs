using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class GuestCheckListRepository : EntityBaseRepository<GuestCheckList>, IGuestCheckListRepository
    {
        public GuestCheckListRepository(RicDbContext context) : base(context)
        {
        }
    }
}
