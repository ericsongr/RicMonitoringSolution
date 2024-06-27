namespace RicModel.RoomRent.Dtos
{
    public class GuestCheckListDto
    {
        public int LookupId { get; set; }
        public int LookupTypeItemId { get; set; }
        public string Description { get; set; }
        public bool IsIncluded { get; set; }
    }
}
