using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class LookupTypesMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<LookupType>().HasKey(t => t.Id);

            modelBuilder.Entity<LookupType>()
                .Property(t => t.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<LookupType>().ToTable("LookupTypes");
        }
    }
}
