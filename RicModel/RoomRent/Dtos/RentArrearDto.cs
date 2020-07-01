namespace RicModel.RoomRent.Dtos
{
    public class RentArrearDto
    {
        public int Id { get; set; }
        public int RentTransactionId { get; set; }
        public decimal UnpaidAmount { get; set; }
    }
}
