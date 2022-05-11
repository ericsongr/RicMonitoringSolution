using System;
using RicModel.RoomRent;

namespace RicModel.RicXplorer
{
    public class GuestBooking
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int Ages { get; set; }
        public virtual LookupTypeItem LookupTypeItem { get; set; }

        public int BookedDetailId { get; set; }
        public GuestBookingDetail GuestBookingDetail { get; set; }
    }
}
