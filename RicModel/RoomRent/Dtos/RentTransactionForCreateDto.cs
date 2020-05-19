namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionForCreateDto : RentTransaction
    {
        public int RentArrearId { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal PreviousUnpaidAmount { get; set; }
    }
}
