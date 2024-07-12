using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class BookingTypeInclusionRepository : EntityBaseRepository<BookingTypeInclusion>, IBookingTypeInclusionRepository
    {
        public BookingTypeInclusionRepository(RicDbContext context) : base(context)
        {
        }

        public IQueryable<BookingTypeInclusion> FindAll()
        {
            return Context.BookingTypeInclusions;
        }
    }
}
