using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RicEntityFramework;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.Enumeration;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Enumerations;
using RicMonitoringAPI.Common.Constants;

namespace RicMonitoringAPI.RoomRent.Controllers
{

    [AllowAnonymous]
    //[Authorize(Policy = "ProcessTenantsTransaction")]
    [Route("api/exec-store-proc")]
    [ApiController]
    public class ExecuteBatchController : ControllerBase
    {
        private readonly RicDbContext _context;
        private readonly IMonthlyRentBatchRepository _monthlyRentBatchRepository;
        private readonly IRenterRepository _renterRepository;
        private readonly IRentTransactionPaymentRepository _rentTransactionPaymentRepository;
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRentTransactionDetailRepository _rentTransactionDetailRepository;
        private readonly IRentArrearRepository _rentArrearRepository;
        private readonly ISettingRepository _settingRepository;

        public ExecuteBatchController(
            RicDbContext context,
            IMonthlyRentBatchRepository monthlyRentBatchRepository,
            IRenterRepository renterRepository,
            IRentTransactionPaymentRepository rentTransactionPaymentRepository,
            IRentTransactionRepository rentTransactionRepository,
            IRentTransactionDetailRepository rentTransactionDetailRepository,
            IRentArrearRepository rentArrearRepository,
            ISettingRepository settingRepository
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _monthlyRentBatchRepository = monthlyRentBatchRepository ?? throw new ArgumentNullException(nameof(monthlyRentBatchRepository));
            _renterRepository = renterRepository ?? throw new ArgumentNullException(nameof(renterRepository));
            _rentTransactionPaymentRepository = rentTransactionPaymentRepository ?? throw new ArgumentNullException(nameof(rentTransactionPaymentRepository));
            _rentTransactionRepository = rentTransactionRepository ?? throw new ArgumentNullException(nameof(rentTransactionRepository));
            _rentTransactionDetailRepository = rentTransactionDetailRepository ?? throw new ArgumentNullException(nameof(rentTransactionDetailRepository));
            _rentArrearRepository = rentArrearRepository ?? throw new ArgumentNullException(nameof(rentArrearRepository));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        [HttpPost()]
        public async Task<IActionResult> ExecRentTransactionBatchFile()
        {
            var currentDate = DateTime.Now;
            var status = DailyBatchStatusConstant.Processing;

            var dailyBatchStatus = _monthlyRentBatchRepository.FindBy(o => o.ProcessStartDateTime.Date == currentDate.Date).ToList();
            if (dailyBatchStatus.Any())
            {
                var item = dailyBatchStatus.FirstOrDefault();
                if (item.ProcesssEndDateTime != null)
                {
                    status = DailyBatchStatusConstant.Processed;
                }
            }
            else
            {
                int monthlyRentBatchId = InsertRentBatch(currentDate);
                int tenantGracePeriod = GetTenantGracePeriod();
                DateTime systemDateTimeProcessed = currentDate;
                DateTime dateIncludedGracePeriod = currentDate.AddDays(-tenantGracePeriod).Date; //minus days of grace period to current date

                string note = "PROCESSED BY THE SYSTEM";

                var transactions = _rentTransactionRepository
                    .FindBy(o => !o.IsProcessed && !o.Renter.IsEndRent && o.DueDate <= dateIncludedGracePeriod, 
                        r => r.Renter, 
                                             rm => rm.Renter.Room,
                                              ar => ar.Renter.RentArrears,
                                                paid => paid.RentTransactionPayments)

                    .Select(o => new BatchRentTransactionDto
                    {
                        Id = o.Id,
                        RenterId = o.RenterId,
                        RoomId = o.RoomId,
                        MonthlyRent = o.Room.Price,
                        PaidAmount = o.PaidAmount ?? 0,
                        Balance = o.Balance,
                        ExcessPaidAmount = o.ExcessPaidAmount,
                        DueDate = o.Renter.NextDueDate,
                        HasMonthDeposit = o.Renter.MonthsUsed < o.Renter.AdvanceMonths,
                        
                        IsUsedDeposit = o.RentTransactionPayments == null ? false :
                            o.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed),

                        IsPaidTotalDueAmount = o.RentTransactionPayments == null ? false :
                            o.RentTransactionPayments.Sum(o => o.Amount) >= o.TotalAmountDue,
                        
                        Arrear = Mapper.Map<RentArrearDto>(o.RentArrears?.FirstOrDefault(o => !o.IsProcessed)) 
                    });
                
                foreach (var transaction in transactions)
                {
                    if (transaction.PaidAmount == 0)
                    {
                        var previousUnpaidBalance = (transaction.Arrear == null ? 0 : transaction.Arrear.UnpaidAmount);
                        DateTime? datePaid;

                        decimal totalAmountDue = transaction.MonthlyRent + previousUnpaidBalance;
                        decimal totalBalance = 0;
                        decimal paidBalance = 0;

                        if (transaction.HasMonthDeposit)
                        {
                            //update month used of the renter
                            var renter = _renterRepository
                                .GetSingleAsync(o => o.Id == transaction.RenterId)
                                .GetAwaiter().GetResult();
                            if (renter != null)
                            {
                                renter.MonthsUsed = renter.MonthsUsed + 1;
                                _renterRepository.Update(renter);
                                _renterRepository.Commit();
                            }

                            //add payment transaction but transaction type is "DepositUsed"
                            _rentTransactionPaymentRepository.Add(new RentTransactionPayment
                            {
                                Amount = 0,
                                DatePaid = currentDate,
                                PaymentTransactionType = PaymentTransactionType.DepositUsed,
                                RentTransactionId = transaction.Id,
                            });
                            _rentTransactionPaymentRepository.Commit();

                            //INSERT FOR USING THE DEPOSIT
                            _rentTransactionDetailRepository.Add(new RentTransactionDetail
                            {
                                TransactionId = transaction.Id,
                                Amount = 0, // make amount payment 0 because there's a deposit
                            });
                            _rentTransactionDetailRepository.Commit();
                            //END RentTransactionDetails

                            totalBalance = previousUnpaidBalance;
                            datePaid = currentDate;
                        }
                        else
                        {
                            totalBalance = totalAmountDue;
                            datePaid = null;
                        }


                        MarkTransactionAsProcessed(transaction.Id, totalBalance, note, datePaid, true);

                        //START RentTransactionDetails
                        //INSERT DATA ON TRANSACTION DETAIL
                        //INSERT PREVIOUS SAVE ARREAR
                        if (previousUnpaidBalance > 0)
                        {
                            _rentTransactionDetailRepository.Add(new RentTransactionDetail
                            {
                                TransactionId = transaction.Id,
                                Amount = previousUnpaidBalance,
                                RentArrearId = transaction.Arrear.RentArrearId
                            });
                        }
                        

                        MarkAsProcessedPreviousTotalBalance(transaction.Id);

                        MarkAsProcessedPreviousTotalBalanceManualEntry(transaction.RenterId);

                        //insert new arrear / unpaid balance
                        if (totalBalance > 0)
                        {
                            InsertNewArrear(transaction, totalBalance);
                        }

                    }
                    else if (transaction.Balance > 0)
                    {
                        //TODO: test
                        InsertNewArrear(transaction, transaction.Balance ?? 0);

                        MarkAsProcessedPreviousTotalBalance(transaction.Id);

                        MarkAsProcessedPreviousTotalBalanceManualEntry(transaction.RenterId);

                        MarkTransactionAsProcessed(transaction.Id);

                    }
                    else if (transaction.IsUsedDeposit)
                    {
                        //TODO: test
                        //previous unpaid amount
                        if (transaction.Arrear.UnpaidAmount > 0)
                        {
                            InsertNewArrear(transaction, transaction.Arrear.UnpaidAmount);
                        }

                        MarkAsProcessedPreviousTotalBalance(transaction.Id);

                        MarkAsProcessedPreviousTotalBalanceManualEntry(transaction.RenterId);

                        MarkTransactionAsProcessed(transaction.Id);
                    }
                    else if (transaction.IsPaidTotalDueAmount)
                    {

                        MarkAsProcessedAndFullyPaidArrear(transaction.Id);

                        MarkTransactionAsProcessed(transaction.Id);
                    }

                    //update due date and create new transaction
                    var updateRenter = _renterRepository
                        .GetSingleIncludesAsync(o => o.Id == transaction.RenterId, rm => rm.Room)
                        .GetAwaiter().GetResult();
                    if (updateRenter != null)
                    {
                        var previousDueDate = updateRenter.NextDueDate;
                        var nextDueDate = updateRenter.NextDueDate.AddMonths(1);

                        updateRenter.PreviousDueDate = previousDueDate;
                        updateRenter.NextDueDate = nextDueDate;
                        _renterRepository.Commit();

                        //for the next billing cycle
                        DateTime dateStart = nextDueDate.AddDays(1);
                        DateTime dateEnd = nextDueDate.AddMonths(1);
                        string period = $"{dateStart.ToString("dd-MMM")} to {dateEnd.ToString("dd-MMM-yyyy")}";

                        //add new rent transaction
                        var newTransaction = new RentTransaction
                        {
                            RenterId = updateRenter.Id,
                            RoomId = updateRenter.RoomId,
                            DueDate = nextDueDate,
                            Period = period,
                            TransactionType = TransactionTypeEnum.MonthlyRent,
                            IsProcessed = false,
                            TotalAmountDue = updateRenter.Room.Price + (transaction.Balance ?? 0)
                        };

                        //set paid amount to value of excess paid amount from previous transaction
                        newTransaction.PaidAmount = transaction.ExcessPaidAmount;

                        _rentTransactionRepository.Add(newTransaction);
                        _rentTransactionRepository.Commit();

                        //save to rent payment transaction table if there's excess payment on the previous transaction
                        if (transaction.ExcessPaidAmount > 0)
                        {
                            InsertExcessPaidAmountToRentTransactionPayment(
                                newTransaction.Id, 
                                transaction.ExcessPaidAmount,
                                systemDateTimeProcessed);
                        }
                    }

                }


                UpdateMonthlyRentBatch(monthlyRentBatchId);

                //List<SqlParameter> pc = new List<SqlParameter>()
                //{
                //    new SqlParameter("@CurrentDate", DateTime.Now)
                //};
                //await _context.Database.ExecuteSqlCommandAsync($"RentTransactionBatchFile @CurrentDate", pc.ToArray());
            }

            //Thread.Sleep(5000);

            return Ok(new {status});
        }

