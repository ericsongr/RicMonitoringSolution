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
using RicModel.RoomRent.Extensions;

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

        public IQueryable<RentTransaction2> GetTransaction(int id)
        {
            var transactions = _context.RentTransactions
                                    .Where(o => o.Id == id)
                                    .Include(o => o.Renter)
                                    .ThenInclude(o => o.RentArrears)
                                    .Include(o => o.Room)
                                    .Include(o => o.RentTransactionPayments)
                                    .Select(t => new RentTransaction2
                                    {
                                        Id = t.Id,
                                        RenterId = t.RenterId,
                                        RenterName = t.Renter.Name,
                                        RoomId = t.RoomId,
                                        RoomName = t.Room.Name,
                                        MonthlyRent = t.Room.Price,
                                        DueDay = t.Renter.DueDay,
                                        PaidDate = t.PaidDate,
                                        PaidAmount = t.PaidAmount,
                                        Balance = t.Balance,
                                        RentArrearId = 
                                            t.Renter.RentArrears.Any(o => !o.IsProcessed) ?
                                                t.Renter.RentArrears.Where(o => !o.IsProcessed).First().Id : 0,
                                        PreviousUnpaidAmount = 
                                            t.Renter.RentArrears.Any(o => !o.IsProcessed) ? 
                                                t.Renter.RentArrears
                                                    .Where(o => !o.IsProcessed).First().UnpaidAmount : 0,
                                        TotalAmountDue = t.TotalAmountDue,
                                        IsDepositUsed = t.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed),
                                        BalanceDateToBePaid = t.BalanceDateToBePaid,
                                        Note = t.Note,
                                        Month = t.DueDate.Month,
                                        Year = t.DueDate.Year,
                                        TransactionType = t.TransactionType,
                                        IsNoAdvanceDepositLeft = t.Renter.MonthsUsed >= t.Renter.AdvanceMonths,
                                        IsProcessed = t.IsProcessed,
                                        RentTransactionPayments = t.RentTransactionPayments
                                    });

            return transactions;
        }

        public IQueryable<RentTransaction2> GetAllTransactions()
        {
            var transactions = _context.RentTransactions
                                    .Where(o => !o.IsProcessed)
                                    .Include(o => o.Renter)
                                    .Include(o => o.Room)
                                    .Include(o => o.RentTransactionPayments)
                                    .Select(t => new RentTransaction2
                                    {
                                        Id = t.Id,
                                        RenterId = t.RenterId,
                                        RenterName = t.Renter.Name,
                                        RoomId = t.RoomId,
                                        RoomName = t.Room.Name,
                                        MonthlyRent = t.Room.Price,
                                        DueDay = t.Renter.DueDay,
                                        PaidDate = t.PaidDate,
                                        PaidAmount = t.PaidAmount,
                                        Balance = t.Balance,
                                        PreviousUnpaidAmount =
                                            t.Renter.RentArrears.Any(o => !o.IsProcessed) ?
                                                t.Renter.RentArrears
                                                    .Where(o => !o.IsProcessed).First().UnpaidAmount : 0,
                                        TotalAmountDue = t.TotalAmountDue,
                                        IsDepositUsed = t.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed),
                                        BalanceDateToBePaid = t.BalanceDateToBePaid,
                                        Note = t.Note,
                                        Month = t.DueDate.Month,
                                        Year = t.DueDate.Year,
                                        TransactionType = t.TransactionType,
                                        IsNoAdvanceDepositLeft = t.Renter.MonthsUsed >= t.Renter.AdvanceMonths,
                                        IsProcessed = t.IsProcessed
                                    });

            return transactions;
        }

        public PagedList<RentTransaction2> GetRentTransactions(RentTransactionResourceParameters rentTransactionResourceParameters)
        {
            var transactions = GetAllTransactions();
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
