using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.RicXplorer.Entities
{
    public class BookedDetail
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string LanguagesSpoken { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string LeaveMessage { get; set; }

        public virtual ICollection<BookedPerson> BookedPersons { get; set; }
    }
}
