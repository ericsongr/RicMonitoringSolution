using System.Collections.Generic;
using System.Linq;
using RicMonitoringAPI.Common.Constants;
using RicMonitoringAPI.Common.Enumeration;
using RicMonitoringAPI.RicXplorer.Interfaces;
using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RicXplorer.Entities.Seeders
{
    public class LookupTypeItemsSeeder : ISeeder
    {
        private RoomRentContext _context;

        public void Execute(RoomRentContext context)
        {
            _context = context;

            var lookupTypeItems = GetLookupTypeItems();
            lookupTypeItems.ForEach(lookupTypeItem =>
            {
                var item = context.LookupTypeItems.SingleOrDefault(o =>
                    o.LookupTypes.Name == LookupTypeConstant.Ages &&
                    o.Description.Trim().ToLower() == lookupTypeItem.Description.Trim().ToLower());
                if (item == null)
                {
                    context.LookupTypeItems.Add(lookupTypeItem);
                }
                else
                {
                    item.Description = lookupTypeItem.Description.Trim();
                    item.IsActive = lookupTypeItem.IsActive;
                }
                context.SaveChanges();
            });
        }

        private List<LookupTypeItems> GetLookupTypeItems()
        {
            //ages
            var lookupType = _context.LookupTypes.SingleOrDefault(o => o.Name == LookupTypeEnum.Ages.ToString());
            if (lookupType != null)
            {
                var lookupTypeId = lookupType.Id;
                var lookupTypeItems = new List<LookupTypeItems>
                {
                    new LookupTypeItems {LookupTypeId = lookupTypeId, Description = LookupTypeItemConstant.Adult, IsActive = true},
                    new LookupTypeItems {LookupTypeId = lookupTypeId, Description = LookupTypeItemConstant.Children, IsActive = true},
                    new LookupTypeItems {LookupTypeId = lookupTypeId, Description = LookupTypeItemConstant.Infant, IsActive = true},
                };

                return lookupTypeItems.ToList();
            }

            // it means no ages found
            return null;
        }
    }
}
