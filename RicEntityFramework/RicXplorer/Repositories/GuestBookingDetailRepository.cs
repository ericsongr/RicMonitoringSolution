using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class GuestBookingDetailRepository : EntityBaseRepository<GuestBookingDetail>, IGuestBookingDetailRepository
    {
        public GuestBookingDetailRepository(RicDbContext context) : base(context)
        {
        }
    }
}
