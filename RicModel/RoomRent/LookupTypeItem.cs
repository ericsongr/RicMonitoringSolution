using System.Collections.Generic;
using RicModel.CostMonitoring;
using RicModel.RicXplorer;

namespace RicModel.RoomRent
{
    public class LookupTypeItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int LookupTypeId { get; set; }

        public virtual LookupType LookupTypes { get; set; }
        public virtual ICollection<GuestBooking> GuestBookings { get; set; }
        public ICollection<BookingTypeInclusion> BookingTypeInclusions { get; set; }

        public virtual ICollection<TransactionCost> TransactionCosts { get; set; }
    }
}
