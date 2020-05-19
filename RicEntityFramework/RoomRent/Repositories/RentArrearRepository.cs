using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RentArrearRepository : EntityBaseRepository<RentArrear>, IRentArrearRepository
    {
        private readonly RicDbContext _context;
        //private readonly IRentTransactionPropertyMappingService _propertyMappingService;

        public RentArrearRepository(
            RicDbContext context
            //, IRentTransactionPropertyMappingService propertyMappingService
            ) : base(context)
        {
            _context = context;
            //_propertyMappingService = propertyMappingService;
        }

       }
}
