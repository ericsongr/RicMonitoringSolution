using System;
using System.Linq;
using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Services.Interfaces
{
    public interface IRentTransactionRepository : IEntityBaseRepository<RentTransaction>
    {
        IQueryable<RentTransaction2> GetTransactionQueryResult(DateTime selectedDate, int renterId = 0);

        PagedList<RentTransaction2> GetRentTransactions(RentTransactionResourceParameters rentTransactionResourceParameters);
    }
}
