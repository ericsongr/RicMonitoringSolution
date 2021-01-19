using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Data.Persistence.Repositories.BaseRepository;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Data.Persistence.Repositories
{
    public class RegisteredDeviceRepository : EntityBaseRepository<RegisteredDevice>, IRegisteredDeviceRepository
    {
        public RegisteredDeviceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
