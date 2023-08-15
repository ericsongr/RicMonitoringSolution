using RicEntityFramework.BaseRepository;
using RicEntityFramework.CostMonitoring.Interfaces;
using RicModel.CostMonitoring;

namespace RicEntityFramework.CostMonitoring.Repositories
{
    public class TransactionCostRepository : EntityBaseRepository<TransactionCost>, ITransactionCostRepository
    {
        public TransactionCostRepository(RicDbContext context) : base(context)
        {
        }
    }
}
