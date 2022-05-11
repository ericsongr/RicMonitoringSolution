using System;
using System.Collections.Generic;

namespace RicModel.RicXplorer
{
    public class GuestBookingDetail
    {
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Country { get; set; }
        public string LanguagesSpoken { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string ContactPerson { get; set; }
        public string LeaveMessage { get; set; }

        public virtual ICollection<GuestBooking> GuestBookings { get; set; }
    }
}
