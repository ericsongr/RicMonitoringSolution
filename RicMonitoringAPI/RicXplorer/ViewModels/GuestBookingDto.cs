using System;
using RicMonitoringAPI.Infrastructure.Helpers;

namespace RicMonitoringAPI.RicXplorer.ViewModels
{
    public class GuestBookingDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public int Age
        {
            get
            {
                return Birthday == null ? 0 : Calculate.Age(Birthday ?? new DateTime());
            }
        }

        public int Ages { get; set; }

        public string AgesName
        {
            get
            {
                switch (Ages)
                {
                    case 1:
                        return "Adult";
                    case 2:
                        return "Child";
                    case 3:
                        return "Infant";
                    default:
                        return "Adult";
                }
            }
        }

        public int GuestBookingDetailId { get; set; }
    }
}
