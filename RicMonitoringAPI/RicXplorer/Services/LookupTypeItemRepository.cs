using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicMonitoringAPI.RicXplorer.Services.Interfaces;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.Services;

namespace RicMonitoringAPI.RicXplorer.Services
{
    public class LookupTypeItemRepository : EntityBaseRepository<LookupType>, ILookupTypeItemRepository
    {
        public LookupTypeItemRepository(RoomRentContext context) : base(context)
        {
        }
    }
}
