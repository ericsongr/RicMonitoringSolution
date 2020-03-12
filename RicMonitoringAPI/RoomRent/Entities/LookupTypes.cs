using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class LookupType : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LookupTypeItems> LookupTypeItems { get; set; }
    }
}
