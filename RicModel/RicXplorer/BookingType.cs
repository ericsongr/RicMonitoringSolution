
using System;
using System.Collections;
using System.Collections.Generic;

namespace RicModel.RicXplorer
{
    public class BookingType
    {
        public int Id { get; set; }
        public int AccountProductId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public int NoOfPersons { get; set; }
        public int NoOfPersonsMax { get; set; }
        public string BookingUrl { get; set; }
        public DateTime UtcDateTimeCreated { get; set; }
        public DateTime? UtcDateTimeUpdated { get; set; }


        public virtual AccountProduct AccountProduct { get; set; }
        public ICollection<BookingTypeImage> BookingTypeImages { get; set; }
        public ICollection<BookingTypeInclusion> BookingTypeInclusions { get; set; }
        public ICollection<GuestBookingDetail> GuestBookingDetails { get; set; }

    }
}
