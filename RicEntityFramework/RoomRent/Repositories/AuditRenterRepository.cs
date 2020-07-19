using System.Linq;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Helpers;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Dtos.Audits;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class AuditRenterRepository : EntityBaseRepository<AuditRenter>, IAuditRenterRepository
    {
        private readonly RicDbContext _context;
        private readonly IRenterPropertyMappingService _propertyMappingService;

        public AuditRenterRepository(
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

        public PagedList<AuditRenter> GetRenters(BaseResourceParameters auditRenterResourceParameters)
        {
            var collectionBeforPaging =
                _context.AuditRenters.ApplySort(auditRenterResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<AuditRenterDto, AuditRenter>());


            if (!string.IsNullOrEmpty(auditRenterResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    auditRenterResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<AuditRenter>.Create(collectionBeforPaging,
                auditRenterResourceParameters.PageNumber,
                auditRenterResourceParameters.PageSize);
        }
    }
}
