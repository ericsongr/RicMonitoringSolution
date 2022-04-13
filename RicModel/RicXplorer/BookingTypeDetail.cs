
using System;
using System.Collections.Generic;
using RicModel.RoomRent;

namespace RicModel.RicXplorer
{
    public class BookingTypeDetail
    {
        public int Id { get; set; }
        public int BookingTypeId { get; set; }
        public virtual BookingType BookingType { get; set; }
        public int InclusionId { get; set; }
        public virtual LookupTypeItem LookupTypeItem { get; set; }

        public bool IsActive { get; set; }
        public DateTime UtcDateTimeCreated { get; set; }
        public DateTime? UtcDateTimeUpdated { get; set; }

    }
}
