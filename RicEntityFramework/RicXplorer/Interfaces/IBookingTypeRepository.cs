using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Interfaces 
{
    public interface IBookingTypeRepository : IEntityBaseRepository<BookingType>
    {
        IQueryable<BookingType> FindAll();
    }
}
