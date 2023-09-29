using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.ToolsInventory.Interfaces;
using RicModel.ToolsInventory;

namespace RicEntityFramework.ToolsInventory.Repositories
{
    public class ToolRepository : RepositoryBase, IToolRepository
    {
        public ToolRepository(RicDbContext context) : base(context)
        { }

        public IQueryable<Tool> FindAll()
        {
            var query = (from t in Context.Tools select t);
            return query;
        }

        public IQueryable<Tool> Find(DateTime startDate, DateTime endDate)
        {
            return FindAll()
                .Include(o => o.ToolsInventory)
                .ThenInclude(o => o.LookupTypeItemAction)
                .Include(o => o.ToolsInventory)
                .ThenInclude(o => o.LookupTypeItemStatus)
                .AsNoTracking()
                .Where(o => o.CreatedDateTimeUtc.Date >= startDate && o.CreatedDateTimeUtc.Date <= endDate);
        }

        public Tool FindDetail(int id)
        {
            return FindAll()
                .Include(o => o.ToolsInventory)
                .ThenInclude(o => o.LookupTypeItemAction)
                .Include(o => o.ToolsInventory)
                .ThenInclude(o => o.LookupTypeItemStatus)
                .AsNoTracking()
                .FirstOrDefault(o => o.Id == id);
        }

        public Tool Find(int id)
        {
            return FindAll()
                .Include(o => o.ToolsInventory)
                .FirstOrDefault(o => o.Id == id);
        }

        public int Save(Tool tool)
        {
            Context.Tools.Add(tool);
            Context.SaveChanges();

            return tool.Id;
        }

        public void Update(Tool tool)
        {
            Context.Tools.Update(tool);
            Context.SaveChanges();
        }
    }
}
