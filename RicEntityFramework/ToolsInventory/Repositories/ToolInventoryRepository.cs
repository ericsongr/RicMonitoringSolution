using RicEntityFramework.BaseRepository;
using RicEntityFramework.ToolsInventory.Interfaces;
using RicModel.ToolsInventory;

namespace RicEntityFramework.ToolsInventory.Repositories
{
    public class ToolInventoryRepository : EntityBaseRepository<ToolInventory>, IToolInventoryRepository
    {
        public ToolInventoryRepository(RicDbContext context) : base(context)
        { }
    }
}
