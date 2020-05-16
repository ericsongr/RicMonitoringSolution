using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RicMonitoringAPI.Common.Enumeration;

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

        public IQueryable<RentTransaction2> GetTransactionQueryResult(DateTime selectedDate, int renterId = 0)
        {
            var renters = _context.Renters.Where(o => !o.IsEndRent)
                           .Select(o => new
                           {
                               RenterId = o.Id,
                               RenterName = o.Name,
                               RoomId = o.RoomId,
                               RoomName = o.Room.Name,
                               MonthlyRent = o.Room.Price,
                               DueDay = o.DueDay,
                               AdvanceMonths = o.AdvanceMonths,
                               MonthsUsed = o.MonthsUsed
                           });

            if (renterId > 0)
            {
                renters = renters.Where(o => o.RenterId == renterId);
            }

            var transactions = (from r in renters
                                join t in _context.RentTransactions
                                on new { r.RenterId, selectedDate.Month, selectedDate.Year } equals new { t.RenterId, t.DueDate.Month, t.DueDate.Year }
                                into rentTrans
                                from trans in rentTrans.DefaultIfEmpty()
                                join arrear in _context.RentArrears.Where(o=> !o.IsProcessed) on r.RenterId equals arrear.RenterId
                                into arrearTempTable
                                from arrearTable in arrearTempTable.DefaultIfEmpty()
                                
                                select new RentTransaction2
                                {
                                    Id = trans == null ? 0 : trans.Id,
                                    RenterId = r.RenterId,
                                    RenterName = r.RenterName,
                                    RoomId = r.RoomId,
                                    RoomName = r.RoomName,
                                    MonthlyRent = r.MonthlyRent,
                                    DueDay = r.DueDay,
                                    PaidDate = trans.PaidDate,
                                    PaidAmount = trans.PaidAmount == null ? 0 : trans.PaidAmount + trans.AdjustmentBalancePaymentDueAmount,
                                    Balance = trans.Balance == null ? 0 : (trans.Balance - trans.AdjustmentBalancePaymentDueAmount),
                                    IsBalanceEditable = trans.Balance != null && trans.Balance > 0,
                                    RentArrearId = (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.Id : 0),
                                    PreviousUnpaidAmount =
                                        trans != null && trans.IsProcessed ?
                                            ((trans.Balance ?? 0) - trans.AdjustmentBalancePaymentDueAmount) : 
                                            (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.UnpaidAmount : 0 ),
                                    TotalAmountDue = 
                                        trans != null && trans.IsProcessed ? 
                                            trans.TotalAmountDue : 
                                                (r.MonthlyRent + (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.UnpaidAmount : 0)),
                                    IsDepositUsed = trans == null ? false : trans.IsDepositUsed,
                                    BalanceDateToBePaid = trans.BalanceDateToBePaid == null ? null : trans.BalanceDateToBePaid,
                                    Note = trans == null ? "" : trans.Note,
                                    Month = selectedDate.Month,
                                    Year = selectedDate.Year,
                                    TransactionType = trans == null ? TransactionTypeEnum.MonthlyRent : trans.TransactionType,
                                    IsNoAdvanceDepositLeft = r.MonthsUsed >= r.AdvanceMonths,
                                    IsProcessed = trans == null ? false : trans.IsProcessed,
                                    AdjustmentBalancePaymentDueAmount = trans == null ? 0 : trans.AdjustmentBalancePaymentDueAmount
                                });

            return transactions;
        }

        public PagedList<RentTransaction2> GetRentTransactions(RentTransactionResourceParameters rentTransactionResourceParameters)
        {
            

            var selectedDate = DateTime.Now; //.AddMonths(1);


            var transactions = GetTransactionQueryResult(selectedDate);
            var collectionBeforPaging =
                transactions.ApplySort(rentTransactionResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<RentTransaction2Dto, RentTransaction2>());

            if (!string.IsNullOrEmpty(rentTransactionResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    rentTransactionResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.RenterName.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<RentTransaction2>.Create(collectionBeforPaging,
                rentTransactionResourceParameters.PageNumber,
                rentTransactionResourceParameters.PageSize);
        }
    }
}
