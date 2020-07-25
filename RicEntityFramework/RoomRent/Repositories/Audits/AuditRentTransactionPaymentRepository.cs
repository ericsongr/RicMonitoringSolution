using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces.IAudits;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.Repositories.Audits
{
    public class AuditRentTransactionPaymentRepository : EntityBaseRepository<AuditRentTransactionPayment>, IAuditRentTransactionPaymentRepository
    {
        public AuditRentTransactionPaymentRepository(RicDbContext context) : base(context)
        {}
    }
}
