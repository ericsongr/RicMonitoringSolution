using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RicMonitoringAPI.RicXplorer.Entities.Seeders;
using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.Common
{
    public static class DbInitializer
    {
        public static void Initialize(RoomRentContext context)
        {
            new LookupTypesSeeder().Execute(context);
            new LookupTypeItemsSeeder().Execute(context);
        }
    }
}
