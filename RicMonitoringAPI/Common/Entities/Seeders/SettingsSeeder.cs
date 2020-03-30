using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using RicMonitoringAPI.Common.Constants;
using RicMonitoringAPI.RicXplorer.Interfaces;
using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.Common.Entities.Seeders
{
    public class SettingsSeeder : ISeeder
    {
        private RoomRentContext _content;

        public void Execute(RoomRentContext context)
        {
            _content = context;

            var settings = GetSettings();
            settings.ForEach(setting => { context.Settings.Add(setting); });
            context.SaveChanges();
        }

        private List<Setting> GetSettings()
        {
            var settings = new List<Setting>();
            var tenantGracePeriodSetting = _content.Settings.SingleOrDefault(o => o.Key == SettingConstant.TenantGracePeriodKey);
            if (tenantGracePeriodSetting == null)
            {
                settings.Add(new Setting
                {
                    Key = SettingConstant.TenantGracePeriodKey,
                    Value = 10.ToString(),
                    FriendlyName = SettingConstant.TenantGracePeriodFriendlyName
                });
            }

            return settings.ToList();
        }
    }
}
