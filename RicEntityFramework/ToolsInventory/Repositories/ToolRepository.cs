using RicEntityFramework.BaseRepository;
using RicEntityFramework.ToolsInventory.Interfaces;
using RicModel.ToolsInventory;

namespace RicEntityFramework.ToolsInventory.Repositories
{
    public class ToolRepository : EntityBaseRepository<Tool>, IToolRepository
    {
        public ToolRepository(RicDbContext context) : base(context)
        { }
    }
}
