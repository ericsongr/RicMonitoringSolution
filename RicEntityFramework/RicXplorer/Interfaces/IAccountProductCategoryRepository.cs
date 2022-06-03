using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.RicXplorer.Repositories;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Interfaces 
{
    public interface IAccountProductCategoryRepository : IEntityBaseRepository<AccountProductCategory>
    {
        IQueryable<AccountProductCategory> FindAll();
    }
}
