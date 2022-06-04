
using System.Collections.Generic;

namespace RicModel.RicXplorer.Dtos
{
    public class BookingTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string OnlinePrice { get; set; }
        public int NoOfPersons { get; set; }
        public int NoOfPersonsMax { get; set; }
        public string BookingUrl { get; set; }

        public string NoOfPersonsInclusion
        {
            get
            {
                return NoOfPersons == 1 ? $"{NoOfPersons} Person" : $"{NoOfPersons} Persons";
            }
        }

        public List<BookingTypeInclusionDto> BookingTypeInclusions { get; set; }
        public List<BookingTypeImageDto> BookingTypeImages { get; set; }
    }
}
