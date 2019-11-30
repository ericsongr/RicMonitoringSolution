using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Services.Interfaces
{
    public interface IRenterRepository : IEntityBaseRepository<Renter>
    {
        PagedList<Renter> GetRenters(RenterResourceParameters renterResourceParameters);
    }
}
