using System.Collections.Generic;
using RicModel.CostMonitoring;
using RicModel.RicXplorer;
using RicModel.ToolsInventory;

namespace RicModel.RoomRent
{
    public class LookupTypeItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int LookupTypeId { get; set; }
        public string Notes { get; set; }

        public virtual LookupType LookupTypes { get; set; }
        public virtual ICollection<GuestBooking> GuestBookings { get; set; }
        public ICollection<BookingTypeInclusion> BookingTypeInclusions { get; set; }

        public virtual ICollection<TransactionCost> TransactionCosts { get; set; }
        public virtual ICollection<ToolInventory> ToolInventoryStatuses { get; set; }
        public virtual ICollection<ToolInventory> ToolInventoryActions { get; set; }
        public virtual ICollection<CheckListForCheckInOutGuest> CheckListForCheckInOutGuests { get; set; }
        public virtual ICollection<GuestBookingDetail> GuestBookingDetails { get; set; }
    }
}
