namespace RicModel.RoomRent.Dtos
{
    public class RenterForUpdateDto : Renter
    {
        public string StartDateInput { get; set; }
        public string AdvancePaidDateInput { get; set; }
    }
}
