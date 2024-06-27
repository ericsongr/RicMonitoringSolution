using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class CheckListForCheckInOutGuestRepository : EntityBaseRepository<CheckListForCheckInOutGuest>, ICheckListForCheckInOutGuestRepository
    {
        public CheckListForCheckInOutGuestRepository(RicDbContext context) : base(context)
        {
        }

        public IQueryable<CheckListForCheckInOutGuest> FindAll()
        {
            return Context.CheckListForCheckInOutGuests;
        }
    }
}
