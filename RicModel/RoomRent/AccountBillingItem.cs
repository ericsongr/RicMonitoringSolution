using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicModel.RoomRent.Enumerations;

namespace RicModel.RoomRent
{
    [Serializable]
    public partial class AccountBillingItem
    {
        public long AccountBillingItemId { get; set; }

        public int AccountId { get; set; }

        [Column(TypeName = "money")]
        public decimal BillingAmount { get; set; }

        [StringLength(100)]
        public string BillingReference { get; set; }

        public DateTime CreatedUtcDateTime { get; set; }

        public DateTime? ProcessedUtcDateTime { get; set; }

        public int BillingReason { get; set; }

        [StringLength(50)]
        public string MessageId { get; set; }

        public virtual Account Account { get; set; }

        public PaymentTypes PaymentType { get; set; }
    }
}
