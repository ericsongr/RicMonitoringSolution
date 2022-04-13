

namespace RicModel.RicXplorer
{
    public class BookingTypeImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public bool IsShow { get; set; }
        public int BookingTypeId { get; set; }
        public virtual BookingType BookingType { get; set; }

    }
}
