using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RicEntityFramework.Interfaces;

namespace RicEntityFramework.Services
{
    public class AccountService : IAccountService
    {

        public IList<TimeZoneInfo> SetupTimeZones()
        {
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            return timeZones.ToList();
        }
    }
}
