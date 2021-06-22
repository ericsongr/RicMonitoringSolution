using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class SmsGatewayRepository : EntityBaseRepository<SmsGateway>, ISmsGatewayRepository
    {
        private readonly RicDbContext _context;

        public SmsGatewayRepository(
            RicDbContext context
            ) : base(context)
        {
            _context = context;
        }

    }
}
