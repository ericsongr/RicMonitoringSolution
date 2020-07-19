using System;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces.IAudits;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings.IAudits;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.Repositories.Audits
{
    public class AuditRentTransactionRepository : EntityBaseRepository<AuditRentTransaction>, IAuditRentTransactionRepository
    {
        private readonly RicDbContext _context;
        private readonly IAuditRentTransactionPropertyMappingService _auditRentTransactionPropertyMappingService;

        public AuditRentTransactionRepository(
            RicDbContext context
            , IAuditRentTransactionPropertyMappingService propertyMappingService
            ) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _auditRentTransactionPropertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
       
    }
}
