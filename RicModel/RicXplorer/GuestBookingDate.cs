using System;

namespace RicModel.RicXplorer
{
    public class GuestBookingDate
    {
        public int Id { get; set; }

        public DateTime DateBooked { get; set; }

        public int GuestBookingDetailId { get; set; }
        public virtual GuestBookingDetail GuestBookingDetail { get; set; }
    }
}
