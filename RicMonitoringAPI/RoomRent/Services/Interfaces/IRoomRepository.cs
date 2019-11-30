using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Services.Interfaces
{
    public interface IRoomRepository : IEntityBaseRepository<Room>
    {
        PagedList<Room> GetRooms(RoomResourceParameters roomResourceParameters);
    }
}