        /// <summary>
        /// This function use to insert excess payment amount to RentTransactionPayment table for next renter billing cycle
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="excessPaymentAmount"></param>
        /// <param name="dateIncludedGracePeriod"></param>
        private void InsertExcessPaidAmountToRentTransactionPayment(
            int transactionId,
            decimal excessPaymentAmount,
            DateTime dateIncludedGracePeriod)
        {
            _rentTransactionPaymentRepository.Add(new RentTransactionPayment
            {
                RentTransactionId = transactionId,
                Amount = excessPaymentAmount,
                DatePaid = dateIncludedGracePeriod, //date where the previous transaction mark as processed
                PaymentTransactionType = PaymentTransactionType.CarryOverExcessPayment
            });
            _rentTransactionPaymentRepository.Commit();
        }


        /// <summary>
        /// update current due date transaction detail
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="totalBalance"></param>
        /// <param name="note"></param>
        /// <param name="datePaid"></param>
        private void MarkTransactionAsProcessed(
            int transactionId, 
            decimal? totalBalance = 0, 
            string note = "", 
            DateTime? datePaid = null, 
            bool isSystemProcessed = false)
        {
            var updateTransaction = _rentTransactionRepository
                .GetSingleAsync(o => o.Id == transactionId)
                .GetAwaiter()
                .GetResult();
            
            if (updateTransaction != null)
            {
                if (totalBalance > 0)
                    updateTransaction.Balance = totalBalance;

                if (datePaid.HasValue)
                    updateTransaction.PaidDate = datePaid;

                if (!string.IsNullOrEmpty(note))
                    updateTransaction.Note = note;

                if(isSystemProcessed)
                    updateTransaction.IsSystemProcessed = true;

                updateTransaction.IsProcessed = true;
                updateTransaction.SystemDateTimeProcessed = DateTime.UtcNow;

                _rentTransactionRepository.Update(updateTransaction);
                _rentTransactionRepository.Commit();
            }
        }

