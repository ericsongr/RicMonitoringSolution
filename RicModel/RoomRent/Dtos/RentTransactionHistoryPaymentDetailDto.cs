namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionHistoryPaymentDetailDto
    {
        public decimal Amount { get; set; }
        public string DatePaidString { get; set; }
        public string PaymentTransactionType { get; set; }
    }
}
