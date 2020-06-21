using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Constants;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.Enumeration;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Enumerations;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RentTransactionRepository : EntityBaseRepository<RentTransaction>, IRentTransactionRepository
    {
        private readonly RicDbContext _context;
        private readonly IRentTransactionPropertyMappingService _propertyMappingService;

        public RentTransactionRepository(
            RicDbContext context
            , IRentTransactionPropertyMappingService propertyMappingService
            ) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public IQueryable<RentTransaction2> GetTransaction(string monthFilter, int renterId)
        {
            var selectedDate = DateTime.Now;
            if (monthFilter == RentTransactionMonthFilterConstant.Current)
            {
                selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            else if (monthFilter == RentTransactionMonthFilterConstant.Previous)
            {
                selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            }

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

            var transactions = (from r in renters
                                join t in _context.RentTransactions
                                        .Include(o => o.RentTransactionPayments)
                                on new { r.RenterId, selectedDate.Month, selectedDate.Year } equals new { t.RenterId, t.DueDate.Month, t.DueDate.Year }
                                into rentTrans
                                from trans in rentTrans.DefaultIfEmpty()
                                join arrear in _context.RentArrears.Where(o=> !o.IsProcessed) on r.RenterId equals arrear.RenterId
                                into arrearTempTable
                                from arrearTable in arrearTempTable.DefaultIfEmpty()
                                where r.RenterId == renterId
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
                                    PaidAmount = trans.PaidAmount == null ? 0 : trans.PaidAmount,
                                    Balance = trans.Balance == null ? 0 : trans.Balance,
                                    RentArrearId = (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.Id : 0),
                                    PreviousUnpaidAmount =
                                        trans != null && trans.IsProcessed ?
                                            (trans.Balance ?? 0) : 
                                            (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.UnpaidAmount : 0 ),
                                    TotalAmountDue = 
                                        trans != null && trans.IsProcessed ? 
                                            trans.TotalAmountDue : 
                                                (r.MonthlyRent + (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.UnpaidAmount : 0)),
                                    IsDepositUsed = trans == null ? false : 
                                        trans.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed),
                                    BalanceDateToBePaid = trans.BalanceDateToBePaid == null ? null : trans.BalanceDateToBePaid,
                                    Note = trans == null ? "" : trans.Note,
                                    Month = selectedDate.Month,
                                    Year = selectedDate.Year,
                                    TransactionType = trans == null ? TransactionTypeEnum.MonthlyRent : trans.TransactionType,
                                    IsNoAdvanceDepositLeft = r.MonthsUsed >= r.AdvanceMonths,
                                    IsProcessed = trans == null ? false : trans.IsProcessed,
                                    RentTransactionPayments = trans.RentTransactionPayments
                                });

            return transactions;
        }

        public IQueryable<RentTransaction2> GetAllTransactions(string monthFilter)
        {
            var selectedDate = DateTime.Now;
            if (monthFilter == RentTransactionMonthFilterConstant.Current)
            {
                selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            else if (monthFilter == RentTransactionMonthFilterConstant.Previous)
            {
                selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            }

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

            var transactions = (from r in renters
                                join t in _context.RentTransactions
                                        .Include(o => o.RentTransactionPayments)
                                on new { r.RenterId, selectedDate.Month, selectedDate.Year } equals new { t.RenterId, t.DueDate.Month, t.DueDate.Year }
                                into rentTrans
                                from trans in rentTrans.DefaultIfEmpty()
                                join arrear in _context.RentArrears.Where(o => !o.IsProcessed) on r.RenterId equals arrear.RenterId
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
                                    PaidAmount = trans.PaidAmount == null ? 0 : trans.PaidAmount,
                                    Balance = trans.Balance == null ? 0 : trans.Balance,
                                    RentArrearId = (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.Id : 0),
                                    PreviousUnpaidAmount =
                                        trans != null && trans.IsProcessed ?
                                            (trans.Balance ?? 0) :
                                            (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.UnpaidAmount : 0),
                                    TotalAmountDue =
                                        trans != null && trans.IsProcessed ?
                                            trans.TotalAmountDue :
                                                (r.MonthlyRent + (arrearTable != null && !arrearTable.IsProcessed ? arrearTable.UnpaidAmount : 0)),
                                    IsDepositUsed = trans == null ? false :
                                        trans.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed),
                                    BalanceDateToBePaid = trans.BalanceDateToBePaid == null ? null : trans.BalanceDateToBePaid,
                                    Note = trans == null ? "" : trans.Note,
                                    Month = selectedDate.Month,
                                    Year = selectedDate.Year,
                                    TransactionType = trans == null ? TransactionTypeEnum.MonthlyRent : trans.TransactionType,
                                    IsNoAdvanceDepositLeft = r.MonthsUsed >= r.AdvanceMonths,
                                    IsProcessed = trans == null ? false : trans.IsProcessed
                                });

            return transactions;
        }

        public PagedList<RentTransaction2> GetRentTransactions(RentTransactionResourceParameters rentTransactionResourceParameters)
        {
            var transactions = GetAllTransactions(rentTransactionResourceParameters.MonthFilter);
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
