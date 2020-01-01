using RicMonitoringAPI.RicXplorer.Entities;
using System.Collections.Generic;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class LookupTypeItems
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int LookupTypeId { get; set; }
        public virtual LookupType LookupTypes { get; set; }

        public virtual ICollection<BookedPerson> BookedPersons { get; set; }

    }
}
