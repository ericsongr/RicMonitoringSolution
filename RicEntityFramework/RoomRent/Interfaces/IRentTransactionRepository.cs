using System;
using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IRentTransactionRepository : IEntityBaseRepository<RentTransaction>
    {
        IQueryable<RentTransaction2> GetTransactionQueryResult(DateTime selectedDate, int renterId = 0);

        PagedList<RentTransaction2> GetRentTransactions(
            RentTransactionResourceParameters rentTransactionResourceParameters);
    }
}
