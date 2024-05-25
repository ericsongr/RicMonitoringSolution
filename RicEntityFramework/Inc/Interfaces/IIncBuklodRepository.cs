using RicModel.ToolsInventory;
using System;
using System.Linq;
using RicModel.Inc;

namespace RicEntityFramework.Inc.Interfaces 
{
    public interface IIncBuklodRepository
    {
        IQueryable<IncBuklod> FindAll();
        IncBuklod Find(int id);
        int Save(IncBuklod tool);
        void Update(IncBuklod tool);
    }
}
