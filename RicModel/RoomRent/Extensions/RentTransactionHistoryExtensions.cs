using System;
using System.Collections.Generic;
using System.Linq;
using RicModel.RoomRent.Enumerations;

namespace RicModel.RoomRent.Extensions
{   
    public static class RentTransactionHistoryExtensions
    {
        public static decimal GetPreviousBalance(this RentTransaction rentTransaction)
        {
            if (rentTransaction == null)
            {
                throw new ArgumentNullException("Source");
            }

            decimal totalPreviousBalance = 0;

            var transactionDetail = rentTransaction.RentTransactionDetails.FirstOrDefault(o => o.RentArrearId != null);
            if (transactionDetail != null)
            {
                totalPreviousBalance = transactionDetail.Amount;
            }

            return totalPreviousBalance;

        }

        public static decimal GetManualUnpaidAmountEntry(this ICollection<RentArrear> rentArrears)
        {
            if (rentArrears == null)
            {
                throw new ArgumentNullException("Source");
            }

            var totalManualEntryArrear = rentArrears.Where(o => !o.IsProcessed && o.IsManualEntry).Sum(t => t.UnpaidAmount);

            return totalManualEntryArrear;

        }

        public static string GetPaidOrUsedDepositDate(this RentTransaction rentTransaction)
        {
            if (rentTransaction == null)
            {
                throw new ArgumentNullException("Source");
            }

            string paidOrUsedDepositDate = "";
            var payments = rentTransaction.RentTransactionPayments.ToList();
            if (payments.Any())
            {
                var useDeposit =
                    payments.FirstOrDefault(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed);
                if (useDeposit != null)
                {
                    paidOrUsedDepositDate = useDeposit.DatePaid.ToShortDateString();
                }
                else
                {
                    //paid or carry over payment
                    paidOrUsedDepositDate = rentTransaction.PaidDateString;
                }

            }

            return paidOrUsedDepositDate;
        }

        public static bool CheckIfUsedDeposit(this RentTransaction rentTransaction)
        {
            if (rentTransaction == null)
            {
                throw new ArgumentNullException("source");
            }

            return rentTransaction.RentTransactionPayments.Any(o => o.PaymentTransactionType == PaymentTransactionType.DepositUsed);
        }

        public static decimal GetMonthlyRent(this RentTransaction rentTransaction)
        {
            if (rentTransaction == null)
            {
                throw new ArgumentNullException("Source");
            }

            var monthlyRent = rentTransaction.Room.Price;

            return monthlyRent;
        }

        public static string GetTransactionPaymentType(this RentTransactionPayment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException("Source");
            }

            var name = Enum.GetName(typeof(PaymentTransactionType), payment.PaymentTransactionType);

            //use to add space before capital letter. eg. 'CarryOverExcessPayment' replace with 'Carry Over Excess Payment'
            name = System.Text.RegularExpressions.Regex.Replace(name, "[A-Z]", " $0").TrimStart(); 
            
            return name;
        }
    }
}
