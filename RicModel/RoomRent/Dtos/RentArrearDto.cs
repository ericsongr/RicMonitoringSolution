namespace RicModel.RoomRent.Dtos
{
    public class RentArrearDto
    {
        public int RentArrearId { get; set; }
        public int RentTransactionId { get; set; }
        public decimal UnpaidAmount { get; set; }
    }
}
