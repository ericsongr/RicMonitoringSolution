
using RicModel.RoomRent.Dtos;
using System.Collections.Generic;

namespace RicModel.RicXplorer.Dtos
{
    public class GuestBookingTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string OnlinePrice { get; set; }
        public int NoOfPersons { get; set; }
        public int NoOfPersonsMax { get; set; }
        public string BookingUrl { get; set; }
        public string LinkRooms { get; set; } // comma delimited

        public List<AmenityIncludedDto> AmenitiesIncluded { get; set; }
    }
}
