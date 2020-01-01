using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RicXplorer.Entities
{
    public class BookedPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Type { get; set; }
        public virtual LookupTypeItems LookupTypeItems { get; set; }

        public int BookedDetailId { get; set; }
        public BookedDetail BookedDetail { get; set; }
    }
}
