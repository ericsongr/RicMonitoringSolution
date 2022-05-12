using System;
using System.ComponentModel.DataAnnotations;
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

        public int GuestBookingDetailId { get; set; }
        public virtual GuestBookingDetail GuestBookingDetail { get; set; }
    }
}
