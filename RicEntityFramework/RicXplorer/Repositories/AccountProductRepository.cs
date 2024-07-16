using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class AccountProductRepository : EntityBaseRepository<AccountProduct>, IAccountProductRepository
    {
        public AccountProductRepository(RicDbContext context) : base(context)
        {
        }
        
    }
}
