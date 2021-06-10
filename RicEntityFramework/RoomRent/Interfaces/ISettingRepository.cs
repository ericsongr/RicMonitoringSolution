using System.Collections.Generic;
using RicCommon.Enumeration;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface ISettingRepository : IEntityBaseRepository<Setting>
    {
        List<Setting> GetAll();
        Setting Get(SettingNameEnum settingName);
        string GetValue(SettingNameEnum settingName);
        bool GetBooleanValue(SettingNameEnum settingName);
        int GetIntValue(SettingNameEnum settingName);
    }

}
