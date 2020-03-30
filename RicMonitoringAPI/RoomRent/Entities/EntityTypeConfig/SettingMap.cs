using Microsoft.EntityFrameworkCore;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class SettingMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Setting>().HasKey(t => t.Id);

            modelBuilder.Entity<Setting>().ToTable("Settings");

        }
    }
}
