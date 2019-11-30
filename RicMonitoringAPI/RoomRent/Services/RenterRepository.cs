using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Entities;
using System.Linq;

namespace RicMonitoringAPI.Services.RenterRent
{
    public class RenterRepository : EntityBaseRepository<Renter>, IRenterRepository
    {
        private new readonly RoomRentContext _context;
        private readonly IRenterPropertyMappingService _propertyMappingService;

        public RenterRepository(
            RoomRentContext context,
            IRenterPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public PagedList<Renter> GetRenters(RenterResourceParameters RenterResourceParameters)
        {
            var collectionBeforPaging =
                _context.Renters.ApplySort(RenterResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<RenterDto, Renter>());


            if (!string.IsNullOrEmpty(RenterResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    RenterResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<Renter>.Create(collectionBeforPaging,
                RenterResourceParameters.PageNumber,
                RenterResourceParameters.PageSize);
        }
    }
}
