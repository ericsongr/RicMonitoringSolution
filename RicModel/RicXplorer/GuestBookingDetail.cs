using System;
using System.Collections.Generic;
using RicModel.RoomRent;

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
        public DateTime CreatedDateTimeUtc { get; set; }
        public int BookingType { get; set; }
        
        public DateTime? CheckedInDateTime { get; set; }
        public string CheckedInBy { get; set; }
        public DateTime? CheckedOutDateTime { get; set; }
        public string CheckedOutBy { get; set; }

        public virtual BookingType BookingTypeModel { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public virtual ICollection<GuestBooking> GuestBookings { get; set; }
        public virtual ICollection<GuestBookingDate> GuestBookingDates { get; set; }
    }
}
