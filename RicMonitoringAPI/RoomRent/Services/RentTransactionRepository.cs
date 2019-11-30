using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;

namespace RicMonitoringAPI.Services.RoomRent
{
    public class RentTransactionRepository : EntityBaseRepository<RentTransaction>, IRentTransactionRepository
    {
        private new readonly RoomRentContext _context;
        private readonly IRentTransactionPropertyMappingService _propertyMappingService;

        public RentTransactionRepository(
            RoomRentContext context,
            IRentTransactionPropertyMappingService propertyMappingService) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public PagedList<RentTransaction2> GetRentTransactions(RentTransactionResourceParameters rentTransactionResourceParameters)
        {
            

            var currentDate = DateTime.Now;

            var renters = _context.Renters
                            .Select(o => new
                            {
                                RenterId = o.Id,
                                Renter = o.Name,
                                RoomId = o.RoomId,
                                Room = o.Room.Name,
                                MonthlyRent = o.Room.Price,
                                o.DueDate,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                            });

            var transactions = (from R in renters
                                join T in _context.RentTransactions
                                on new { R.RenterId, R.Month, R.Year } equals new { T.RenterId , T.DueDate.Month, T.DueDate.Year }
                                into rentTrans
                                from trans in rentTrans.DefaultIfEmpty()
                                select new RentTransaction2 {
                                    Id = trans.Id == null ? 0 : trans.Id,
                                    RenterId = R.RenterId,
                                    Renter = R.Renter,
                                    RoomId = R.RoomId,
                                    Room = R.Room,
                                    MonthlyRent = R.MonthlyRent,
                                    DueDate = R.DueDate,
                                    PaidDate = trans.PaidDate  == null ? null : trans.PaidDate.ToShortDateString(),
                                    Amount = trans.Amount == null ? 0 : trans.Amount,
                                    Balance = trans.Balance == null ? 0 : trans.Balance,
                                    IsDepositUsed = trans.IsDepositUsed == null ? false : trans.IsDepositUsed,
                                    BalanceDateToBePaid = trans.BalanceDateToBePaid == null ? null : trans.BalanceDateToBePaid,
                                    Note = trans.Note == null ? "" : trans.Note,
                                    Month = R.Month,
                                    Year = R.Year
                                });


            var collectionBeforPaging =
                transactions.ApplySort(rentTransactionResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<RentTransaction2Dto, RentTransaction2>());


            if (!string.IsNullOrEmpty(rentTransactionResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    rentTransactionResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Renter.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<RentTransaction2>.Create(collectionBeforPaging,
                rentTransactionResourceParameters.PageNumber,
                rentTransactionResourceParameters.PageSize);
        }
    }
}
