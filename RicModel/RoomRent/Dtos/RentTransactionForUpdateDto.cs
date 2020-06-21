namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionForUpdateDto : RentTransaction
    {
        public bool IsAddingAdvancePayment { get; set; }
    }
}
