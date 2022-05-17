using System;

namespace RicMonitoringAPI.RicXplorer.ViewModels
{
    public class GuestBookingDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public int Age { get; set; }
        public int Ages { get; set; } = 1; //1 - adult TODO: adjustment
        public int GuestBookingDetailId { get; set; }
    }
}
