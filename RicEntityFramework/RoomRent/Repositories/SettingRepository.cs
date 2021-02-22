using System;
using RicCommon.Enumeration;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class SettingRepository : EntityBaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(RicDbContext context) : base(context)
        {
        }

        public Setting Get(SettingNameEnum settingName)
        {
            var setting = GetSingleAsync(o => o.Key == settingName.ToString()).GetAwaiter().GetResult();
            if (setting != null)
                return setting;
            else
                throw new Exception($"Setting Name: {settingName} not found.");
        }

        public string GetValue(SettingNameEnum settingName)
        {
            return Get(settingName).Value;
        }
    }
}
