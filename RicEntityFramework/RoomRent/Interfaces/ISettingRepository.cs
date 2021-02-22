using RicCommon.Enumeration;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface ISettingRepository : IEntityBaseRepository<Setting>
    {

        Setting Get(SettingNameEnum settingName);
        string GetValue(SettingNameEnum settingName);
    }

}
