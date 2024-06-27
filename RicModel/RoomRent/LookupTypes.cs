using RicModel.RicXplorer;
using System.Collections.Generic;

namespace RicModel.RoomRent
{
    public class LookupType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<LookupTypeItem> LookupTypeItems { get; set; }
        public virtual ICollection<CheckListForCheckInOutGuest> CheckListForCheckInOutGuests { get; set; }
    }
}
