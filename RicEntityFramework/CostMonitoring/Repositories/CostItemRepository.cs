using RicEntityFramework.BaseRepository;
using RicEntityFramework.CostMonitoring.Interfaces;
using RicModel.CostMonitoring;

namespace RicEntityFramework.CostMonitoring.Repositories
{
    public class CostItemRepository : EntityBaseRepository<CostItem>, ICostItemRepository
    {
        public CostItemRepository(RicDbContext context) : base(context)
        {
        }
    }
}
