using System.Collections.Generic;

namespace RicModel.RoomRent.Dtos
{
    public class GuestRoomTypesAvailabilityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<GuestRoomsOrBedsAvailabilityDto> GuestRoomsOrBeds { get; set; }
    }
}
