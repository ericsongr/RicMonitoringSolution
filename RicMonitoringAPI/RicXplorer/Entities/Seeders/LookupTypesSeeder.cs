using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicMonitoringAPI.Common.Constants;
using RicMonitoringAPI.Common.Enumeration;
using RicMonitoringAPI.RicXplorer.Interfaces;
using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RicXplorer.Entities.Seeders
{
    public class LookupTypesSeeder : ISeeder
    {
        public void Execute(RoomRentContext context)
        {
            var lookupTypes = GetLookupTypes();
            lookupTypes.ForEach(lookupType =>
            {
                var item = context.LookupTypes.SingleOrDefault(o =>
                    o.Name.Trim().ToLower() == lookupType.Name.Trim().ToLower());
                if (item == null)
                {
                    context.LookupTypes.Add(lookupType);
                }
                else
                {
                    item.Name = lookupType.Name.Trim();
                }
                context.SaveChanges();
            });
        }

        private List<LookupType> GetLookupTypes()
        {
            var lookupTypes = new List<LookupType>
            {
                new LookupType {Name = LookupTypeConstant.Ages}
            };

            return lookupTypes.ToList();
        }
    }
}
