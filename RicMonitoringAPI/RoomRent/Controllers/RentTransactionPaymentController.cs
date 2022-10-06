using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent.Enumerations;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    //[AllowAnonymous]
    [Authorize(Policy = "SuperAndAdmin")]
    [Route("api/rent-transaction-payments")]
    public class RentTransactionPaymentController : ApiBaseController


    {
        private readonly IRentTransactionPaymentRepository _rentTransactionPaymentRepository;
        private readonly IRentTransactionRepository _rentTransactionRepository;
        private readonly IRenterRepository _renterRepository;

        public RentTransactionPaymentController(
            IRentTransactionPaymentRepository rentTransactionPaymentRepository,
            IRentTransactionRepository rentTransactionRepository,
            IRenterRepository renterRepository)
        {
            _rentTransactionPaymentRepository = rentTransactionPaymentRepository ?? throw new ArgumentNullException(nameof(rentTransactionPaymentRepository));
            _rentTransactionRepository = rentTransactionRepository ?? throw new ArgumentNullException(nameof(rentTransactionRepository));
            _renterRepository = renterRepository ?? throw new ArgumentNullException(nameof(rentTransactionRepository));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, int renterId)
        {
            try
            {
                int rentTransactionId = 0;
                decimal totalPaidAmount = 0;
                decimal totalDueAmount = 0;
                bool isNoAdvanceDepositLeft = false;

                var payment = await _rentTransactionPaymentRepository.GetSingleAsync(o => o.Id == id);

                if (payment == null)
                {
                    return NotFound();
                }

                rentTransactionId = payment.RentTransactionId;

                payment.IsDeleted = true;

                _rentTransactionPaymentRepository.Update(payment);
                _rentTransactionPaymentRepository.Commit();

                //if the deleted payment is UsedDeposit it should return back the count to renter Month Used 
                if (payment.PaymentTransactionType == PaymentTransactionType.DepositUsed)
                {
                    var renter = await _renterRepository.GetSingleAsync(o => o.Id == renterId);
                    if (renter != null)
                    {
                        renter.MonthsUsed = renter.MonthsUsed - 1;
                        _renterRepository.Commit();

                        isNoAdvanceDepositLeft = renter.MonthsUsed >= renter.AdvanceMonths;
                    }
                }

                //sum total payment 
                var transaction = await _rentTransactionRepository.GetSingleAsync(o => o.Id == rentTransactionId);
                var payments = _rentTransactionPaymentRepository.FindBy(o => o.RentTransactionId == rentTransactionId);
                var isDepositUsed = payments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed);
                totalPaidAmount = payments.Sum(o => o.Amount);
                totalDueAmount = transaction.TotalAmountDue;

                if (totalPaidAmount > totalDueAmount)
                {
                    transaction.ExcessPaidAmount = isDepositUsed ? totalPaidAmount : totalPaidAmount - totalDueAmount;
                    transaction.Balance = 0;
                }
                else
                {
                    transaction.Balance = totalDueAmount - totalPaidAmount;
                    transaction.ExcessPaidAmount = 0;
                }

                transaction.PaidAmount = totalPaidAmount;

                _rentTransactionRepository.Commit();

                //message = "Payment has been deleted.";
                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        status = "delete_success",
                        response = new
                        {
                            id,
                            transactionType = payment.PaymentTransactionType.ToString(),
                            isNoAdvanceDepositLeft = isNoAdvanceDepositLeft
                        }
                    },
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                return Ok(HandleApiException(ex.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}