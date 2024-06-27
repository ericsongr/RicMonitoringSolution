
namespace RicModel.RoomRent.Dtos
{
    public class LookupTypeItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int LookupTypeId { get; set; }
        public string Notes { get; set; }
        public string LookupTypeName { get; set; }
    }
}
