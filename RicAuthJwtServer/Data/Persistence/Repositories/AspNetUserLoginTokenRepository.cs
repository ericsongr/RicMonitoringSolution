using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Data.Persistence.Repositories.BaseRepository;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Data.Persistence.Repositories
{
    public class AspNetUserLoginTokenRepository : EntityBaseRepository<AspNetUserLoginToken>, IAspNetUserLoginTokenRepository
    {
        public AspNetUserLoginTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
