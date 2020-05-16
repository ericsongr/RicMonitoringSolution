using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using RicMonitoringAPI.Services;

namespace RicMonitoringAPI.RoomRent.Services
{
    public class MonthlyRentBatchRepository : EntityBaseRepository<MonthlyRentBatch>, IMonthlyRentBatchRepository
    {
        public MonthlyRentBatchRepository(RoomRentContext context) : base(context)
        { }
    }


}
