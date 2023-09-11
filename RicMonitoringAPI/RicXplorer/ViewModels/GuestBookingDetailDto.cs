using System;
using System.Collections.Generic;

namespace RicMonitoringAPI.RicXplorer.ViewModels
{
    public class GuestBookingDetailDto
    {
        public int Id { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ArrivalDateString { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DepartureDateString { get; set; }
        public string Country { get; set; }
        public string LanguagesSpoken { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string ContactPerson { get; set; }
        public string LeaveMessage { get; set; }
        public int BookingType { get; set; }
        public string BookingTypeName { get; set; }
        public DateTime? CreatedDateTimeUtc { get; set; }
        public string CreatedDateTimeUtcString { get; set; }
        public ICollection<GuestBookingDto> GuestBookings { get; set; }


        public string ArrivalDepartureDate
        {
            get { return ArrivalDate?.ToString("dd-MMM-yyyy") + " to " + DepartureDate?.ToString("dd-MMM-yyyy"); }
        }

    }
}
