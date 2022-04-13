using RicModel.RoomRent;

namespace RicModel.RicXplorer
{
    public class BookedPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Ages { get; set; }
        public virtual LookupTypeItem LookupTypeItem { get; set; }

        public int BookedDetailId { get; set; }
        public BookedDetail BookedDetail { get; set; }
    }
}
