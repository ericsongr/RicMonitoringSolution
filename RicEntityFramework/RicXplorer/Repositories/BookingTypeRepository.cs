using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class BookingTypeRepository : EntityBaseRepository<BookingType>, IBookingTypeRepository
    {
        public BookingTypeRepository(RicDbContext context) : base(context)
        {
        }
    }
}
