using RicModel.ToolsInventory;
using System;
using System.Linq;

namespace RicEntityFramework.ToolsInventory.Interfaces 
{
    public interface IToolRepository
    {
        IQueryable<Tool> FindAll();
        IQueryable<Tool> Find(DateTime startDate, DateTime endDate);
        Tool FindDetail(int id);
        Tool Find(int id);
        int Save(Tool tool);
        void Update(Tool tool);
    }
}
