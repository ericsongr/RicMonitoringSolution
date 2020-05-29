using System;
using System.Linq;

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
    }
}
