using System;
using System.Collections.Generic;
using RicModel.RoomRent;

namespace RicModel.RicXplorer
{
    public class CheckListForCheckInOutGuest
    {
        public int Id { get; set; }
        public int LookupId { get; set; }
        public virtual LookupType LookupType { get; set; }
        
        public int LookupTypeItemId { get; set; }
        public virtual LookupTypeItem LookupTypeItem { get; set; }

        public bool IsIncluded { get; set; }
        
        
    }
}
