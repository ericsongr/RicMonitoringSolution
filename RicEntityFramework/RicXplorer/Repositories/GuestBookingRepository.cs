using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class GuestBookingRepository : EntityBaseRepository<GuestBooking>, IGuestBookingRepository
    {
        public GuestBookingRepository(RicDbContext context) : base(context)
        {
        }
    }
}
