using RicEntityFramework;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RentTransactionDetailRepository : EntityBaseRepository<RentTransactionDetail>, IRentTransactionDetailRepository
    {
        private readonly RicDbContext _context;
        //private readonly IRentTransactionPropertyMappingService _propertyMappingService;

        public RentTransactionDetailRepository(
            RicDbContext context
            //, IRentTransactionPropertyMappingService propertyMappingService
            ) : base(context)
        {
            _context = context;
            //_propertyMappingService = propertyMappingService;
        }

       }
}
