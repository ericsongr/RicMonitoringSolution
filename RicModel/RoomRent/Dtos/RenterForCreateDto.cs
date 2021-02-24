namespace RicModel.RoomRent.Dtos
{
    public class RenterForCreateDto : Renter
    {
        public string StartDateInput { get; set; }
        public string AdvancePaidDateInput { get; set; }
        public string Base64 { get; set; }
    }
}
