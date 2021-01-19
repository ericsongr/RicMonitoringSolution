using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Data.Persistence.Repositories.BaseRepository;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Data.Persistence.Repositories
{
    public class RefreshTokenRepository : EntityBaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
