using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RicXplorer.Interfaces
{
    interface ISeeder
    {
        void Execute(RoomRentContext context);
    }
}
