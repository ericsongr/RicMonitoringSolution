using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class AccountProductCategoryRepository : EntityBaseRepository<AccountProductCategory>, IAccountProductCategoryRepository
    {
        public AccountProductCategoryRepository(RicDbContext context) : base(context)
        {
        }

        public IQueryable<AccountProductCategory> FindAll()
        {
            return Context.AccountProductCategories;
        }
    }
}
