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
using RicEntityFramework.Helpers;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.Enumeration;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Enumerations;
using RicMonitoringAPI.Common.Constants;

namespace RicMonitoringAPI.RoomRent.Controllers
{

    //[AllowAnonymous]
    [Authorize(Policy = "ProcessTenantsTransaction")]
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

        [HttpGet]
        public IActionResult GetDailyBatch([FromQuery] string fields)
        {
            var dailyBatch = _monthlyRentBatchRepository.FindAll()
                .OrderByDescending(o =>  o.ProcessStartDateTime);

            var dto = Mapper.Map<IEnumerable<MonthlyRentBatchDto>>(dailyBatch); 

            return Ok(dto.ShapeData(fields));

        }

        [HttpPost()]
        public async Task<IActionResult> ExecRentTransactionBatchFile()
        {
            var currentDateTimeUtc = DateTime.UtcNow;
            var status = DailyBatchStatusConstant.Processing;

            try
            {
                var dailyBatchStatus = _monthlyRentBatchRepository.FindBy(o => o.ProcessStartDateTime.Date == currentDateTimeUtc.Date).ToList();
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
                    int monthlyRentBatchId = InsertRentBatch(currentDateTimeUtc);
                    int tenantGracePeriod = GetTenantGracePeriod();
                    DateTime systemDateTimeProcessed = currentDateTimeUtc;
                    DateTime dateIncludedGracePeriod = currentDateTimeUtc.AddDays(-tenantGracePeriod).Date; //minus days of grace period to current date

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
                        //previous unpaid balance
                        var previousArrearUnpaidBalance = (transaction.Arrear == null ? 0 : transaction.Arrear.UnpaidAmount);

                        //no transaction made, either payment or use deposit
                        if (transaction.PaidAmount == 0 && !transaction.IsUsedDeposit)
                        {
                            DateTime? datePaid;

                            decimal totalAmountDue = transaction.MonthlyRent + previousArrearUnpaidBalance;
                            decimal totalBalance = 0;

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
                                    DatePaid = currentDateTimeUtc,
                                    PaymentTransactionType = PaymentTransactionType.DepositUsed,
                                    RentTransactionId = transaction.Id,
                                });
                                _rentTransactionPaymentRepository.Commit();
                                //END RentTransactionDetails

                                totalBalance = previousArrearUnpaidBalance;
                                datePaid = currentDateTimeUtc;
                            }
                            else
                            {
                                totalBalance = totalAmountDue;
                                datePaid = null;
                            }

                            MarkTransactionAsProcessed(transaction.Id, totalBalance, note, datePaid, true);

                            //INSERT FOR USING THE DEPOSIT
                            // make amount payment 0 because there's a deposit or unpaid amount
                            InsertMonthlyRentToRentTransactionDetail(transaction.Id, 0);

                            //START RentTransactionDetails
                            if (previousArrearUnpaidBalance > 0)
                                InsertArrearInRentTransactionDetail(transaction, previousArrearUnpaidBalance);

                            MarkAsProcessedPreviousTotalBalance(transaction.RenterId, systemDateTimeProcessed);

                            //insert new arrear / unpaid balance
                            if (totalBalance > 0)
                                InsertNewArrear(transaction, totalBalance);

