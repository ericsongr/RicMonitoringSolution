using RicMonitoringAPI.Common.Entities.Seeders;
using RicMonitoringAPI.RicXplorer.Entities.Seeders;
using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.Common
{
    public static class DbInitializer
    {
        public static void Initialize(RoomRentContext context)
        {
            new SettingsSeeder().Execute(context);
            new LookupTypesSeeder().Execute(context);
            new LookupTypeItemsSeeder().Execute(context);
        }
    }
}
