using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IRenterRepository : IEntityBaseRepository<Renter>
    {
        PagedList<Renter> GetRenters(RenterResourceParameters renterResourceParameters);
    }
}
