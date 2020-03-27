using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Services.Interfaces;

namespace RicMonitoringAPI.Services.RoomRent
{
    public class RentArrearRepository : EntityBaseRepository<RentArrear>, IRentArrearRepository
    {
        private new readonly RoomRentContext _context;
        private readonly IRentTransactionPropertyMappingService _propertyMappingService;

        public RentArrearRepository(
            RoomRentContext context,
            IRentTransactionPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

       }
}
