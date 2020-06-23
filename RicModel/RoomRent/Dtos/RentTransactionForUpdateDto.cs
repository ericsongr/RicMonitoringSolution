namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionForUpdateDto : RentTransaction
    {
        public bool IsAddingPayment { get; set; }
        public bool IsEditingPayment { get; set; }
        public int RentTransactionPaymentId { get; set; }
    }
}
