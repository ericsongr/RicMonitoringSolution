using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.RoomRent.Interfaces;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [AllowAnonymous]
    [Route("api/rent-transaction-payments")]
    public class RentTransactionPaymentController : Controller
    {
        private readonly IRentTransactionPaymentRepository _rentTransactionPaymentRepository;
        private readonly IRentTransactionRepository _rentTransactionRepository;

        public RentTransactionPaymentController(
            IRentTransactionPaymentRepository rentTransactionPaymentRepository,
            IRentTransactionRepository rentTransactionRepository)
        {
            _rentTransactionPaymentRepository = rentTransactionPaymentRepository ?? throw new ArgumentNullException(nameof(rentTransactionPaymentRepository));
            _rentTransactionRepository = rentTransactionRepository ?? throw new ArgumentNullException(nameof(rentTransactionRepository));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string message = "";

            bool status = true;
            
            try
            {
                int rentTransactionId = 0;
                decimal totalPaidAmount = 0;
                decimal totalDueAmount = 0;

                var payment = await _rentTransactionPaymentRepository.GetSingleAsync(o => o.Id == id);

                if (payment == null)
                {
                    return NotFound();
                }

                rentTransactionId = payment.RentTransactionId;

                payment.IsDeleted = true;

                _rentTransactionPaymentRepository.Update(payment);
                _rentTransactionPaymentRepository.Commit();

                //sum total payment 
                var transaction = await _rentTransactionRepository.GetSingleAsync(o => o.Id == rentTransactionId);
                var payments = _rentTransactionPaymentRepository.FindBy(o => o.RentTransactionId == rentTransactionId);
                
                totalPaidAmount = payments.Sum(o => o.Amount);
                totalDueAmount = transaction.TotalAmountDue;

                if (totalPaidAmount > totalDueAmount)
                {
                    transaction.ExcessPaidAmount = totalPaidAmount - totalDueAmount;
                    transaction.Balance = 0;
                }
                else
                {
                    transaction.Balance = totalDueAmount - totalPaidAmount;
                    transaction.ExcessPaidAmount = 0;
                }

                transaction.PaidAmount = totalPaidAmount;

                _rentTransactionRepository.Commit();
                

                message = "Payment has been deleted.";
            }
            catch (Exception ex)
            {
                status = false;

                message = ex.Message;
            }
           

            return Ok(new {status, message });
        }
    }
}