using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Services.Interfaces;

namespace RicMonitoringAPI.Services.RoomRent
{
    public class RentTransactionDetailRepository : EntityBaseRepository<RentTransactionDetail>, IRentTransactionDetailRepository
    {
        private new readonly RoomRentContext _context;
        private readonly IRentTransactionPropertyMappingService _propertyMappingService;

        public RentTransactionDetailRepository(
            RoomRentContext context,
            IRentTransactionPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

       }
}
