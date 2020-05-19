using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class MonthlyRentBatchRepository : EntityBaseRepository<MonthlyRentBatch>, IMonthlyRentBatchRepository
    {
        public MonthlyRentBatchRepository(RicDbContext context) : base(context)
        { }
    }


}
