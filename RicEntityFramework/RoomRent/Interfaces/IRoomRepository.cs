using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicModel.RoomRent;


namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IRoomRepository : IEntityBaseRepository<Room>
    {
        PagedList<Room> GetRooms(RoomResourceParameters roomResourceParameters);
    }
}
