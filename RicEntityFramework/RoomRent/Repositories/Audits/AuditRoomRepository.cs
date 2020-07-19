using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Helpers;
using RicEntityFramework.RoomRent.Interfaces.IAudits;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos.Audits;

namespace RicEntityFramework.RoomRent.Repositories.Audits
{
    public class AuditRoomRepository : EntityBaseRepository<AuditRoom>, IAuditRoomRepository
    {
        private readonly RicDbContext _context;
        private readonly IAuditRoomPropertyMappingService _auditRoomPropertyMappingService;

        public AuditRoomRepository(
            RicDbContext context
            , IAuditRoomPropertyMappingService auditRoomPropertyMappingService
            ) : base(context)
        {
            _context = context;
            _auditRoomPropertyMappingService = auditRoomPropertyMappingService;
        }

        public PagedList<AuditRoom> GetAuditRooms(BaseResourceParameters auditRoomResourceParameters)
        {
            var collectionBeforPaging =
                _context.AuditRooms.ApplySort(
                    auditRoomResourceParameters.OrderBy,
                        _auditRoomPropertyMappingService.GetPropertyMapping<AuditRoomDto, AuditRoom>());


            if (!string.IsNullOrEmpty(auditRoomResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    auditRoomResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }


            var roomIds = _context.Renters.Where(o => !o.IsEndRent).Select(o => o.RoomId);

            return PagedList<AuditRoom>.Create(collectionBeforPaging,
                auditRoomResourceParameters.PageNumber,
                auditRoomResourceParameters.PageSize);
        }
    }
}
