
using System.Collections.Generic;

namespace RicModel.RicXplorer.Dtos
{
    public class BookingTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }

        public List<BookingTypeInclusionDto> BookingTypeInclusions { get; set; }
        public List<BookingTypeImageDto> BookingTypeImages { get; set; }
    }
}
