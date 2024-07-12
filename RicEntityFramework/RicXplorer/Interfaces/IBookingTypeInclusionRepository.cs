using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Interfaces 
{
    public interface IBookingTypeInclusionRepository : IEntityBaseRepository<BookingTypeInclusion>
    {
        IQueryable<BookingTypeInclusion> FindAll();
    }
}
