using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Inc.Interfaces;
using RicModel.Inc;

namespace RicEntityFramework.Inc.Repositories
{
    public class IncBuklodRepository : RepositoryBase, IIncBuklodRepository
    {
        public IncBuklodRepository(RicDbContext context) : base(context)
        { }

        public IQueryable<IncBuklod> FindAll()
        {
            var query = (from t in Context.IncBuklod select t);
            return query;
        }


        public IncBuklod Find(int id)
        {
            return FindAll()
                .FirstOrDefault(o => o.Id == id);
        }

        public int Save(IncBuklod tool)
        {
            Context.IncBuklod.Add(tool);
            Context.SaveChanges();

            return tool.Id;
        }

        public void Update(IncBuklod tool)
        {
            Context.IncBuklod.Update(tool);
            Context.SaveChanges();
        }
    }
}
