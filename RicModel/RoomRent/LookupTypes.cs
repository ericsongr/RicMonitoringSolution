using System.Collections.Generic;

namespace RicModel.RoomRent
{
    public class LookupType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LookupTypeItem> LookupTypeItems { get; set; }
    }
}
