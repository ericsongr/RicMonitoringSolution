using System;
using System.Collections.Generic;

namespace RicModel.RoomRent.Dtos
{
    public class RentTransaction2Dto : RentTransaction2
    {
        public DateTime DueDate { get; set; }

        public string DueDateString => DueDate.ToString("dd-MMM-yyyy");

        public string DatePaidString => PaidDate?.ToString("dd-MMM-yyyy");

        public string Period { get; set; }

        public List<RentTransactionPaymentDto> Payments { get; set; }

        public BillingStatementDto BillingStatement =>
            new BillingStatementDto
            {
                RenterName = RenterName,
                RoomName = RoomName,
                Period = Period,
                DueDate = DueDateString,
                MonthlyRent = MonthlyRent.ToString("#,###"),
                HasPreviousBalance = PreviousUnpaidAmount > 0,
                PreviousUnpaidAmount = PreviousUnpaidAmount.ToString("#,###"),
                TotalAmountDue = TotalAmountDue.ToString("#,###")
            };

    }
}
