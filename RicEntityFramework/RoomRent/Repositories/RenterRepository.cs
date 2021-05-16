using System.Linq;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RenterRepository : EntityBaseRepository<Renter>, IRenterRepository
    {
        private readonly RicDbContext _context;
        private readonly IRenterPropertyMappingService _propertyMappingService;

        public RenterRepository(
            RicDbContext context
            , IRenterPropertyMappingService propertyMappingService
            ) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public DbSet<Renter> Renters()
        {
            return _context.Renters;
        }

        public PagedList<Renter> GetRenters(RenterResourceParameters renterResourceParameters)
        {
            var collectionBeforPaging =
                _context.Renters
                    .Where(o => o.Room.AccountId == renterResourceParameters.AccountId)
                    .ApplySort(renterResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<RenterDto, Renter>());


            collectionBeforPaging = collectionBeforPaging
                .Where(a => !a.IsEndRent);

            if (!string.IsNullOrEmpty(renterResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    renterResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<Renter>.Create(collectionBeforPaging,
                renterResourceParameters.PageNumber,
                renterResourceParameters.PageSize);
        }
    }
}