        /// <summary>
        /// INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="totalBalance"></param>
        private void InsertNewArrear(BatchRentTransactionDto transaction, decimal totalBalance)
        {
            _rentArrearRepository.Add(new RentArrear
            {
                RenterId = transaction.RenterId,
                RentTransactionId = transaction.Id,
                UnpaidAmount = totalBalance,
                IsProcessed = false
            });
            _rentArrearRepository.Commit();
        }

        /// <summary>
        /// SET THE PREVIOUS TOTAL BALANCE THAT WAS MANUAL ENTRY IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private void MarkAsProcessedPreviousTotalBalanceManualEntry(int renterId)
        {
            var arrearManualEntry = _rentArrearRepository
                .GetSingleAsync(o => o.RenterId == renterId &&
                                     o.IsManualEntry).GetAwaiter().GetResult();
            if (arrearManualEntry != null)
            {
                arrearManualEntry.IsProcessed = true;
                _rentArrearRepository.Update(arrearManualEntry);
                _rentArrearRepository.Commit();
            }
        }

        /// <summary>
        /// SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private void MarkAsProcessedPreviousTotalBalance(int transactionId)
        {
            var arrearPrevious = _rentArrearRepository
                .GetSingleAsync(o => o.RentTransactionId == transactionId &&
                                     !o.IsManualEntry).GetAwaiter().GetResult();
            if (arrearPrevious != null)
            {
                arrearPrevious.IsProcessed = true;
                _rentArrearRepository.Update(arrearPrevious);
                _rentArrearRepository.Commit();
            }
        }

