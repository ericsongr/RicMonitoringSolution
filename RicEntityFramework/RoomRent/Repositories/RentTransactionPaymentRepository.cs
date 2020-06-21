using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RentTransactionPaymentRepository : EntityBaseRepository<RentTransactionPayment>, IRentTransactionPaymentRepository
    {
        public RentTransactionPaymentRepository(RicDbContext context) : base(context)
        {}
    }
}
