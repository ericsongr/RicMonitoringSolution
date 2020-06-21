using System;
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

            var item = rentTransaction.RentTransactionDetails.FirstOrDefault(o => o.RentArrearId != null);
            if (item != null)
            {
                return item.Amount;
            }
            else
            {
                return 0;
            }
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
