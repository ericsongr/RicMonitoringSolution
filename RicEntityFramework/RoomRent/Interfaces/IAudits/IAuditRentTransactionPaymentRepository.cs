

using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.Interfaces.IAudits
{
    public interface IAuditRentTransactionPaymentRepository : IEntityBaseRepository<AuditRentTransactionPayment>
    {
    }
}
