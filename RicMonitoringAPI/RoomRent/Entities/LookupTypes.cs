using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class LookupType
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual ICollection<LookupTypeItems> LookupTypeItems { get; set; }
    }
}
