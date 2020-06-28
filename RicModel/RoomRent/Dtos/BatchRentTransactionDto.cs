using System;
using System.Collections.Generic;

namespace RicModel.RoomRent.Dtos
{
    public class BatchRentTransactionDto
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public int RoomId { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Balance { get; set; }
        public decimal ExcessPaidAmount { get; set; }
        public DateTime DueDate { get; set; }

        public bool HasMonthDeposit { get; set; }
        public bool IsUsedDeposit { get; set; }
        public bool IsPaidTotalDueAmount { get; set; }

        public RentArrearDto Arrear { get; set; }
    }
}
