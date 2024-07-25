namespace RicModel.RoomRent.Dtos
{
    public class GuestRoomsOrBedsAvailabilityDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string BookingType { get; set; }
        public string DefaultImage { get; set; }
        public int GuestId { get; set; }

        public bool IsAvailable
        {
            get { return GuestId == 0; }
        }
    }
}
