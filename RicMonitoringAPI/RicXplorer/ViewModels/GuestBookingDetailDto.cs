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
        public DateTime? CheckedInDateTime { get; set; }
        public string CheckedInBy { get; set; }
        public DateTime? CheckedOutDateTime { get; set; }
        public string CheckedOutBy { get; set; }
        public ICollection<GuestBookingDto> GuestBookings { get; set; }


        public string ArrivalDepartureDate
        {
            get { return ArrivalDate?.ToString("dd-MMM-yyyy") + " to " + DepartureDate?.ToString("dd-MMM-yyyy"); }
        }
        
        public string CheckedInDateTimeString
        {
            get { return CheckedInDateTime?.ToString("dd-MMM-yyyy hh:mm tt"); }
        }
        public string CheckedOutDateTimeString
        {
            get { return CheckedOutDateTime?.ToString("dd-MMM-yyyy hh:mm tt"); }
        }

    }
}
