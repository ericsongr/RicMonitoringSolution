using System;
using RicMonitoringAPI.Common.Enumeration;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RentTransaction2
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public string RenterName { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal MonthlyRent { get; set; }
        public int DueDay { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Balance { get; set; }
        public decimal PreviousUnpaidAmount { get; set; } //arrears
        public int RentArrearId { get; set; } //arrears
        public DateTime? BalanceDateToBePaid { get; set; }
        public decimal TotalAmountDue { get; set; }
        public bool IsDepositUsed { get; set; }
        public string Note { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public bool IsNoAdvanceDepositLeft { get; set; }

    }
}