                            //create next billing cycle
                            CreateNewRenterBillingCycle(transaction, systemDateTimeProcessed);
                        }
                        else if (transaction.Balance > 0)
                        {

                            MarkAsProcessedPreviousTotalBalance(transaction.RenterId, systemDateTimeProcessed);

                            MarkTransactionAsProcessed(transaction.Id);

                            InsertMonthlyRentToRentTransactionDetail(transaction.Id, transaction.MonthlyRent);

                            if (previousArrearUnpaidBalance > 0)
                                InsertArrearInRentTransactionDetail(transaction, previousArrearUnpaidBalance);

                            InsertNewArrear(transaction, transaction.Balance ?? 0);

                            //create next billing cycle
                            CreateNewRenterBillingCycle(transaction, systemDateTimeProcessed);
                        }
                        else if (transaction.IsUsedDeposit)
                        {
                            MarkAsProcessedPreviousTotalBalance(transaction.RenterId, systemDateTimeProcessed);

                            MarkTransactionAsProcessed(transaction.Id);

                            InsertMonthlyRentToRentTransactionDetail(transaction.Id, transaction.MonthlyRent);

                            if (previousArrearUnpaidBalance > 0)
                                InsertArrearInRentTransactionDetail(transaction, previousArrearUnpaidBalance);

                            //previous unpaid amount
                            if (previousArrearUnpaidBalance > 0)
                                InsertNewArrear(transaction, previousArrearUnpaidBalance);

                            //create next billing cycle
                            CreateNewRenterBillingCycle(transaction, systemDateTimeProcessed);

                        }
                        else if (transaction.IsPaidTotalDueAmount)
                        {

                            MarkAsProcessedAndFullyPaidArrear(transaction.Id, systemDateTimeProcessed);

                            MarkTransactionAsProcessed(transaction.Id);

                            InsertMonthlyRentToRentTransactionDetail(transaction.Id, transaction.MonthlyRent);

                            if (previousArrearUnpaidBalance > 0)
                                InsertArrearInRentTransactionDetail(transaction, previousArrearUnpaidBalance);

                            //create next billing cycle
                            CreateNewRenterBillingCycle(transaction, systemDateTimeProcessed);

                        }
                    }

                    UpdateMonthlyRentBatch(monthlyRentBatchId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //var renterIds = new List<int> {53,
            //    54,
            //    55,
            //    56,
            //    57,
            //    58,
            //    59};

            //foreach (var renterId in renterIds)
            //{
            //    CreateNewRenterBillingCycle(new BatchRentTransactionDto
            //    {
            //        RenterId = renterId
            //    }, DateTime.Now);
            //}

            //var status = "ok";

            return Ok(new { status });
        }

        /// <summary>
        /// This function use to create new/next billing cycle for renter transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="systemDateTimeProcessed"></param>
        private void CreateNewRenterBillingCycle(BatchRentTransactionDto transaction, DateTime systemDateTimeProcessed)
        {
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
                _renterRepository.Update(updateRenter);
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
                    TotalAmountDue = updateRenter.Room.Price + (transaction.Balance ?? 0) //compute already unpaid balance and 
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

        private void InsertMonthlyRentToRentTransactionDetail(int transactionId, decimal dueAmount)
        {
            _rentTransactionDetailRepository.Add(new RentTransactionDetail
            {
                TransactionId = transactionId,
                Amount = dueAmount,
            });
            _rentTransactionDetailRepository.Commit();
        }

        /// <summary>
        /// Insert unpaid balance or arrears use for payment history of the renter
        /// INSERT DATA ON TRANSACTION DETAIL
        /// INSERT PREVIOUS SAVE ARREAR
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="previousArrearUnpaidBalance"></param>
        private void InsertArrearInRentTransactionDetail(BatchRentTransactionDto transaction, decimal previousArrearUnpaidBalance)
        {
            _rentTransactionDetailRepository.Add(new RentTransactionDetail
            {
                TransactionId = transaction.Id,
                Amount = previousArrearUnpaidBalance,
                RentArrearId = transaction.Arrear.Id
            });
            _rentTransactionDetailRepository.Commit();
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

                if (isSystemProcessed)
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
                IsProcessed = false,
            });
            _rentArrearRepository.Commit();
        }

        /// <summary>
        /// SET THE PREVIOUS TOTAL BALANCE/MANUAL ENTRY IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private void MarkAsProcessedPreviousTotalBalance(int renterId, DateTime processedDateTime)
        {
            //previous balance
            var arrearsPrevious = _rentArrearRepository
                .FindBy(o => o.RenterId == renterId &&
                                     !o.IsProcessed);
            if (arrearsPrevious.Any())
            {
                foreach (var arrear in arrearsPrevious)
                {
                    arrear.IsProcessed = true;
                    arrear.ProcessedDateTime = processedDateTime;
                    _rentArrearRepository.Update(arrear);
                }
                _rentArrearRepository.Commit();
            }
        }

        /// <summary>
        /// SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private void MarkAsProcessedAndFullyPaidArrear(int transactionId, DateTime processedDateTime)
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
                    arrear.ProcessedDateTime = processedDateTime;
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
                monthlyRentBatch.ProcesssEndDateTime = DateTime.UtcNow;
                _monthlyRentBatchRepository.Commit();
            }
        }

    }
}