        /// <summary>
        /// SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private void MarkAsProcessedAndFullyPaidArrear(int transactionId)
        {
            var renterArrear = _rentTransactionDetailRepository
                .GetSingleAsync(o => o.TransactionId == transactionId && o.RentArrearId > 0)
                .GetAwaiter().GetResult();
            if (renterArrear != null)
            {
                var arrear = _rentArrearRepository
                    .GetSingleAsync(o => o.Id == renterArrear.RentArrearId).GetAwaiter().GetResult();
                if (arrear != null)
                {
                    arrear.IsProcessed = true;
                    arrear.Note = "Fully paid including arrears";

                    _rentArrearRepository.Update(arrear);
                    _rentArrearRepository.Commit();
                }
            }
            
        }

        private int GetTenantGracePeriod()
        {
            var tenantGracePeriodKey = _settingRepository
                .FindBy(o => o.Key == SettingConstant.TenantGracePeriodKey)
                .FirstOrDefault();
            if (tenantGracePeriodKey != null)
            {
                return Convert.ToInt32(tenantGracePeriodKey.Value);
            }
            return 0;
        }

        private int InsertRentBatch(DateTime currentDate)
        {
            var monthlyRentBatch = new MonthlyRentBatch
            {
                Month = currentDate.Month,
                Year = currentDate.Year,
                ProcessStartDateTime = currentDate,
                ProcesssEndDateTime = null
            };
            _monthlyRentBatchRepository.Add(monthlyRentBatch);
            _monthlyRentBatchRepository.Commit();

            return monthlyRentBatch.Id;
        }

        private void UpdateMonthlyRentBatch(int monthlyRentBatchId)
        {
            var monthlyRentBatch = _monthlyRentBatchRepository
                .GetSingleAsync(o => o.Id == monthlyRentBatchId)
                .GetAwaiter().GetResult();
            ;
            if (monthlyRentBatch != null)
            {
                monthlyRentBatch.ProcesssEndDateTime = DateTime.Now;
                _monthlyRentBatchRepository.Commit();
            }
        }

    }
}
